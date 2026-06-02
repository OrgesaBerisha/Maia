import { useState, useEffect, useCallback } from 'react';
import BottomNav from './BottomNav.jsx';
import SizeModal from './SizeModal.jsx';
import SiteLogo from './SiteLogo.jsx';
import { useCart } from './CartContext.jsx';
import api from './api/axios.js';
import './SearchPage.css';

const SECTIONS = ['ALL', 'WOMAN', 'MAN', 'KIDS'];

/* map API shape → frontend shape */
function mapProduct(p) {
  return {
    id:       p.id,
    name:     p.title?.toUpperCase(),
    price:    `${p.price} EUR`,
    category: p.category?.toUpperCase(),
    image:    p.imageUrl,
  };
}

function SearchPage() {
  const [query, setQuery]        = useState('');
  const [section, setSection]    = useState('ALL');
  const [products, setProducts]  = useState([]);
  const [loading, setLoading]    = useState(true);
  const [error, setError]        = useState(null);
  const [totalItems, setTotal]   = useState(0);
  const [modalProduct, setModal] = useState(null);

  const { addToBag } = useCart();

  const fetchProducts = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const params = {
        page:     1,
        pageSize: 50,
      };
      if (query.trim()) params.search = query.trim();

      const { data } = await api.get('/CardsWomen/browse', { params });
      const items = (data.items ?? []).map(mapProduct);
      setProducts(items);
      setTotal(data.totalItems ?? items.length);
    } catch (err) {
      setError('Could not load products. Is the backend running?');
      setProducts([]);
    } finally {
      setLoading(false);
    }
  }, [query, section]);

  /* debounce: wait 350ms after user stops typing before fetching */
  useEffect(() => {
    const t = setTimeout(fetchProducts, 350);
    return () => clearTimeout(t);
  }, [fetchProducts]);

  const hasQuery     = query.trim().length > 0;
  const sectionLabel = section === 'ALL' ? '' : `${section} · `;

  return (
    <div className="search-page">
      {/* Top blob */}
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

      {/* Header tabs */}
      <header className="search-header">
        <nav className="section-tabs">
          {SECTIONS.map(sec => (
            <button
              key={sec}
              className={`section-tab${section === sec ? ' active' : ''}`}
              onClick={() => setSection(sec)}
            >
              {sec}
              {section === sec && <span className="tab-indicator" />}
            </button>
          ))}
        </nav>
        <SiteLogo />
      </header>

      <main className="search-main">
        {/* Search input */}
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

        {/* Status label */}
        <p className="results-label">
          {loading
            ? 'LOADING...'
            : error
              ? error
              : hasQuery
                ? `${totalItems} RESULT${totalItems !== 1 ? 'S' : ''} FOR "${query.toUpperCase()}"`
                : `${sectionLabel}YOU MIGHT BE INTERESTED IN`}
        </p>

        {/* Product grid */}
        {!loading && products.length > 0 && (
          <div className="product-grid">
            {products.map(product => (
              <div key={product.id} className="product-card">
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
                    onClick={e => { e.preventDefault(); setModal(product); }}
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
  );
}

export default SearchPage;
