import { useState } from 'react'
import { Link } from 'react-router-dom'
import api from './api/axios.js'
import SiteLogo from './SiteLogo.jsx'
import './LoginPage.css'

function ForgotPasswordPage() {
  const [email, setEmail]     = useState('')
  const [message, setMessage] = useState('')
  const [error, setError]     = useState('')
  const [loading, setLoad]    = useState(false)

  const handleSubmit = async e => {
    e.preventDefault()
    setLoad(true)
    setError('')
    setMessage('')
    try {
      const { data } = await api.post('/auth/forgot-password', { email })
      setMessage(data.message)
    } catch {
      setError('Gabim. Provo sërish.')
    } finally {
      setLoad(false)
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <SiteLogo />
        <h2 className="auth-title">FORGOT PASSWORD</h2>

        {message ? (
          <div style={{ textAlign: 'center' }}>
            <p style={{ color: '#4a7c59', marginBottom: '24px' }}>{message}</p>
            <Link to="/login" className="auth-link">← Back to Login</Link>
          </div>
        ) : (
          <form className="auth-form" onSubmit={handleSubmit} noValidate>
            <div className="auth-field">
              <label className="auth-label">EMAIL</label>
              <input
                className="auth-input"
                type="email"
                value={email}
                onChange={e => setEmail(e.target.value)}
                required
                autoFocus
                placeholder="email@example.com"
              />
            </div>

            {error && <p className="auth-error">{error}</p>}

            <button
              type="submit"
              className={`auth-btn${loading ? ' auth-btn--loading' : ''}`}
              disabled={loading}
            >
              {loading ? 'SENDING...' : 'SEND RESET LINK'}
            </button>

            <p className="auth-switch">
              <Link to="/login" className="auth-link">← Back to Login</Link>
            </p>
          </form>
        )}
      </div>
    </div>
  )
}

export default ForgotPasswordPage
