import { Link, useLocation } from 'react-router-dom';
import { useCart } from './CartContext.jsx';
import './BottomNav.css';

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

function BottomNav() {
  const { pathname } = useLocation();
  const { bag } = useCart();

  return (
    <nav className="bottom-nav">
      <Link to="/"       className={`nav-icon${pathname === '/'       ? ' nav-icon--active' : ''}`} title="Home">
        <IconHome />
      </Link>
      <Link to="/shop"   className={`nav-icon${pathname === '/shop'   ? ' nav-icon--active' : ''}`} title="Menu">
        <IconMenu />
      </Link>
      <Link to="/search" className={`nav-icon${pathname === '/search' ? ' nav-icon--active' : ''}`} title="Search">
        <IconSearch />
      </Link>
      <Link to="/bag"    className={`nav-icon nav-icon--bag${pathname === '/bag' ? ' nav-icon--active' : ''}`} title="Bag">
        <IconBag />
        {bag.length > 0 && <span className="nav-bag-badge">{bag.length}</span>}
      </Link>
      <button className="nav-icon" title="Account">
        <IconUser />
      </button>
    </nav>
  );
}

export default BottomNav;
