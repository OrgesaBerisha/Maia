import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from './AuthContext.jsx'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import api from './api/axios.js'
import './PurchasesPage.css'

function PurchasesPage() {
  const { isLoggedIn } = useAuth()
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(true)
  const [expanded, setExpanded] = useState(null)

  useEffect(() => {
    if (!isLoggedIn) return
    api.get('/Order')
      .then(({ data }) => setOrders(data))
      .catch(() => {})
      .finally(() => setLoading(false))
  }, [isLoggedIn])

  const fmtDate = iso =>
    new Date(iso).toLocaleDateString('en-GB', { day: '2-digit', month: 'long', year: 'numeric' })

  const fmtPrice = n => `${Number(n).toFixed(2)} EUR`

  return (
    <div className="purchases-page">
      <svg
        className="purchases-blob"
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

      <header className="purchases-header">
        <Link to="/profile" className="purchases-back">← BACK</Link>
        <SiteLogo />
        <span className="purchases-header-label">PURCHASES</span>
      </header>

      <main className="purchases-main">
        <h1 className="purchases-heading">PURCHASES</h1>

        {loading && <p className="purchases-empty">Loading…</p>}

        {!loading && orders.length === 0 && (
          <p className="purchases-empty">NO PURCHASES YET</p>
        )}

        {!loading && orders.length > 0 && (
          <div className="purchases-list">
            {orders.map(order => (
              <div key={order.id} className="order-card">
                <button
                  className="order-card-header"
                  onClick={() => setExpanded(expanded === order.id ? null : order.id)}
                >
                  <div className="order-card-left">
                    <span className="order-id">ORDER #{order.id}</span>
                    <span className="order-date">{fmtDate(order.createdAt)}</span>
                  </div>
                  <div className="order-card-right">
                    <span className={`order-status order-status--${(order.status ?? '').toLowerCase()}`}>
                      {order.status}
                    </span>
                    <span className="order-total">{fmtPrice(order.totalPrice)}</span>
                    <span className="order-chevron">{expanded === order.id ? '−' : '+'}</span>
                  </div>
                </button>

                {expanded === order.id && (
                  <div className="order-items">
                    {(order.items ?? []).map((item, i) => (
                      <div key={i} className="order-item">
                        {item.imageUrl && (
                          <img src={item.imageUrl} alt={item.productName} className="order-item-img" />
                        )}
                        <div className="order-item-details">
                          <span className="order-item-name">{item.productName}</span>
                          <span className="order-item-meta">
                            {item.quantity} × {fmtPrice(item.price)}
                          </span>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            ))}
          </div>
        )}
      </main>

      <BottomNav />
    </div>
  )
}

export default PurchasesPage
