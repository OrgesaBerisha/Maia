import { Link } from 'react-router-dom'
import { useNotifications } from './NotificationContext.jsx'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './NotificationsPage.css'

function fmtTime(iso) {
  const d = new Date(iso)
  return (
    d.toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) +
    '  ' +
    d.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })
  )
}

function NotificationsPage() {
  const { notifications, markRead, unreadCount } = useNotifications()

  return (
    <div className="notifs-page">
      <svg
        className="notifs-blob"
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

      <header className="notifs-header">
        <Link to="/profile" className="notifs-back">← BACK</Link>
        <SiteLogo />
        <span className="notifs-header-label">NOTIFICATIONS</span>
      </header>

      <main className="notifs-main">
        <div className="notifs-title-row">
          <h1 className="notifs-heading">NOTIFICATIONS</h1>
          {unreadCount > 0 && (
            <span className="notifs-badge">{unreadCount} unread</span>
          )}
        </div>

        {notifications.length === 0 ? (
          <p className="notifs-empty">NO NOTIFICATIONS</p>
        ) : (
          <div className="notifs-list">
            {notifications.map(n => (
              <div
                key={n.id}
                className={`notif-item${n.isRead ? '' : ' notif-item--unread'}`}
                onClick={() => !n.isRead && markRead(n.id)}
                role={n.isRead ? undefined : 'button'}
                tabIndex={n.isRead ? undefined : 0}
                onKeyDown={e => e.key === 'Enter' && !n.isRead && markRead(n.id)}
              >
                <div className="notif-indicator">
                  {!n.isRead && <span className="notif-dot" />}
                </div>
                <div className="notif-content">
                  <p className="notif-message">{n.message}</p>
                  {n.createdAt && (
                    <span className="notif-time">{fmtTime(n.createdAt)}</span>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </main>

      <BottomNav />
    </div>
  )
}

export default NotificationsPage
