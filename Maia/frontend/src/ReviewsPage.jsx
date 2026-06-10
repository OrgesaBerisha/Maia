import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import api from './api/axios.js'
import { useAuth } from './AuthContext.jsx'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './ReviewsPage.css'

function Stars({ value, size = 20, interactive = false, onChange }) {
  const [hover, setHover] = useState(0)
  return (
    <div className="rp-stars" style={{ fontSize: size }}>
      {[1, 2, 3, 4, 5].map(n => (
        <span
          key={n}
          className={`rp-star${(interactive ? (hover || value) : value) >= n ? ' rp-star--on' : ''}`}
          onClick={() => interactive && onChange?.(n)}
          onMouseEnter={() => interactive && setHover(n)}
          onMouseLeave={() => interactive && setHover(0)}
          style={{ cursor: interactive ? 'pointer' : 'default' }}
        >★</span>
      ))}
    </div>
  )
}

function ReviewsPage() {
  const navigate = useNavigate()
  const { isLoggedIn, user } = useAuth()
  const [data, setData]         = useState({ average: 0, count: 0, reviews: [] })
  const [loading, setLoading]   = useState(true)
  const [rating, setRating]     = useState(0)
  const [comment, setComment]   = useState('')
  const [submitting, setSub]    = useState(false)
  const [done, setDone]         = useState(false)
  const [error, setError]       = useState('')

  const load = async () => {
    setLoading(true)
    try {
      const { data: d } = await api.get('/reviews/store')
      setData(d)
    } catch { /* ignore */ }
    setLoading(false)
  }

  useEffect(() => { load() }, [])

  const handleSubmit = async e => {
    e.preventDefault()
    if (rating === 0) { setError('Zgjedh numrin e yjeve.'); return }
    setSub(true); setError('')
    try {
      await api.post('/reviews/store', { rating, comment })
      setDone(true)
      setRating(0)
      setComment('')
      await load()
    } catch (err) {
      const status = err?.response?.status
      const msg = err?.response?.data?.message ?? err?.response?.data ?? err?.message ?? 'Unknown error'
      setError(`Error ${status}: ${typeof msg === 'string' ? msg : JSON.stringify(msg)}`)
    }
    setSub(false)
  }

  return (
    <div className="rp-page">
      <svg className="rp-blob" viewBox="0 0 1440 220" preserveAspectRatio="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
        <path d="M0,0 L1440,0 L1440,140 C1340,162 1200,178 1060,164 C920,150 800,118 660,132 C520,146 380,178 240,188 C160,194 80,190 0,196 Z" fill="#d4c5b3" />
      </svg>

      <header className="rp-header">
        <button className="rp-back-btn" onClick={() => navigate(-1)}>← BACK</button>
        <SiteLogo />
      </header>

      <main className="rp-main">
        <h1 className="rp-title">REVIEWS</h1>

        {/* Summary */}
        {!loading && data.count > 0 && (
          <div className="rp-summary">
            <span className="rp-avg-num">{data.average}</span>
            <div style={{ display: 'flex', flexDirection: 'column', gap: 4 }}>
              <Stars value={Math.round(data.average)} size={18} />
              <span className="rp-count">{data.count} VLERËSIM{data.count !== 1 ? 'E' : ''}</span>
            </div>
          </div>
        )}

        {/* Form */}
        {isLoggedIn ? (
          <form className="rp-form" onSubmit={handleSubmit}>
            <p className="rp-form-title">
              {done ? '✓ Faleminderit për vlerësimin!' : `Lër vlerësimin tënd, ${user?.firstName ?? 'ti'}`}
            </p>
            <Stars value={rating} size={32} interactive onChange={v => { setRating(v); setError('') }} />
            {rating === 0 && <p className="rp-hint">Klikoni yjet</p>}
            <textarea
              className="rp-textarea"
              placeholder="Shkruaj mendimin tënd për MAIA..."
              value={comment}
              onChange={e => setComment(e.target.value)}
              maxLength={1000}
              rows={4}
            />
            {error && <p className="rp-error">{error}</p>}
            <button
              type="submit"
              className={`rp-btn${rating === 0 || submitting ? ' rp-btn--off' : ''}`}
              disabled={rating === 0 || submitting}
            >
              {submitting ? 'Duke ruajtur...' : 'POSTO'}
            </button>
          </form>
        ) : (
          <div className="rp-login-box">
            <p>Kyçu për të lënë një vlerësim.</p>
            <a href="/login" className="rp-btn">KYÇU</a>
          </div>
        )}

        {/* Reviews list */}
        {!loading && (
          <>
            <div className="rp-divider" />
            <div className="rp-list">
              {data.reviews.length === 0
                ? <p className="rp-empty">Bëhu i pari që lë një vlerësim.</p>
                : data.reviews.map(r => (
                    <div key={r.id} className="rp-item">
                      <div className="rp-item-top">
                        <Stars value={r.rating} size={14} />
                        <span className="rp-item-user">{r.userName}</span>
                        <span className="rp-item-date">
                          {new Date(r.createdAt).toLocaleDateString('sq-AL', { day: '2-digit', month: 'short', year: 'numeric' })}
                        </span>
                      </div>
                      {r.comment && <p className="rp-item-comment">{r.comment}</p>}
                    </div>
                  ))
              }
            </div>
          </>
        )}

      </main>

      <BottomNav />
    </div>
  )
}

export default ReviewsPage
