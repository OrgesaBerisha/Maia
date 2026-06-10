import { useState, useEffect, useCallback, useMemo } from 'react'
import { useSearchParams, useNavigate } from 'react-router-dom'
import BottomNav from './BottomNav.jsx'
import SizeModal from './SizeModal.jsx'
import SiteLogo from './SiteLogo.jsx'
import { useCart } from './CartContext.jsx'
import { useWishlist } from './WishlistContext.jsx'
import { useAuth } from './AuthContext.jsx'
import api from './api/axios.js'
import './SearchPage.css'

const SECTIONS = ['WOMAN', 'MAN', 'KIDS']

const COLORS = ['Black', 'White', 'Blue', 'Red', 'Pink', 'Grey', 'Navy', 'Beige', 'Brown', 'Green', 'Yellow']

const COLOR_HEX = {
  Black: '#1a1208', White: '#f5f0eb', Blue: '#3b6fa0', Red: '#c0392b',
  Pink: '#e8829e', Grey: '#9a9a9a', Navy: '#1b3a6b', Beige: '#d4b896',
  Brown: '#7a5230', Green: '#3a8a4a', Yellow: '#d4b800',
}

function mapProduct(p, source) {
  const discount = p.discountPercent ?? 0
  const salePrice = discount > 0
    ? (parseFloat(p.price) * (1 - discount / 100)).toFixed(2)
    : null
  return {
    id:             p.id,
    name:           (p.title ?? p.name)?.toUpperCase(),
    price:          p.price,
    priceLabel:     `${p.price} EUR`,
    salePrice,
    discountPercent: discount > 0 ? discount : null,
    category:       (p.womanCategory ?? p.menCategoryName ?? p.kidsCategoryName ?? p.category ?? '').toUpperCase(),
    categoryId:     p.womanCategoryId ?? p.menCategoryId ?? p.kidsCategoryId,
    image:          source === 'KIDS' && (p.imageUrl ?? p.image)
                      ? `/api/imageproxy?url=${encodeURIComponent(p.imageUrl ?? p.image)}`
                      : (p.imageUrl ?? p.image),
    color:          p.color ?? null,
    source,
    productSource:  source.toLowerCase(),
  }
}

async function fetchBySection(section, query) {
  const q = query.trim()
  switch (section) {
    case 'WOMAN': {
      const params = { page: 1, pageSize: 500 }
      if (q) params.search = q
      const { data } = await api.get('/CardsWomen/browse', { params })
      return (data.items ?? data ?? []).map(p => mapProduct(p, 'WOMAN'))
    }
    case 'MAN': {
      const endpoint = q ? '/MenCards/search' : '/MenCards'
      const params = q ? { name: q } : {}
      const { data } = await api.get(endpoint, { params })
      return (Array.isArray(data) ? data : data.items ?? []).map(p => mapProduct(p, 'MAN'))
    }
    case 'KIDS': {
      const endpoint = q ? '/KidsCards/search' : '/KidsCards'
      const params = q ? { name: q } : {}
      const { data } = await api.get(endpoint, { params })
      return (Array.isArray(data) ? data : data.items ?? []).map(p => mapProduct(p, 'KIDS'))
    }
    default: {
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
  const navigate = useNavigate()
  const query                      = searchParams.get('q') ?? ''
  const [section, setSection]     = useState(searchParams.get('section') ?? 'WOMAN')
  const [products, setProducts]   = useState([])
  const [loading, setLoading]     = useState(true)
  const [error, setError]         = useState(null)
  const [modalProduct, setModal] = useState(null)
  const [showFilters, setShowFilters] = useState(false)

  const [selectedColors, setSelectedColors]     = useState([])
  const [selectedCategory, setSelectedCategory] = useState(searchParams.get('category') ?? '')
  const [sortBy, setSortBy]                     = useState('')
  const [searchInput, setSearchInput]           = useState(query)

  const { addToBag } = useCart()
  const { isWishlisted, toggleWishlist } = useWishlist()
  const { isLoggedIn } = useAuth()

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const items = await fetchBySection(section, query)
      setProducts(items)
      setSelectedColors([])
      setSortBy('')
    } catch {
      setError('Could not load products. Is the backend running?')
      setProducts([])
    } finally {
      setLoading(false)
    }
  }, [section, query])

  useEffect(() => {
    setSearchInput(query)
  }, [query])


  useEffect(() => {
    const t = setTimeout(load, 350)
    return () => clearTimeout(t)
  }, [load])

  const changeSection = (sec) => {
    setSection(sec)
    setSelectedCategory('')
    setSearchParams(p => { p.set('section', sec); return p })
  }

  const handleSearch = (e) => {
    e.preventDefault()
    const q = searchInput.trim()
    setSearchParams(p => {
      if (q) p.set('q', q)
      else p.delete('q')
      return p
    })
  }

  const toggleColor = (color) => {
    setSelectedColors(prev =>
      prev.includes(color) ? prev.filter(c => c !== color) : [...prev, color]
    )
  }

  const categories = useMemo(() => {
    const cats = [...new Set(products.map(p => p.category).filter(Boolean))]
    return cats.sort()
  }, [products])

  const filtered = useMemo(() => {
    let list = [...products]

    if (selectedColors.length > 0)
      list = list.filter(p => p.color && selectedColors.includes(p.color))

    if (selectedCategory) {
      if (selectedCategory === 'SALE')
        list = list.filter(p => p.category === 'SALE' || p.discountPercent > 0)
      else
        list = list.filter(p => p.category === selectedCategory)
    }

    if (sortBy === 'price_asc')
      list.sort((a, b) => a.price - b.price)
    else if (sortBy === 'price_desc')
      list.sort((a, b) => b.price - a.price)

    return list
  }, [products, selectedColors, selectedCategory, sortBy])

  const hasActiveFilters = selectedColors.length > 0 || selectedCategory || sortBy

  const clearFilters = () => {
    setSelectedColors([])
    setSelectedCategory('')
    setSortBy('')
  }

  const hasQuery = query.trim().length > 0

  return (
    <div className="search-page">
      <svg className="search-blob" viewBox="0 0 1440 220" preserveAspectRatio="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
        <path d="M0,0 L1440,0 L1440,140 C1340,162 1200,178 1060,164 C920,150 800,118 660,132 C520,146 380,178 240,188 C160,194 80,190 0,196 Z" fill="#d4c5b3" />
      </svg>

      <header className="search-header">
        <div className="search-header-left">
          <button className="collection-menu-btn" onClick={() => navigate(`/shop?section=${section}`)} aria-label="Go back">
            ←
          </button>
          <nav className="section-tabs">
            {SECTIONS.map(sec => (
              <button key={sec} className={`section-tab${section === sec ? ' active' : ''}`} onClick={() => changeSection(sec)}>
                {sec}
                {section === sec && <span className="tab-indicator" />}
              </button>
            ))}
          </nav>
        </div>
        <SiteLogo />
      </header>

      <main className="search-main">
        <form className="search-input-wrap" onSubmit={handleSearch}>
          <input
            className="search-input"
            type="text"
            placeholder="WHAT ARE YOU LOOKING FOR?"
            value={searchInput}
            onChange={e => setSearchInput(e.target.value)}
            spellCheck={false}
          />
          <button type="submit" className="search-submit-btn" aria-label="Search">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <circle cx="11" cy="11" r="8" />
              <line x1="21" y1="21" x2="16.65" y2="16.65" />
            </svg>
          </button>
        </form>

        <div className="filter-bar">
          <button className={`filter-toggle${showFilters ? ' active' : ''}`} onClick={() => setShowFilters(v => !v)}>
            FILTERS {hasActiveFilters ? `(${(selectedColors.length > 0 ? 1 : 0) + (selectedCategory ? 1 : 0) + (sortBy ? 1 : 0)})` : ''}
          </button>

          <div className="sort-inline">
            <button className={`sort-btn${sortBy === 'price_asc' ? ' active' : ''}`} onClick={() => setSortBy(s => s === 'price_asc' ? '' : 'price_asc')}>
              PRICE ↑
            </button>
            <button className={`sort-btn${sortBy === 'price_desc' ? ' active' : ''}`} onClick={() => setSortBy(s => s === 'price_desc' ? '' : 'price_desc')}>
              PRICE ↓
            </button>
          </div>

          {hasActiveFilters && (
            <button className="clear-filters" onClick={clearFilters}>CLEAR ALL</button>
          )}
        </div>

        {showFilters && (
          <div className="filter-panel">
            <div className="filter-section">
              <p className="filter-label">COLOR</p>
              <div className="color-chips">
                {COLORS.map(color => (
                  <button
                    key={color}
                    className={`color-chip${selectedColors.includes(color) ? ' selected' : ''}`}
                    onClick={() => toggleColor(color)}
                    title={color}
                    style={{ '--chip-color': COLOR_HEX[color] ?? '#ccc' }}
                  />
                ))}
              </div>
              {selectedColors.length > 0 && (
                <p className="selected-colors">{selectedColors.join(', ')}</p>
              )}
            </div>

            {categories.length > 0 && (
              <div className="filter-section">
                <p className="filter-label">CATEGORY</p>
                <div className="category-chips">
                  {categories.map(cat => (
                    <button
                      key={cat}
                      className={`cat-chip${selectedCategory === cat ? ' selected' : ''}`}
                      onClick={() => setSelectedCategory(s => s === cat ? '' : cat)}
                    >
                      {cat}
                    </button>
                  ))}
                </div>
              </div>
            )}
          </div>
        )}

        <p className="results-label">
          {loading
            ? 'LOADING...'
            : error
              ? error
              : hasQuery
                ? `${filtered.length} RESULT${filtered.length !== 1 ? 'S' : ''} FOR "${query.toUpperCase()}"`
                : `${section !== 'ALL' ? section + ' · ' : ''}${filtered.length} ITEM${filtered.length !== 1 ? 'S' : ''}`}
        </p>

        {!loading && filtered.length > 0 && (
          <div className="product-grid">
            {filtered.map(product => (
              <div
                key={`${product.source}-${product.id}`}
                className="product-card"
                onClick={() => navigate(`/product/${product.productSource}/${product.id}`, { state: { product } })}
                style={{ cursor: 'pointer' }}
              >
                <div className="product-img-wrap">
                  <img src={product.image} alt={product.name} className="product-img" loading="lazy" />
                  {product.color && (
                    <span className="product-color-dot" style={{ background: COLOR_HEX[product.color] ?? '#ccc' }} title={product.color} />
                  )}
                  {isLoggedIn && (
                    <button
                      className={`product-wishlist${isWishlisted(product.id, product.source) ? ' product-wishlist--active' : ''}`}
                      aria-label={isWishlisted(product.id, product.source) ? 'Remove from wishlist' : 'Add to wishlist'}
                      onClick={e => { e.stopPropagation(); toggleWishlist(product) }}
                    >
                      {isWishlisted(product.id, product.source) ? '♥' : '♡'}
                    </button>
                  )}
                  <button
                    className="product-add"
                    aria-label={`Add ${product.name} to bag`}
                    onClick={e => { e.stopPropagation(); setModal(product) }}
                  >+</button>
                </div>
                <div className="product-info">
                  <span className="product-name">{product.name}</span>
                  {product.salePrice ? (
                    <div className="product-price-block">
                      <span className="product-price product-price--original">{product.priceLabel}</span>
                      <span className="product-price product-price--sale">{product.salePrice} EUR</span>
                    </div>
                  ) : (
                    <span className="product-price">{product.priceLabel}</span>
                  )}
                  {product.color && <span className="product-color-label">{product.color}</span>}
                </div>
              </div>
            ))}
          </div>
        )}

        {!loading && !error && filtered.length === 0 && (
          <p className="no-results">No items found.</p>
        )}
      </main>

      <BottomNav />

      {modalProduct && (
        <SizeModal product={modalProduct} onClose={() => setModal(null)} onAddToBag={addToBag} />
      )}

    </div>
  )
}

export default SearchPage
