import { useState } from 'react';
import BottomNav from './BottomNav.jsx';
import SiteLogo from './SiteLogo.jsx';
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
          <SiteLogo />
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

      <BottomNav />
    </div>
  );
}

export default ShopPage;
