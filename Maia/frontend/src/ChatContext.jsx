import { createContext, useContext, useState, useEffect, useRef } from 'react'
import * as signalR from '@microsoft/signalr'
import api from './api/axios.js'
import { useAuth } from './AuthContext.jsx'

const ChatContext = createContext(null)

export function ChatProvider({ children }) {
  const { user } = useAuth()
  const [conversation, setConversation] = useState(null)
  const [messages, setMessages] = useState([])
  const [open, setOpen] = useState(false)
  const [unread, setUnread] = useState(0)
  const [connecting, setConnecting] = useState(false)
  const [typing, setTyping] = useState(false)
  const connRef = useRef(null)
  const openRef = useRef(false)
  openRef.current = open

  const startChat = async () => {
    if (connRef.current || connecting) return
    setConnecting(true)
    try {
      const { data: conv } = await api.post('/chat/conversations')
      setConversation(conv)

      const { data: msgs } = await api.get(`/chat/conversations/${conv.id}/messages`)
      setMessages(msgs)

      const conn = new signalR.HubConnectionBuilder()
        .withUrl('http://localhost:5151/hubs/chat', { withCredentials: true })
        .withAutomaticReconnect()
        .build()

      // SignalR used only for staff replies pushed from dashboard
      conn.on('StaffReply', (msg) => {
        setMessages(prev => [...prev, msg])
        if (!openRef.current) setUnread(u => u + 1)
      })

      conn.on('ConversationClosed', () => {
        setConversation(c => c ? { ...c, status: 'Closed' } : c)
      })

      await conn.start()
      connRef.current = conn
    } catch (err) {
      console.error('Chat failed:', err)
    } finally {
      setConnecting(false)
    }
  }

  const toggleOpen = () => {
    const next = !open
    setOpen(next)
    setUnread(0)
    if (next && !connRef.current && !connecting) {
      startChat()
    }
  }

  const sendMessage = async (content) => {
    if (!content.trim() || !conversation) return

    // show customer message immediately
    const optimistic = {
      id: Date.now(),
      conversationId: conversation.id,
      senderName: user?.firstName ?? 'You',
      isStaff: false,
      content: content.trim(),
      sentAt: new Date().toISOString(),
    }
    setMessages(prev => [...prev, optimistic])
    setTyping(true)

    try {
      const { data } = await api.post(
        `/chat/conversations/${conversation.id}/messages`,
        { content: content.trim(), senderName: user?.firstName ?? 'You' }
      )
      setMessages(prev => [
        ...prev.filter(m => m.id !== optimistic.id),
        data.userMessage,
        data.aiMessage,
      ])
    } catch (err) {
      console.error('Send failed:', err)
    } finally {
      setTyping(false)
    }
  }

  const deleteConversation = async () => {
    if (!conversation) return
    try {
      await api.delete(`/chat/conversations/${conversation.id}`)
    } catch { /* continue */ }
    connRef.current?.stop()
    connRef.current = null
    setConversation(null)
    setMessages([])
    setOpen(false)
    setUnread(0)
  }

  useEffect(() => {
    return () => { connRef.current?.stop() }
  }, [])

  return (
    <ChatContext.Provider value={{
      conversation, messages, open, unread, connecting, typing,
      toggleOpen, startChat, sendMessage, deleteConversation
    }}>
      {children}
    </ChatContext.Provider>
  )
}

export function useChat() {
  return useContext(ChatContext)
}
