import { useState, useEffect, useCallback } from 'react'
import { useSearchParams } from 'react-router-dom'
import BottomNav from './BottomNav.jsx'
import SizeModal from './SizeModal.jsx'
import SiteLogo from './SiteLogo.jsx'
import { useCart } from './CartContext.jsx'
import api from './api/axios.js'
import './SearchPage.css'

const SECTIONS = ['ALL', 'WOMAN', 'MAN', 'KIDS']

function mapProduct(p) {
  return {
    id:       p.id,
    name:     (p.title ?? p.name)?.toUpperCase(),
    price:    `${p.price} EUR`,
    category: p.category?.toUpperCase(),
    image:    p.imageUrl ?? p.image,
  }
}

async function fetchBySection(section, query) {
  const q = query.trim()
  switch (section) {
    case 'WOMAN': {
      const params = { page: 1, pageSize: 50 }
      if (q) params.search = q
      const { data } = await api.get('/CardsWomen/browse', { params })
      return (data.items ?? data ?? []).map(mapProduct)
    }
    case 'MAN': {
      const endpoint = q ? '/MenCards/search' : '/MenCards'
      const params = q ? { search: q } : {}
      const { data } = await api.get(endpoint, { params })
      return (Array.isArray(data) ? data : data.items ?? []).map(mapProduct)
    }
    case 'KIDS': {
      const endpoint = q ? '/KidsCards/search' : '/KidsCards'
      const params = q ? { search: q } : {}
      const { data } = await api.get(endpoint, { params })
      return (Array.isArray(data) ? data : data.items ?? []).map(mapProduct)
    }
    default: {
      // ALL — fetch from all three in parallel, merge results
      const results = await Promise.allSettled([
        fetchBySection('WOMAN', q),
        fetchBySection('MAN', q),
        fetchBySection('KIDS', q),
      ])
      return results.flatMap(r => r.status === 'fulfilled' ? r.value : [])
    }
  }
}

function SearchPage() {
  const [searchParams, setSearchParams] = useSearchParams()
  const [query, setQuery]       = useState(searchParams.get('q') ?? '')
  const [section, setSection]   = useState(searchParams.get('section') ?? 'ALL')
  const [products, setProducts] = useState([])
  const [loading, setLoading]   = useState(true)
  const [error, setError]       = useState(null)
  const [modalProduct, setModal] = useState(null)

  const { addToBag } = useCart()

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const items = await fetchBySection(section, query)
      setProducts(items)
    } catch {
      setError('Could not load products. Is the backend running?')
      setProducts([])
    } finally {
      setLoading(false)
    }
  }, [section, query])

  useEffect(() => {
    const t = setTimeout(load, 350)
    return () => clearTimeout(t)
  }, [load])

  const changeSection = (sec) => {
    setSection(sec)
    setSearchParams(p => { p.set('section', sec); return p })
  }

  const hasQuery = query.trim().length > 0

  return (
    <div className="search-page">
      <svg
        className="search-blob"
        viewBox="0 0 1440 220"
        preserveAspectRatio="none"
        xmlns="http://www.w3.org/2000/svg"
        aria-hidden="true"
      >
        <path
          d="M0,0 L1440,0 L1440,140
             C1340,162 1200,178 1060,164
             C920,150 800,118 660,132
             C520,146 380,178 240,188
             C160,194 80,190 0,196
             Z"
          fill="#d4c5b3"
        />
      </svg>

      <header className="search-header">
        <nav className="section-tabs">
          {SECTIONS.map(sec => (
            <button
              key={sec}
              className={`section-tab${section === sec ? ' active' : ''}`}
              onClick={() => changeSection(sec)}
            >
              {sec}
              {section === sec && <span className="tab-indicator" />}
            </button>
          ))}
        </nav>
        <SiteLogo />
      </header>

      <main className="search-main">
        <div className="search-input-wrap">
          <input
            className="search-input"
            type="text"
            placeholder="WHAT ARE YOU LOOKING FOR?"
            value={query}
            onChange={e => setQuery(e.target.value)}
            autoFocus
            spellCheck={false}
          />
        </div>

        <p className="results-label">
          {loading
            ? 'LOADING...'
            : error
              ? error
              : hasQuery
                ? `${products.length} RESULT${products.length !== 1 ? 'S' : ''} FOR "${query.toUpperCase()}"`
                : `${section !== 'ALL' ? section + ' · ' : ''}YOU MIGHT BE INTERESTED IN`}
        </p>

        {!loading && products.length > 0 && (
          <div className="product-grid">
            {products.map(product => (
              <div key={`${product.id}-${product.name}`} className="product-card">
                <div className="product-img-wrap">
                  <img
                    src={product.image}
                    alt={product.name}
                    className="product-img"
                    loading="lazy"
                  />
                  <button
                    className="product-add"
                    aria-label={`Add ${product.name} to bag`}
                    onClick={e => { e.preventDefault(); setModal(product) }}
                  >
                    +
                  </button>
                </div>
                <div className="product-info">
                  <span className="product-name">{product.name}</span>
                  <span className="product-price">{product.price}</span>
                </div>
              </div>
            ))}
          </div>
        )}

        {!loading && !error && products.length === 0 && (
          <p className="no-results">No items found.</p>
        )}
      </main>

      <BottomNav />

      {modalProduct && (
        <SizeModal
          product={modalProduct}
          onClose={() => setModal(null)}
          onAddToBag={addToBag}
        />
      )}
    </div>
  )
}

export default SearchPage
