import { useState } from 'react';
import { Link } from 'react-router-dom';
import './ShopPage.css';

const SECTIONS = ['WOMAN', 'MAN', 'KIDS'];

const CATEGORIES = {
  WOMAN: ['VIEW ALL', 'JACKETS', 'BLAZERS', 'T-SHIRTS', 'DRESSES', 'TOPS', 'SHIRTS', 'JEANS', 'SKIRTS', 'LEATHER', 'SHORTS'],
  MAN:   ['VIEW ALL', 'JACKETS', 'BLAZERS', 'T-SHIRTS', 'TROUSERS', 'SHIRTS', 'JEANS', 'SHORTS', 'LEATHER', 'SUITS'],
  KIDS:  ['VIEW ALL', 'T-SHIRTS', 'DRESSES', 'TOPS', 'JEANS', 'SHORTS', 'OUTERWEAR'],
};

const FOOTER_CATS = {
  left:  ['SHOES |', 'ACCESSORIES'],
  right: ['SHOES', 'BAGS', 'JEWELLERY'],
};

function IconHome() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round">
      <path d="M3 9.5L12 3l9 6.5V20a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V9.5z"/>
      <polyline points="9 21 9 12 15 12 15 21"/>
    </svg>
  );
}

function IconMenu() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round">
      <line x1="3" y1="6"  x2="21" y2="6"/>
      <line x1="3" y1="12" x2="21" y2="12"/>
      <line x1="3" y1="18" x2="21" y2="18"/>
    </svg>
  );
}

function IconSearch() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round">
      <circle cx="11" cy="11" r="7"/>
      <line x1="16.5" y1="16.5" x2="22" y2="22"/>
    </svg>
  );
}

function IconBag() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round">
      <path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/>
      <line x1="3" y1="6" x2="21" y2="6"/>
      <path d="M16 10a4 4 0 0 1-8 0"/>
    </svg>
  );
}

function IconUser() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round">
      <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
      <circle cx="12" cy="7" r="4"/>
    </svg>
  );
}

function ShopPage() {
  const [activeSection, setActiveSection] = useState('WOMAN');

  return (
    <div className="shop-page">
      {/* Top blob header */}
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
      </header>

      {/* Category list */}
      <main className="shop-main">
        <div className="categories-grid">
          <div className="categories-col-label">
            <span className="collection-label">COLLECTION</span>
          </div>
          <div className="categories-col-list">
            {CATEGORIES[activeSection].map(cat => (
              <a key={cat} href="#" className="category-item">
                {cat}
              </a>
            ))}
          </div>
        </div>

        <div className="categories-footer-grid">
          <div className="categories-col-label">
            {FOOTER_CATS.left.map(cat => (
              <a key={cat} href="#" className="category-item footer-item">
                {cat}
              </a>
            ))}
          </div>
          <div className="categories-col-list">
            {FOOTER_CATS.right.map(cat => (
              <a key={cat} href="#" className="category-item footer-item">
                {cat}
              </a>
            ))}
          </div>
        </div>
      </main>

      {/* Bottom navigation */}
      <nav className="bottom-nav">
        <Link to="/" className="nav-icon active" title="Home">
          <IconHome />
        </Link>
        <button className="nav-icon" title="Menu">
          <IconMenu />
        </button>
        <button className="nav-icon" title="Search">
          <IconSearch />
        </button>
        <button className="nav-icon" title="Bag">
          <IconBag />
        </button>
        <button className="nav-icon" title="Account">
          <IconUser />
        </button>
      </nav>
    </div>
  );
}

export default ShopPage;
