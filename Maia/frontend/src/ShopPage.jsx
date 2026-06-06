import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './ShopPage.css'

const SECTIONS = ['WOMAN', 'MAN', 'KIDS']

const CATEGORIES = {
  WOMAN: ['VIEW ALL', 'TOPS', 'DRESSES', 'BOTTOMS', 'OUTERWEAR', 'SWIMWEAR', 'MATCHING SETS', 'FOOTWEAR', 'ACCESSORIES', 'SALE'],
  MAN:   ['VIEW ALL', 'TOPS', 'BOTTOMS', 'SUITS & FORMALWEAR', 'OUTERWEAR', 'SWIMWEAR', 'FOOTWEAR', 'ACCESSORIES', 'SALE'],
  KIDS:  ['VIEW ALL', 'T-SHIRTS', 'DRESSES', 'TOPS', 'JEANS', 'SHORTS', 'SALE'],
}

const FOOTER_CATS = {
  left:  ['SHOES |', 'ACCESSORIES'],
  right: ['SHOES', 'BAGS', 'JEWELLERY'],
}

const SECTION_KEY = { WOMAN: 'WOMAN', MAN: 'MAN', KIDS: 'KIDS' }

function ShopPage() {
  const [activeSection, setActiveSection] = useState('WOMAN')
  const [searchQuery, setSearchQuery] = useState('')
  const navigate = useNavigate()

  const goToSearch = (category) => {
    const params = new URLSearchParams({ section: SECTION_KEY[activeSection] })
    if (category !== 'VIEW ALL') params.set('category', category)
    navigate(`/search?${params.toString()}`)
  }

  const handleSearch = (e) => {
    e.preventDefault()
    if (!searchQuery.trim()) return
    const params = new URLSearchParams({ section: SECTION_KEY[activeSection], q: searchQuery.trim() })
    navigate(`/search?${params.toString()}`)
  }

  return (
    <div className="shop-page">
      <header className="shop-header">
        <svg
          className="shop-blob"
          viewBox="0 0 1440 260"
          preserveAspectRatio="none"
          xmlns="http://www.w3.org/2000/svg"
          aria-hidden="true"
        >
          <path
            d="M0,0 L1440,0 L1440,160
               C1360,178 1260,195 1140,182
               C1020,169 900,138 780,152
               C660,166 540,200 420,210
               C300,220 180,215 90,222
               C60,224 30,228 0,232
               Z"
            fill="#d4c5b3"
          />
        </svg>

        <div className="shop-header-content">
          <div className="shop-header-left">
            <button className="collection-menu-btn" onClick={() => navigate('/shop')} aria-label="Collection menu">
              <span /><span /><span />
            </button>
            <nav className="section-tabs">
              {SECTIONS.map(sec => (
                <button
                  key={sec}
                  className={`section-tab${activeSection === sec ? ' active' : ''}`}
                  onClick={() => setActiveSection(sec)}
                >
                  {sec}
                  {activeSection === sec && <span className="tab-indicator" />}
                </button>
              ))}
            </nav>
          </div>
          <SiteLogo />
        </div>
      </header>

      <main className="shop-main">
        <div className="categories-grid">
          <div className="categories-col-label">
            <span className="collection-label">COLLECTION</span>
          </div>
          <div className="categories-col-list">
            {CATEGORIES[activeSection].map(cat => (
              <button
                key={cat}
                className={`category-item${cat === 'SALE' ? ' category-item--sale' : ''}`}
                onClick={() => goToSearch(cat)}
              >
                {cat}
              </button>
            ))}
          </div>
        </div>

        <form className="shop-search-form" onSubmit={handleSearch}>
          <input
            className="shop-search-input"
            type="text"
            placeholder="WHAT ARE YOU LOOKING FOR?"
            value={searchQuery}
            onChange={e => setSearchQuery(e.target.value)}
            spellCheck={false}
          />
        </form>

      </main>

      <BottomNav />
    </div>
  )
}

export default ShopPage
