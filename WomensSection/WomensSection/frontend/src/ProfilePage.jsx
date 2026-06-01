import { Link } from 'react-router-dom';
import BottomNav from './BottomNav.jsx';
import SiteLogo from './SiteLogo.jsx';
import './ProfilePage.css';

const MENU_ITEMS = [
  { label: 'PURCHASES',     to: '/purchases' },
  { label: 'FAVORITES',     to: '/search' },
  { label: 'CONTACT DATA',  to: '/contact' },
  { label: 'STORES',        to: '/stores' },
  { label: 'NOTIFICATIONS', to: '/notifications' },
];

const IMAGES = [
  'https://images.unsplash.com/photo-1509631179647-0177331693ae?w=500&q=80',
  'https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=500&q=80',
];

function ProfilePage() {
  return (
    <div className="profile-page">
      {/* Top blob */}
      <svg
        className="profile-blob"
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

      {/* Header */}
      <header className="profile-header">
        <button className="profile-qr-btn">
          <span className="profile-qr-label">MAIA QR</span>
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6">
            <rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/>
            <rect x="3" y="14" width="7" height="7"/>
            <path d="M14 14h.01M14 18h.01M18 14h.01M18 18h.01M18 21h.01M21 18h.01M21 14h.01"/>
          </svg>
        </button>

        <SiteLogo />

        <Link to="/settings" className="profile-settings-link">SETTINGS</Link>
      </header>

      {/* Body */}
      <main className="profile-main">
        {/* Left — menu */}
        <nav className="profile-menu">
          {MENU_ITEMS.map(item => (
            <Link key={item.label} to={item.to} className="profile-menu-item">
              {item.label}
            </Link>
          ))}
        </nav>

        {/* Right — staggered images */}
        <div className="profile-images">
          <img src={IMAGES[0]} alt="" className="profile-img profile-img--top" />
          <img src={IMAGES[1]} alt="" className="profile-img profile-img--bottom" />
        </div>
      </main>

      <BottomNav />
    </div>
  );
}

export default ProfilePage;
