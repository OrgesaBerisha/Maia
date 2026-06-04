import { createContext, useContext, useState, useEffect, useCallback, useRef } from 'react'
import * as signalR from '@microsoft/signalr'
import api from './api/axios.js'
import { useAuth } from './AuthContext.jsx'

const NotificationContext = createContext(null)

export function NotificationProvider({ children }) {
  const { isLoggedIn } = useAuth()
  const [notifications, setNotifications] = useState([])
  const connectionRef = useRef(null)

  const fetchNotifications = useCallback(async () => {
    if (!isLoggedIn) return
    try {
      const { data } = await api.get('/notifications')
      setNotifications(data)
    } catch (err) {
      console.error('[Notifications] fetch failed:', err?.response?.status, err?.message)
    }
  }, [isLoggedIn])

  // Load notifications on login
  useEffect(() => {
    if (isLoggedIn) fetchNotifications()
    else setNotifications([])
  }, [isLoggedIn, fetchNotifications])

  // SignalR connection
  useEffect(() => {
    if (!isLoggedIn) return

    const connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5151/hubs/notifications', {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build()

    connection.on('ReceiveNotification', (notification) => {
      setNotifications(prev => [notification, ...prev])
    })

    connection.start().catch(() => {})
    connectionRef.current = connection

    return () => {
      connection.stop()
    }
  }, [isLoggedIn])

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
