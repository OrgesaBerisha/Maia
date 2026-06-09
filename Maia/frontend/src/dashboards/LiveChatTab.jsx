import { useState, useEffect, useRef } from 'react'
import * as signalR from '@microsoft/signalr'
import api from '../api/axios.js'

export default function LiveChatTab() {
  const [conversations, setConversations] = useState([])
  const [selected, setSelected] = useState(null)
  const [messages, setMessages] = useState([])
  const [input, setInput] = useState('')
  const connRef = useRef(null)
  const bottomRef = useRef(null)

  useEffect(() => {
    api.get('/chat/conversations').then(r => setConversations(r.data)).catch(() => {})

    const conn = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5151/hubs/chat', { withCredentials: true })
      .withAutomaticReconnect()
      .build()

    conn.on('ReceiveMessage', (msg) => {
      setMessages(prev => [...prev, msg])
      setConversations(prev => prev.map(c =>
        c.id === msg.conversationId ? { ...c, lastMessageAt: msg.sentAt } : c
      ))
    })

    conn.on('NewConversation', (conv) => {
      setConversations(prev => [conv, ...prev])
    })

    conn.start().then(() => { connRef.current = conn }).catch(() => {})
    return () => conn.stop()
  }, [])

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: 'smooth' })
  }, [messages])

  const openConv = async (conv) => {
    setSelected(conv)
    const r = await api.get(`/chat/conversations/${conv.id}/messages`)
    setMessages(r.data)
  }

  const send = async (e) => {
    e.preventDefault()
    if (!input.trim() || !selected) return
    try {
      const { data } = await api.post(
        `/chat/conversations/${selected.id}/messages`,
        { content: input.trim(), senderName: 'Support' }
      )
      setMessages(prev => [...prev, data.userMessage, data.aiMessage].filter(Boolean))
      setInput('')
    } catch { alert('Failed to send.') }
  }

  const closeConv = async (id) => {
    await api.patch(`/chat/conversations/${id}/close`)
    setConversations(prev => prev.map(c => c.id === id ? { ...c, status: 'Closed' } : c))
    if (selected?.id === id) setSelected(s => ({ ...s, status: 'Closed' }))
  }

  const deleteConv = async (id) => {
    if (!confirm('Delete this conversation?')) return
    await api.delete(`/chat/conversations/${id}`)
    setConversations(prev => prev.filter(c => c.id !== id))
    if (selected?.id === id) { setSelected(null); setMessages([]) }
  }

  return (
    <div className="db-section" style={{ display: 'flex', gap: 16, height: 520 }}>
      {/* Conversation list */}
      <div style={{ width: 260, borderRight: '1px solid #e8e0d8', overflowY: 'auto' }}>
        <div className="db-toolbar"><span className="db-section-title">Conversations</span></div>
        {conversations.length === 0 && <p style={{ padding: 16, color: '#999', fontSize: 13 }}>No active chats.</p>}
        {conversations.map(c => (
          <div key={c.id}
            onClick={() => openConv(c)}
            style={{
              padding: '10px 14px', cursor: 'pointer', borderBottom: '1px solid #f0ebe4',
              background: selected?.id === c.id ? '#f5f0eb' : '#fff',
            }}>
            <div style={{ fontWeight: 600, fontSize: 13 }}>{c.userName}</div>
            <div style={{ fontSize: 11, color: '#999' }}>{c.userEmail}</div>
            <div style={{ display: 'flex', gap: 6, marginTop: 4, alignItems: 'center' }}>
              <span style={{
                fontSize: 10, padding: '1px 6px', borderRadius: 8,
                background: c.status === 'Open' ? '#d4edda' : '#f8d7da',
                color: c.status === 'Open' ? '#155724' : '#721c24'
              }}>{c.status}</span>
              <button
                onClick={e => { e.stopPropagation(); deleteConv(c.id) }}
                style={{ marginLeft: 'auto', background: 'none', border: 'none', cursor: 'pointer', color: '#e63946', fontSize: 12 }}>
                🗑
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Chat window */}
      <div style={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
        {!selected ? (
          <div style={{ flex: 1, display: 'flex', alignItems: 'center', justifyContent: 'center', color: '#999', fontSize: 13 }}>
            Select a conversation to start chatting
          </div>
        ) : (
          <>
            <div style={{ padding: '10px 16px', borderBottom: '1px solid #e8e0d8', display: 'flex', alignItems: 'center', gap: 8 }}>
              <span style={{ fontWeight: 600 }}>{selected.userName}</span>
              <span style={{ fontSize: 12, color: '#999' }}>{selected.userEmail}</span>
              <div style={{ marginLeft: 'auto', display: 'flex', gap: 6 }}>
                {selected.status === 'Open' && (
                  <button className="db-btn db-btn--ghost" style={{ fontSize: 12 }}
                    onClick={() => closeConv(selected.id)}>Close Chat</button>
                )}
                <button className="db-btn db-btn--ghost" style={{ fontSize: 12, color: '#e63946' }}
                  onClick={() => deleteConv(selected.id)}>🗑 Delete</button>
              </div>
            </div>
            <div style={{ flex: 1, overflowY: 'auto', padding: 16, display: 'flex', flexDirection: 'column', gap: 8 }}>
              {messages.map(m => (
                <div key={m.id} style={{ alignSelf: m.isStaff ? 'flex-end' : 'flex-start', maxWidth: '70%' }}>
                  <div style={{ fontSize: 10, color: '#999', marginBottom: 2 }}>{m.senderName}</div>
                  <div style={{
                    background: m.isStaff ? '#1a1208' : '#f2ede8',
                    color: m.isStaff ? '#fff' : '#1a1208',
                    padding: '8px 12px', borderRadius: 12, fontSize: 13
                  }}>{m.content}</div>
                  <div style={{ fontSize: 10, color: '#bbb', marginTop: 2 }}>
                    {new Date(m.sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                  </div>
                </div>
              ))}
              <div ref={bottomRef} />
            </div>
            {selected.status === 'Open' && (
              <form onSubmit={send} style={{ display: 'flex', padding: 10, borderTop: '1px solid #e8e0d8', gap: 8 }}>
                <input
                  value={input} onChange={e => setInput(e.target.value)}
                  placeholder="Reply…"
                  style={{ flex: 1, border: '1px solid #ddd', borderRadius: 20, padding: '8px 14px', fontSize: 13, fontFamily: 'inherit', outline: 'none' }}
                />
                <button type="submit" className="db-btn" style={{ borderRadius: 20 }}>Send</button>
              </form>
            )}
          </>
        )}
      </div>
    </div>
  )
}
