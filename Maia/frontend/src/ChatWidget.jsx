import { useState, useRef, useEffect } from 'react'
import { useChat } from './ChatContext.jsx'
import './ChatWidget.css'

export default function ChatWidget() {
  const { conversation, messages, open, unread, connecting, typing, toggleOpen, sendMessage, startChat, deleteConversation } = useChat()
  const [input, setInput] = useState('')
  const bottomRef = useRef(null)

  useEffect(() => {
    if (open) bottomRef.current?.scrollIntoView({ behavior: 'smooth' })
  }, [messages, open])

  const handleSend = async (e) => {
    e.preventDefault()
    if (!input.trim()) return
    const text = input
    setInput('')
    await sendMessage(text)
  }

  const isClosed = conversation?.status === 'Closed'

  return (
    <>
      {open && (
        <div className="chat-window">
          <div className="chat-header">
            <span>💬 Support Chat</span>
            {isClosed && <span className="chat-closed-badge">Closed</span>}
            {conversation && (
              <button className="chat-delete-btn" onClick={deleteConversation} title="Delete conversation">🗑</button>
            )}
            <button className="chat-close-btn" onClick={toggleOpen}>✕</button>
          </div>

          <div className="chat-messages">
            {!conversation && !connecting && (
              <p className="chat-empty">Connecting to support…</p>
            )}
            {connecting && <p className="chat-empty">Connecting…</p>}
            {conversation && messages.length === 0 && !connecting && (
              <p className="chat-empty">Hi! How can we help you today?</p>
            )}
            {messages.map((m, i) => (
              <div key={m.id ?? i} className={`chat-msg ${m.isStaff ? 'chat-msg--staff' : 'chat-msg--user'}`}>
                <span className="chat-msg-sender">{m.isStaff ? '🛍️ Support' : 'You'}</span>
                <span className="chat-msg-content">{m.content}</span>
                <span className="chat-msg-time">
                  {new Date(m.sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                </span>
              </div>
            ))}
            {typing && (
              <div className="chat-msg chat-msg--staff">
                <span className="chat-msg-sender">🛍️ Support</span>
                <span className="chat-msg-content chat-typing">● ● ●</span>
              </div>
            )}
            <div ref={bottomRef} />
          </div>

          {conversation && !isClosed && (
            <form className="chat-input-row" onSubmit={handleSend}>
              <input
                className="chat-input"
                value={input}
                onChange={e => setInput(e.target.value)}
                placeholder="Type a message…"
                autoFocus
              />
              <button className="chat-send-btn" type="submit">➤</button>
            </form>
          )}
          {isClosed && <p className="chat-closed-msg">This conversation has been closed.</p>}
        </div>
      )}

      <button className="chat-fab" onClick={toggleOpen}>
        💬
        {unread > 0 && <span className="chat-fab-badge">{unread}</span>}
      </button>
    </>
  )
}
