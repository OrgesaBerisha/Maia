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

const IMAGES = [
  'https://images.unsplash.com/photo-1509631179647-0177331693ae?w=500&q=80',
  'https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=500&q=80',
]

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
          <img src={IMAGES[0]} alt="" className="profile-img profile-img--top" />
          <img src={IMAGES[1]} alt="" className="profile-img profile-img--bottom" />
        </div>
      </main>

      <BottomNav />
    </div>
  )
}

export default ProfilePage
