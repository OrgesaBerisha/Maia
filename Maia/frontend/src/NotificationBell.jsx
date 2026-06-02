import { useState, useRef, useEffect } from 'react'
import { useNotifications } from './NotificationContext.jsx'
import { useAuth } from './AuthContext.jsx'
import './NotificationBell.css'

function IconBell() {
  return (
    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round">
      <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"/>
      <path d="M13.73 21a2 2 0 0 1-3.46 0"/>
    </svg>
  )
}

function NotificationBell() {
  const { isLoggedIn } = useAuth()
  const { notifications, unreadCount, markRead } = useNotifications()
  const [open, setOpen] = useState(false)
  const ref = useRef(null)

  useEffect(() => {
    function handleClick(e) {
      if (ref.current && !ref.current.contains(e.target)) setOpen(false)
    }
    document.addEventListener('mousedown', handleClick)
    return () => document.removeEventListener('mousedown', handleClick)
  }, [])

  if (!isLoggedIn) return null

  return (
    <div className="notif-wrap" ref={ref}>
      <button
        className={`nav-icon notif-btn${open ? ' nav-icon--active' : ''}`}
        onClick={() => setOpen(o => !o)}
        title="Notifications"
      >
        <IconBell />
        {unreadCount > 0 && (
          <span className="notif-badge">{unreadCount > 9 ? '9+' : unreadCount}</span>
        )}
      </button>

      {open && (
        <div className="notif-dropdown">
          <p className="notif-title">NOTIFICATIONS</p>
          {notifications.length === 0 ? (
            <p className="notif-empty">No notifications</p>
          ) : (
            <ul className="notif-list">
              {notifications.map(n => (
                <li
                  key={n.id}
                  className={`notif-item${n.isRead ? ' notif-item--read' : ''}`}
                  onClick={() => markRead(n.id)}
                >
                  <span className="notif-item-title">{n.title}</span>
                  <span className="notif-item-msg">{n.message}</span>
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  )
}

export default NotificationBell
