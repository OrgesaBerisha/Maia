import { useState } from 'react'
import { Link, useSearchParams, useNavigate } from 'react-router-dom'
import api from './api/axios.js'
import SiteLogo from './SiteLogo.jsx'
import './LoginPage.css'

function ResetPasswordPage() {
  const [searchParams]          = useSearchParams()
  const navigate                = useNavigate()
  const token                   = searchParams.get('token') ?? ''

  const [password, setPassword] = useState('')
  const [confirm, setConfirm]   = useState('')
  const [error, setError]       = useState('')
  const [loading, setLoad]      = useState(false)

  const handleSubmit = async e => {
    e.preventDefault()
    if (password !== confirm) { setError('Fjalëkalimet nuk përputhen.'); return }
    if (password.length < 6)  { setError('Fjalëkalimi duhet të ketë min 6 karaktere.'); return }

    setLoad(true)
    setError('')
    try {
      await api.post('/auth/reset-password', { token, newPassword: password })
      navigate('/login', { state: { message: 'Fjalëkalimi u ndryshua. Hyr tani.' } })
    } catch (err) {
      setError(err?.response?.data?.message ?? 'Linku është i pavlefshëm ose ka skaduar.')
    } finally {
      setLoad(false)
    }
  }

  if (!token) {
    return (
      <div className="auth-page">
        <div className="auth-card">
          <SiteLogo />
          <p className="auth-error">Link i pavlefshëm.</p>
          <Link to="/login" className="auth-link">← Back to Login</Link>
        </div>
      </div>
    )
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <SiteLogo />
        <h2 className="auth-title">RESET PASSWORD</h2>

        <form className="auth-form" onSubmit={handleSubmit} noValidate>
          <div className="auth-field">
            <label className="auth-label">FJALËKALIMI I RI</label>
            <input
              className="auth-input"
              type="password"
              value={password}
              onChange={e => setPassword(e.target.value)}
              required
              autoFocus
            />
          </div>
          <div className="auth-field">
            <label className="auth-label">KONFIRMO FJALËKALIMIN</label>
            <input
              className="auth-input"
              type="password"
              value={confirm}
              onChange={e => setConfirm(e.target.value)}
              required
            />
          </div>

          {error && <p className="auth-error">{error}</p>}

          <button
            type="submit"
            className={`auth-btn${loading ? ' auth-btn--loading' : ''}`}
            disabled={loading}
          >
            {loading ? 'SAVING...' : 'SAVE NEW PASSWORD'}
          </button>
        </form>
      </div>
    </div>
  )
}

export default ResetPasswordPage
