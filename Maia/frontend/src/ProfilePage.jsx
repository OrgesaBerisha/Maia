import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext.jsx'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './ProfilePage.css'

const MENU_ITEMS = [
  { label: 'PURCHASES',     to: '/purchases' },
  { label: 'REVIEWS',       to: '/reviews' },
  { label: 'CONTACT DATA',  to: '/contact' },
  { label: 'STORES',        to: '/stores' },
  { label: 'NOTIFICATIONS', to: '/notifications' },
]

const DASHBOARD_ROUTES = {
  Admin:         '/dashboard/admin',
  SalesManager:  '/dashboard/sales',
  WomenManager:  '/dashboard/women',
  MenManager:    '/dashboard/men',
  KidsManager:   '/dashboard/kids',
}

const HERO_IMG  = 'https://dsh6y5eym1jrl.cloudfront.net/_next/image?url=https%3A%2F%2Fd166chel5lrjm5.cloudfront.net%2Fimages%2Fhome%2Fimages%2Fdesktop%2F20260604_17.jpg&w=3840&q=100'
const KIDS_IMG  = 'https://static.zara.net/assets/public/fbc9/255b/8da942ca84e9/cad7713c6a46/08350547500201-ult1/08350547500201-ult1.jpg?ts=1776932587261&w=1381'

function ProfilePage() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = async () => {
    await logout()
    navigate('/login')
  }

  return (
    <div className="profile-page">
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
        <span className="profile-settings-link">
          {user?.firstName && user?.lastName
            ? `${user.firstName} ${user.lastName}`
            : user?.email ?? ''}
        </span>
      </header>

      <main className="profile-main">
        <nav className="profile-menu">
          {MENU_ITEMS.map(item => (
            <Link key={item.label} to={item.to} className="profile-menu-item">
              {item.label}
            </Link>
          ))}
          {DASHBOARD_ROUTES[user?.role] && (
            <Link to={DASHBOARD_ROUTES[user.role]} className="profile-menu-item profile-dashboard-btn">
              DASHBOARD
            </Link>
          )}
          <button className="profile-menu-item profile-logout-btn" onClick={handleLogout}>
            LOG OUT
          </button>
        </nav>

        <div className="profile-images">
          <img src={HERO_IMG} alt="" className="profile-img profile-img--top" />
          <img src={KIDS_IMG} alt="" className="profile-img profile-img--bottom" />
        </div>
      </main>

      <BottomNav />
    </div>
  )
}

export default ProfilePage
