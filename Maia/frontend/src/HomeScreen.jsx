import { useState, useEffect, useRef } from 'react'
import { Link } from 'react-router-dom'
import { useCart } from './CartContext.jsx'
import { useAuth } from './AuthContext.jsx'
import BottomNav from './BottomNav.jsx'
import './HomeScreen.css'

const HERO_SLIDES = [
  {
    src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_03.jpg&w=3840&q=100',
    tag: 'NEW COLLECTION',
    title: 'The New\nEdit',
    cta: 'Shop Women',
    href: '/shop?section=WOMAN',
  },
  {
    src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_02.jpg&w=3840&q=100',
    tag: 'SUMMER 2025',
    title: 'Summer\nEssentials',
    cta: 'Discover Now',
    href: '/shop',
  },
  {
    src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_05.jpg&w=3840&q=100',
    tag: 'EXCLUSIVE STYLES',
    title: 'Made\nFor You',
    cta: 'Shop Now',
    href: '/shop',
  },
  {
    src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_01.jpg&w=3840&q=100',
    tag: 'LATEST DROPS',
    title: 'New\nArrivals',
    cta: 'Shop Now',
    href: '/shop',
  },
  {
    src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_02.jpg&w=3840&q=100',
    tag: 'THE EVENING EDIT',
    title: 'Dressed\nTo Shine',
    cta: 'Explore',
    href: '/shop?section=WOMAN',
  },
]

const WOMEN_ARRIVALS = [
  { src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-portia-jade-crepe-strapless-maxi-dress-with-detachable-georgette-cape-front-view-1.jpg&w=3840&q=100', name: 'Portia Maxi Dress' },
  { src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-charlotte-bronze-chiffon-mesh-strapless-mini-dress-front-view-1.jpg&w=3840&q=100', name: 'Charlotte Mini' },
  { src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-sareena-pear-satin-embellished-fringed-maxi-dress-front-view-11.jpg&w=3840&q=100', name: 'Sareena Satin Maxi' },
  { src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-charlotte-bronze-chiffon-mesh-strapless-maxi-dress-front-view-1.jpg&w=3840&q=100', name: 'Charlotte Maxi' },
  { src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-chantelle-jade-silk-organza-open-back-mini-dress-front-view-1.jpg&w=3840&q=100', name: 'Chantelle Silk Mini' },
]

const MEN_IMAGES = [
  'https://cdn.sanity.io/images/0v989dzu/production/4a57d4d20f49e88ae6e586062207d1c5262f9996-918x1284.jpg?w=450&h=629&q=100&crop=center&auto=format',
  'https://cdn.sanity.io/images/0v989dzu/production/79fec4340ed60034eb14c3a723f7014adb0a7199-918x1284.jpg?w=450&h=629&q=100&crop=center&auto=format',
  'https://cdn.sanity.io/images/0v989dzu/production/f522ebce1aad0ad349fa25088ce9a55a0a9c44f0-918x1284.jpg?w=450&h=629&q=100&crop=center&auto=format',
  'https://cdn.sanity.io/images/0v989dzu/production/53c54bceadf606fa25d2ddd6c0ac8358f120dccb-918x1284.jpg?w=450&h=629&q=100&crop=center&auto=format',
]

const BANNER_SRC = 'https://cdn.sanity.io/images/0v989dzu/production/3d2608e9bcc541e791ac85454d76fdee100fa46e-2880x1140.jpg?w=1920&q=100&crop=center&auto=format'

const CATS = [
  {
    label: 'WOMEN',
    sub: 'New In',
    href: '/shop?section=WOMAN',
    src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fdetailed%2F122%2Fhouse-of-cb-portia-jade-crepe-strapless-maxi-dress-with-detachable-georgette-cape-front-view-1.jpg&w=3840&q=100',
  },
  {
    label: 'MEN',
    sub: 'New In',
    href: '/shop?section=MAN',
    src: 'https://cdn.sanity.io/images/0v989dzu/production/4a57d4d20f49e88ae6e586062207d1c5262f9996-918x1284.jpg?w=450&h=629&q=100&crop=center&auto=format',
  },
  {
    label: 'KIDS',
    sub: 'New In',
    href: '/shop?section=KIDS',
    src: 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fslider%2F20260604_05.jpg&w=3840&q=100',
  },
]

function SearchIcon() {
  return (
    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round">
      <circle cx="11" cy="11" r="7"/>
      <line x1="16.5" y1="16.5" x2="22" y2="22"/>
    </svg>
  )
}

function BagIcon() {
  return (
    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round">
      <path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/>
      <line x1="3" y1="6" x2="21" y2="6"/>
      <path d="M16 10a4 4 0 0 1-8 0"/>
    </svg>
  )
}

function UserIcon() {
  return (
    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round">
      <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
      <circle cx="12" cy="7" r="4"/>
    </svg>
  )
}

export default function HomeScreen() {
  const { bag } = useCart()
  const { isLoggedIn } = useAuth()
  const [current, setCurrent] = useState(0)
  const [textVisible, setTextVisible] = useState(true)
  const [navVisible, setNavVisible] = useState(false)
  const timerRef = useRef(null)

  useEffect(() => {
    const onScroll = () => {
      const nearBottom = window.innerHeight + window.scrollY >= document.documentElement.scrollHeight - 80
      setNavVisible(nearBottom)
    }
    window.addEventListener('scroll', onScroll, { passive: true })
    return () => window.removeEventListener('scroll', onScroll)
  }, [])

  const goTo = (idx) => {
    setTextVisible(false)
    setTimeout(() => {
      setCurrent((idx + HERO_SLIDES.length) % HERO_SLIDES.length)
      setTextVisible(true)
    }, 180)
  }

  const advance = () => goTo(current + 1)

  useEffect(() => {
    timerRef.current = setInterval(advance, 5000)
    return () => clearInterval(timerRef.current)
  }, [current])

  const resetTimer = () => {
    clearInterval(timerRef.current)
    timerRef.current = setInterval(advance, 5000)
  }

  const prev = () => { goTo(current - 1); resetTimer() }
  const next = () => { goTo(current + 1); resetTimer() }

  const slide = HERO_SLIDES[current]

  return (
    <div className="hs-root">

      {/* ── Top Nav ──────────────────────────────────────────── */}
      <header className="hs-nav">
        <nav className="hs-nav-section hs-nav-left">
          <Link to="/shop?section=WOMAN" className="hs-nav-link">Women</Link>
          <Link to="/shop?section=MAN"   className="hs-nav-link">Men</Link>
          <Link to="/shop?section=KIDS"  className="hs-nav-link">Kids</Link>
        </nav>

        <Link to="/" className="hs-nav-logo">MAIA</Link>

        <div className="hs-nav-section hs-nav-right">
          <Link to="/search" className="hs-nav-icon-btn" aria-label="Search"><SearchIcon /></Link>
          <Link to="/bag"    className="hs-nav-icon-btn hs-nav-bag" aria-label="Bag">
            <BagIcon />
            {bag.length > 0 && <span className="hs-nav-badge">{bag.length}</span>}
          </Link>
          <Link to={isLoggedIn ? '/profile' : '/login'} className="hs-nav-icon-btn" aria-label="Account"><UserIcon /></Link>
        </div>
      </header>

      {/* ── Hero Slider ───────────────────────────────────────── */}
      <section className="hs-hero">
        <div className="hs-slides-track" style={{ transform: `translateX(-${current * 100}%)` }}>
          {HERO_SLIDES.map((s, i) => (
            <div key={i} className="hs-slide">
              <img src={s.src} alt="" loading={i === 0 ? 'eager' : 'lazy'} />
            </div>
          ))}
        </div>

        <div className="hs-slide-gradient" />

        <div className={`hs-hero-text${textVisible ? ' hs-hero-text--in' : ''}`}>
          <span className="hs-hero-tag">{slide.tag}</span>
          <h1 className="hs-hero-title">{slide.title}</h1>
          <Link to={slide.href} className="hs-hero-cta">{slide.cta}</Link>
        </div>

        <button className="hs-arrow hs-arrow--l" onClick={prev} aria-label="Previous">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="15 18 9 12 15 6"/></svg>
        </button>
        <button className="hs-arrow hs-arrow--r" onClick={next} aria-label="Next">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="9 18 15 12 9 6"/></svg>
        </button>

        <div className="hs-dots">
          {HERO_SLIDES.map((_, i) => (
            <button
              key={i}
              className={`hs-dot${i === current ? ' hs-dot--on' : ''}`}
              onClick={() => { goTo(i); resetTimer() }}
              aria-label={`Slide ${i + 1}`}
            />
          ))}
        </div>
      </section>

      {/* ── Shop by Category ──────────────────────────────────── */}
      <section className="hs-cats">
        {CATS.map(cat => (
          <Link key={cat.label} to={cat.href} className="hs-cat">
            <img src={cat.src} alt={cat.label} loading="lazy" />
            <div className="hs-cat-overlay">
              <span className="hs-cat-sub">{cat.sub}</span>
              <span className="hs-cat-label">{cat.label}</span>
            </div>
          </Link>
        ))}
      </section>

      {/* ── New Arrivals ──────────────────────────────────────── */}
      <section className="hs-section">
        <div className="hs-section-head">
          <h2 className="hs-section-title">New Arrivals</h2>
          <Link to="/shop?section=WOMAN" className="hs-view-all">View All</Link>
        </div>
        <div className="hs-scroll">
          {WOMEN_ARRIVALS.map((item, i) => (
            <Link key={i} to="/shop?section=WOMAN" className="hs-product-card">
              <div className="hs-product-img-wrap">
                <img src={item.src} alt={item.name} loading="lazy" />
              </div>
              <div className="hs-product-info">
                <span className="hs-product-name">{item.name}</span>
                <span className="hs-product-action">Shop Now</span>
              </div>
            </Link>
          ))}
        </div>
      </section>

      {/* ── Men's Edit ────────────────────────────────────────── */}
      <section className="hs-men-section">
        <div className="hs-men-head">
          <div>
            <p className="hs-men-tag">The Edit</p>
            <h2 className="hs-section-title">Men's Collection</h2>
          </div>
          <Link to="/shop?section=MAN" className="hs-view-all">View All</Link>
        </div>
        <div className="hs-men-grid">
          {MEN_IMAGES.map((src, i) => (
            <Link key={i} to="/shop?section=MAN" className="hs-men-card">
              <img src={src} alt={`Men look ${i + 1}`} loading="lazy" />
            </Link>
          ))}
        </div>
      </section>

      {/* ── Full-Width Banner ─────────────────────────────────── */}
      <section className="hs-banner">
        <img src={BANNER_SRC} alt="Maia Collection" loading="lazy" />
        <div className="hs-banner-overlay">
          <span className="hs-banner-tag">MAIA — SEASON 2025</span>
          <h2 className="hs-banner-title">The New Season</h2>
          <Link to="/shop" className="hs-banner-cta">Discover The Collection</Link>
        </div>
      </section>

      {/* ── Bottom Nav ────────────────────────────────────────── */}
      <div className={`hs-bottom-nav${navVisible ? ' hs-bottom-nav--visible' : ''}`}>
        <BottomNav />
      </div>
    </div>
  )
}
