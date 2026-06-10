import { useState, useEffect } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import api from './api/axios.js'
import './ShopPage.css'

const SECTIONS = ['WOMAN', 'MAN', 'KIDS']

const CATEGORIES = {
  WOMAN: ['VIEW ALL', 'TOPS', 'DRESSES', 'BOTTOMS', 'OUTERWEAR', 'SWIMWEAR', 'MATCHING SETS', 'FOOTWEAR', 'ACCESSORIES', 'SALE'],
  MAN:   ['VIEW ALL', 'TOPS', 'BOTTOMS', 'SUITS & FORMALWEAR', 'OUTERWEAR', 'SWIMWEAR', 'FOOTWEAR', 'ACCESSORIES', 'SALE'],
  KIDS:  ['VIEW ALL', 'BABY', 'GIRLS', 'BOYS', 'SWIMWEAR', 'FOOTWEAR', 'ACCESSORIES', 'SALE'],
}

const SECTION_KEY = { WOMAN: 'WOMAN', MAN: 'MAN', KIDS: 'KIDS' }

const SECTION_ENDPOINT = { WOMAN: '/CardsWomen', MAN: '/MenCards', KIDS: '/KidsCards' }

const SECTION_TAG = { WOMAN: 'New Season', MAN: "Men's Edit", KIDS: 'Kids Collection' }

const FALLBACK_IMAGES = {
  WOMAN: [
    'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-portia-jade-crepe-strapless-maxi-dress-with-detachable-georgette-cape-front-view-1.jpg&w=3840&q=100',
    'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-charlotte-bronze-chiffon-mesh-strapless-mini-dress-front-view-1.jpg&w=3840&q=100',
    'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-chantelle-jade-silk-organza-open-back-mini-dress-front-view-1.jpg&w=3840&q=100',
  ],
  MAN: [
    'https://cdn.sanity.io/images/0v989dzu/production/4a57d4d20f49e88ae6e586062207d1c5262f9996-918x1284.jpg?w=450&h=629&q=100&crop=center&auto=format',
    'https://cdn.sanity.io/images/0v989dzu/production/79fec4340ed60034eb14c3a723f7014adb0a7199-918x1284.jpg?w=450&h=629&q=100&crop=center&auto=format',
    'https://cdn.sanity.io/images/0v989dzu/production/f522ebce1aad0ad349fa25088ce9a55a0a9c44f0-918x1284.jpg?w=450&h=629&q=100&crop=center&auto=format',
  ],
  KIDS: [
    'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_05.jpg&w=3840&q=100',
    'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_01.jpg&w=3840&q=100',
    'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_03.jpg&w=3840&q=100',
  ],
}

function ShopPage() {
  const [searchParams] = useSearchParams()
  const [activeSection, setActiveSection] = useState(searchParams.get('section') ?? 'WOMAN')
  const [searchQuery, setSearchQuery] = useState('')
  const [editorialImages, setEditorialImages] = useState({
    WOMAN: FALLBACK_IMAGES.WOMAN,
    MAN:   FALLBACK_IMAGES.MAN,
    KIDS:  FALLBACK_IMAGES.KIDS,
  })
  const navigate = useNavigate()

  useEffect(() => {
    async function loadImages(section) {
      try {
        const r = await api.get(SECTION_ENDPOINT[section])
        const products = r.data ?? []
        const imgs = products
          .filter(p => p.imageUrl)
          .map(p => p.imageUrl)
          .slice(0, 3)
        if (imgs.length >= 3) {
          setEditorialImages(prev => ({ ...prev, [section]: imgs }))
        }
      } catch {
        // keep fallback
      }
    }
    loadImages('WOMAN')
    loadImages('MAN')
    loadImages('KIDS')
  }, [])

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

  const imgs = editorialImages[activeSection]

  return (
    <div className="shop-page">
      <header className="shop-header">
        <svg className="shop-blob" viewBox="0 0 1440 260" preserveAspectRatio="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
          <path d="M0,0 L1440,0 L1440,160 C1360,178 1260,195 1140,182 C1020,169 900,138 780,152 C660,166 540,200 420,210 C300,220 180,215 90,222 C60,224 30,228 0,232 Z" fill="#d4c5b3" />
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
        <div className="shop-body">

          {/* Left: label + categories */}
          <div className="categories-area">
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
          </div>

          {/* Right: editorial images */}
          <div className="shop-editorial">
            <span className="shop-editorial-tag">{SECTION_TAG[activeSection]}</span>
            <div className="shop-editorial-grid">
              <div className="shop-ed-main">
                <img src={imgs[0]} alt="" loading="lazy" />
              </div>
              <div className="shop-ed-stack">
                <div className="shop-ed-small">
                  <img src={imgs[1]} alt="" loading="lazy" />
                </div>
                <div className="shop-ed-small">
                  <img src={imgs[2]} alt="" loading="lazy" />
                  <div className="shop-ed-cta-wrap">
                    <button
                      className="shop-ed-cta"
                      onClick={() => navigate(`/search?section=${SECTION_KEY[activeSection]}`)}
                    >
                      Shop Now
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>

        </div>
      </main>

      <BottomNav />
    </div>
  )
}

export default ShopPage
