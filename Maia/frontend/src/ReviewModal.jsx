import { useState, useEffect } from 'react'
import api from './api/axios.js'
import { useAuth } from './AuthContext.jsx'
import './ReviewModal.css'

function Stars({ value, size = 18, interactive = false, onChange }) {
  const [hover, setHover] = useState(0)
  return (
    <div className="stars" style={{ fontSize: size }}>
      {[1, 2, 3, 4, 5].map(n => (
        <span
          key={n}
          className={`star${(interactive ? (hover || value) : value) >= n ? ' star--filled' : ''}`}
          onClick={() => interactive && onChange?.(n)}
          onMouseEnter={() => interactive && setHover(n)}
          onMouseLeave={() => interactive && setHover(0)}
          style={{ cursor: interactive ? 'pointer' : 'default' }}
        >
          ★
        </span>
      ))}
    </div>
  )
}

function ReviewModal({ product, onClose }) {
  const { isLoggedIn } = useAuth()
  const [data, setData]       = useState({ average: 0, count: 0, reviews: [] })
  const [loading, setLoading] = useState(true)
  const [rating, setRating]   = useState(0)
  const [comment, setComment] = useState('')
  const [submitting, setSub]  = useState(false)
  const [submitted, setSubmitted] = useState(false)

  const source = (product.source ?? 'WOMAN').toUpperCase()

  const load = async () => {
    setLoading(true)
    try {
      const { data: d } = await api.get('/reviews', {
        params: { productId: product.id, source }
      })
      setData(d)
    } catch { /* ignore */ }
    setLoading(false)
  }

  useEffect(() => {
    load()
    const onKey = e => { if (e.key === 'Escape') onClose() }
    window.addEventListener('keydown', onKey)
    return () => window.removeEventListener('keydown', onKey)
  }, [])

  const handleSubmit = async e => {
    e.preventDefault()
    if (rating === 0) return
    setSub(true)
    try {
      await api.post('/reviews', { productId: product.id, source, rating, comment })
      setSubmitted(true)
      setRating(0)
      setComment('')
      await load()
    } catch { /* ignore */ }
    setSub(false)
  }

  const formatDate = iso =>
    new Date(iso).toLocaleDateString('sq-AL', { day: '2-digit', month: 'short', year: 'numeric' })

  return (
    <div className="review-backdrop" onClick={onClose}>
      <div className="review-modal" onClick={e => e.stopPropagation()}>
        <button className="review-close" onClick={onClose}>✕</button>

        {/* Product header */}
        <div className="review-product">
          <img src={product.image} alt={product.name} className="review-product-img" />
          <div>
            <div className="review-product-name">{product.name}</div>
            <div className="review-product-price">{product.priceLabel}</div>
            {!loading && (
              <div className="review-summary">
                <Stars value={Math.round(data.average)} size={16} />
                <span className="review-avg">{data.average > 0 ? data.average.toFixed(1) : '—'}</span>
                <span className="review-count">({data.count} {data.count === 1 ? 'review' : 'reviews'})</span>
              </div>
            )}
          </div>
        </div>

        <div className="review-divider" />

        {/* Add review form */}
        {isLoggedIn ? (
          <form className="review-form" onSubmit={handleSubmit}>
            <p className="review-form-title">
              {submitted ? '✓ Review u ruajt!' : 'Lër një vlerësim'}
            </p>
            <Stars value={rating} size={28} interactive onChange={setRating} />
            {rating === 0 && <p className="review-hint">Klikoni yjet për të vlerësuar</p>}
            <textarea
              className="review-textarea"
              placeholder="Shkruaj komentin tënd (opsionale)..."
              value={comment}
              onChange={e => setComment(e.target.value)}
              maxLength={1000}
              rows={3}
            />
            <button
              type="submit"
              className={`review-submit${rating === 0 ? ' review-submit--disabled' : ''}`}
              disabled={rating === 0 || submitting}
            >
              {submitting ? 'Duke ruajtur...' : 'POSTO REVIEW'}
            </button>
          </form>
        ) : (
          <p className="review-login-note">Kyçu për të lënë një vlerësim.</p>
        )}

        <div className="review-divider" />

        {/* Reviews list */}
        <div className="review-list">
          {loading ? (
            <p className="review-empty">Duke ngarkuar...</p>
          ) : data.reviews.length === 0 ? (
            <p className="review-empty">Nuk ka vlerësime ende. Bëhu i pari!</p>
          ) : (
            data.reviews.map(r => (
              <div key={r.id} className="review-item">
                <div className="review-item-header">
                  <span className="review-item-user">{r.userName}</span>
                  <Stars value={r.rating} size={13} />
                  <span className="review-item-date">{formatDate(r.createdAt)}</span>
                </div>
                {r.comment && <p className="review-item-comment">{r.comment}</p>}
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  )
}

export { Stars }
export default ReviewModal
