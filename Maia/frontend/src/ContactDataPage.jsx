import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from './AuthContext.jsx'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './ContactDataPage.css'

const STORAGE_KEY = 'maia_contact_extra'

function loadExtra() {
  try { return JSON.parse(localStorage.getItem(STORAGE_KEY) ?? '{}') }
  catch { return {} }
}

function ContactDataPage() {
  const { user } = useAuth()
  const extra = loadExtra()

  const [form, setForm] = useState({
    firstName:  user?.firstName  ?? '',
    lastName:   user?.lastName   ?? '',
    email:      user?.email      ?? '',
    phone:      extra.phone      ?? '',
    address:    extra.address    ?? '',
    city:       extra.city       ?? '',
    postalCode: extra.postalCode ?? '',
    country:    extra.country    ?? '',
  })
  const [saved, setSaved] = useState(false)

  const handleChange = e => {
    setForm(f => ({ ...f, [e.target.name]: e.target.value }))
    setSaved(false)
  }

  const handleSubmit = e => {
    e.preventDefault()
    localStorage.setItem(STORAGE_KEY, JSON.stringify({
      phone:      form.phone,
      address:    form.address,
      city:       form.city,
      postalCode: form.postalCode,
      country:    form.country,
    }))
    setSaved(true)
    setTimeout(() => setSaved(false), 3000)
  }

  return (
    <div className="contact-page">
      <svg
        className="contact-blob"
        viewBox="0 0 1440 220"
        preserveAspectRatio="none"
        xmlns="http://www.w3.org/2000/svg"
        aria-hidden="true"
      >
        <path
          d="M0,0 L1440,0 L1440,140
             C1340,162 1200,178 1060,164
             C920,150 800,118 660,132
             C520,146 380,178 240,188
             C160,194 80,190 0,196
             Z"
          fill="#d4c5b3"
        />
      </svg>

      <header className="contact-header">
        <Link to="/profile" className="contact-back">← BACK</Link>
        <SiteLogo />
        <span className="contact-header-label">CONTACT DATA</span>
      </header>

      <main className="contact-main">
        <h1 className="contact-heading">CONTACT DATA</h1>

        <form className="contact-form" onSubmit={handleSubmit}>
          <p className="contact-section-label">PERSONAL INFO</p>

          <div className="contact-row">
            <div className="contact-field">
              <label className="contact-label">FIRST NAME</label>
              <input
                className="contact-input"
                name="firstName"
                value={form.firstName}
                onChange={handleChange}
              />
            </div>
            <div className="contact-field">
              <label className="contact-label">LAST NAME</label>
              <input
                className="contact-input"
                name="lastName"
                value={form.lastName}
                onChange={handleChange}
              />
            </div>
          </div>

          <div className="contact-field">
            <label className="contact-label">EMAIL</label>
            <input
              className="contact-input"
              name="email"
              type="email"
              value={form.email}
              onChange={handleChange}
            />
          </div>

          <div className="contact-field">
            <label className="contact-label">PHONE</label>
            <input
              className="contact-input"
              name="phone"
              type="tel"
              value={form.phone}
              onChange={handleChange}
              placeholder="+1 000 000 0000"
            />
          </div>

          <div className="contact-divider" />
          <p className="contact-section-label">SHIPPING ADDRESS</p>

          <div className="contact-field">
            <label className="contact-label">ADDRESS</label>
            <input
              className="contact-input"
              name="address"
              value={form.address}
              onChange={handleChange}
              placeholder="Street and number"
            />
          </div>

          <div className="contact-row">
            <div className="contact-field">
              <label className="contact-label">CITY</label>
              <input
                className="contact-input"
                name="city"
                value={form.city}
                onChange={handleChange}
              />
            </div>
            <div className="contact-field">
              <label className="contact-label">POSTAL CODE</label>
              <input
                className="contact-input"
                name="postalCode"
                value={form.postalCode}
                onChange={handleChange}
              />
            </div>
          </div>

          <div className="contact-field">
            <label className="contact-label">COUNTRY</label>
            <input
              className="contact-input"
              name="country"
              value={form.country}
              onChange={handleChange}
            />
          </div>

          <button type="submit" className={`contact-save-btn${saved ? ' contact-save-btn--saved' : ''}`}>
            {saved ? 'SAVED' : 'SAVE CHANGES'}
          </button>
        </form>
      </main>

      <BottomNav />
    </div>
  )
}

export default ContactDataPage
