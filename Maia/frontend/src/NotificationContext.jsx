import { createContext, useContext, useState, useEffect, useCallback } from 'react'
import api from './api/axios.js'
import { useAuth } from './AuthContext.jsx'

const NotificationContext = createContext(null)

export function NotificationProvider({ children }) {
  const { isLoggedIn } = useAuth()
  const [notifications, setNotifications] = useState([])

  const fetchNotifications = useCallback(async () => {
    if (!isLoggedIn) return
    try {
      const { data } = await api.get('/notifications')
      setNotifications(data)
    } catch {
      // silent fail
    }
  }, [isLoggedIn])

  useEffect(() => {
    fetchNotifications()
    const interval = setInterval(fetchNotifications, 30000)
    return () => clearInterval(interval)
  }, [fetchNotifications])

  const markRead = useCallback(async (id) => {
    try {
      await api.patch(`/notifications/${id}/read`)
      setNotifications(prev =>
        prev.map(n => n.id === id ? { ...n, isRead: true } : n)
      )
    } catch {
      // silent fail
    }
  }, [])

  const unreadCount = notifications.filter(n => !n.isRead).length

  return (
    <NotificationContext.Provider value={{ notifications, unreadCount, markRead, fetchNotifications }}>
      {children}
    </NotificationContext.Provider>
  )
}

export function useNotifications() {
  return useContext(NotificationContext)
}
