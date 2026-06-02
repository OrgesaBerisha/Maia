import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from './AuthContext.jsx'
import SiteLogo from './SiteLogo.jsx'
import './LoginPage.css'

function RegisterPage() {
  const { register } = useAuth()
  const navigate     = useNavigate()

  const [form, setForm]    = useState({ firstName: '', lastName: '', email: '', password: '' })
  const [error, setError]  = useState('')
  const [loading, setLoad] = useState(false)

  const handleChange = e => setForm(f => ({ ...f, [e.target.name]: e.target.value }))

  const handleSubmit = async e => {
    e.preventDefault()
    setError('')
    setLoad(true)
    try {
      await register(form.firstName, form.lastName, form.email, form.password)
      navigate('/login')
    } catch (err) {
      setError(err?.response?.data?.message ?? 'Registration failed. Try again.')
    } finally {
      setLoad(false)
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <SiteLogo />
        <h2 className="auth-title">CREATE ACCOUNT</h2>

        <form className="auth-form" onSubmit={handleSubmit} noValidate>
          <div className="auth-row">
            <div className="auth-field">
              <label className="auth-label">FIRST NAME</label>
              <input className="auth-input" type="text" name="firstName"
                value={form.firstName} onChange={handleChange} required autoFocus />
            </div>
            <div className="auth-field">
              <label className="auth-label">LAST NAME</label>
              <input className="auth-input" type="text" name="lastName"
                value={form.lastName} onChange={handleChange} required />
            </div>
          </div>
          <div className="auth-field">
            <label className="auth-label">EMAIL</label>
            <input className="auth-input" type="email" name="email"
              value={form.email} onChange={handleChange} required />
          </div>
          <div className="auth-field">
            <label className="auth-label">PASSWORD</label>
            <input className="auth-input" type="password" name="password"
              value={form.password} onChange={handleChange} required minLength={6} />
          </div>

          {error && <p className="auth-error">{error}</p>}

          <button type="submit" className={`auth-btn${loading ? ' auth-btn--loading' : ''}`} disabled={loading}>
            {loading ? 'CREATING...' : 'CREATE ACCOUNT'}
          </button>
        </form>

        <p className="auth-switch">
          Already have an account?{' '}
          <Link to="/login" className="auth-link">Sign in</Link>
        </p>
      </div>
    </div>
  )
}

export default RegisterPage
