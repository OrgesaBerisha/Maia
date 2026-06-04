import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext.jsx'
import SiteLogo from './SiteLogo.jsx'
import './LoginPage.css'

function LoginPage() {
  const { login } = useAuth()
  const navigate  = useNavigate()

  const [form, setForm]    = useState({ email: '', password: '' })
  const [error, setError]  = useState('')
  const [loading, setLoad] = useState(false)

  const handleChange = e => setForm(f => ({ ...f, [e.target.name]: e.target.value }))

  const handleSubmit = async e => {
    e.preventDefault()
    setError('')
    setLoad(true)
    try {
      const data = await login(form.email, form.password)
      const role = data?.role ?? ''
      const roleRoutes = {
        Admin:         '/dashboard/admin',
        SalesManager:  '/dashboard/sales',
        WomenManager:  '/dashboard/women',
        MenManager:    '/dashboard/men',
        KidsManager:   '/dashboard/kids',
      }
      navigate(roleRoutes[role] ?? '/')
    } catch (err) {
      const data = err?.response?.data
      if (data?.errors) {
        const msgs = Object.values(data.errors).flat().join(' ')
        setError(msgs)
      } else {
        setError(data?.message ?? 'Invalid email or password.')
      }
    } finally {
      setLoad(false)
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <SiteLogo />
        <h2 className="auth-title">SIGN IN</h2>

        <form className="auth-form" onSubmit={handleSubmit} noValidate>
          <div className="auth-field">
            <label className="auth-label">EMAIL</label>
            <input className="auth-input" type="email" name="email"
              value={form.email} onChange={handleChange} required autoFocus />
          </div>
          <div className="auth-field">
            <label className="auth-label">PASSWORD</label>
            <input className="auth-input" type="password" name="password"
              value={form.password} onChange={handleChange} required />
          </div>

          {error && <p className="auth-error">{error}</p>}

          <button type="submit" className={`auth-btn${loading ? ' auth-btn--loading' : ''}`} disabled={loading}>
            {loading ? 'SIGNING IN...' : 'SIGN IN'}
          </button>
        </form>

        <p className="auth-switch">
          Don't have an account?{' '}
          <Link to="/register" className="auth-link">Register</Link>
        </p>
        <p className="auth-switch">
          <Link to="/forgot-password" className="auth-link">Forgot password?</Link>
        </p>
      </div>
    </div>
  )
}

export default LoginPage
