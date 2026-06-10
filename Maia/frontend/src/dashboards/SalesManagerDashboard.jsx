import { useState, useEffect, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthContext.jsx'
import api from '../api/axios.js'
import LiveChatTab from './LiveChatTab.jsx'
import './DashboardLayout.css'

const TABS = [
  { key: 'overview',   label: 'Overview',      icon: '◈' },
  { key: 'customers',  label: 'Customers',     icon: '◉' },
  { key: 'women',      label: 'Women Sales',   icon: '◇' },
  { key: 'men',        label: 'Men Sales',     icon: '◆' },
  { key: 'kids',       label: 'Kids Sales',    icon: '◎' },
  { key: 'active',     label: 'Active Sales',  icon: '◑' },
  { key: 'chat',       label: 'Live Chat',     icon: '💬' },
]

function useApi(fetcher, deps = []) {
  const [data, setData] = useState([])
  const [loading, setL] = useState(true)
  const [error, setErr] = useState('')

  const load = useCallback(async () => {
    setL(true); setErr('')
    try { setData(await fetcher()) }
    catch (e) { setErr(e?.response?.data?.message ?? 'Failed to load.') }
    finally { setL(false) }
  }, deps)

  useEffect(() => { load() }, [load])
  return { data, loading, error, reload: load }
}

function Modal({ title, onClose, children, actions }) {
  return (
    <div className="db-modal-overlay" onClick={e => e.target === e.currentTarget && onClose()}>
      <div className="db-modal">
        <h2 className="db-modal-title">{title}</h2>
        {children}
        <div className="db-modal-actions">{actions}</div>
      </div>
    </div>
  )
}

// ── Customers Tab ─────────────────────────────────────────────────────────
function CustomersTab() {
  const [q, setQ] = useState('')

  const { data: customers, loading, error } = useApi(async () => {
    const r = await api.get('/users/customers')
    return r.data
  }, [])

  const filtered = customers.filter(u =>
    !q || `${u.firstName} ${u.lastName} ${u.email}`.toLowerCase().includes(q.toLowerCase())
  )

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <input className="db-search" placeholder="Search customers…" value={q} onChange={e => setQ(e.target.value)} />
        <span className="db-stat-label" style={{ flexShrink: 0 }}>{filtered.length} customers</span>
      </div>
      {error && <div className="db-error">{error}</div>}
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr><th>NAME</th><th>EMAIL</th><th>JOINED</th></tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={3} className="db-empty">Loading…</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan={3} className="db-empty">No customers found.</td></tr>
            ) : filtered.map(u => (
              <tr key={u.userID ?? u.email}>
                <td>{u.firstName} {u.lastName}</td>
                <td>{u.email}</td>
                <td>{u.createdAt ? new Date(u.createdAt).toLocaleDateString() : '—'}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

// ── Section Sales Tab ──────────────────────────────────────────────────────
function SectionSalesTab({ section }) {
  const endpointMap = { women: '/CardsWomen', men: '/MenCards', kids: '/KidsCards' }
  const endpoint = endpointMap[section]

  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [discount, setDiscount] = useState('')
  const [saving, setSaving] = useState(false)
  const [saveErr, setSaveErr] = useState('')

  const { data: products, loading, error, reload } = useApi(async () => {
    const r = await api.get(endpoint)
    return r.data
  }, [section])

  const filtered = products
    .filter(p => !q || p.title?.toLowerCase().includes(q.toLowerCase()))
    .sort((a, b) => ((b.discountPercent || 0) > 0 ? 1 : 0) - ((a.discountPercent || 0) > 0 ? 1 : 0))

  const openSale = (p) => {
    setSelected(p)
    setDiscount(p.discountPercent ?? '')
    setSaveErr('')
    setModal('sale')
  }

  const applySale = async () => {
    setSaving(true); setSaveErr('')
    const pct = parseInt(discount) || 0
    try {
      if (section === 'women') {
        await api.patch(`/CardsWomen/${selected.id}/sale`, { discountPercent: pct })
      } else if (section === 'kids') {
        await api.patch(`/KidsCards/${selected.id}/sale`, { discountPercent: pct })
      } else {
        await api.put(`/MenCards/${selected.id}`, {
          title: selected.title,
          imageUrl: selected.imageUrl || null,
          price: selected.price,
          menCategoryId: selected.menCategoryId,
          description: selected.description || '',
          color: selected.color || null,
          discountPercent: pct,
        })
      }
      setModal(null); reload()
    } catch (e) {
      setSaveErr(e?.response?.data?.message ?? 'Failed to apply sale.')
    } finally { setSaving(false) }
  }

  const removeSale = async (p) => {
    try {
      if (section === 'women') {
        await api.patch(`/CardsWomen/${p.id}/sale`, { discountPercent: 0 })
      } else if (section === 'kids') {
        await api.patch(`/KidsCards/${p.id}/sale`, { discountPercent: 0 })
      } else {
        await api.put(`/MenCards/${p.id}`, {
          title: p.title,
          imageUrl: p.imageUrl || null,
          price: p.price,
          menCategoryId: p.menCategoryId,
          description: p.description || '',
          color: p.color || null,
          discountPercent: 0,
        })
      }
      reload()
    } catch (e) { alert(e?.response?.data?.message ?? 'Failed.') }
  }

  const discountedPrice = selected
    ? (parseFloat(selected.price) * (1 - (parseInt(discount) || 0) / 100)).toFixed(2)
    : ''

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <input className="db-search" placeholder="Search products…" value={q} onChange={e => setQ(e.target.value)} />
        <span className="db-stat-label" style={{ flexShrink: 0 }}>{filtered.length} products</span>
      </div>
      {error && <div className="db-error">{error}</div>}
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr>
            <th></th><th>PRODUCT</th><th>ORIGINAL PRICE</th><th>SALE</th><th>ACTIONS</th>
          </tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={5} className="db-empty">Loading…</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan={5} className="db-empty">No products found.</td></tr>
            ) : filtered.map(p => (
              <tr key={p.id}>
                <td><img className="db-img-preview" src={p.imageUrl || 'https://placehold.co/56x56?text=No+Img'} alt={p.title} /></td>
                <td>{p.title}</td>
                <td>€{Number(p.price).toFixed(2)}</td>
                <td>
                  {p.discountPercent
                    ? <span className="db-badge db-badge--green">{p.discountPercent}% OFF</span>
                    : <span className="db-badge db-badge--grey">No sale</span>}
                </td>
                <td>
                  <div style={{ display: 'flex', gap: 6 }}>
                    <button className="db-btn db-btn--primary db-btn--sm" onClick={() => openSale(p)}>
                      {p.discountPercent ? 'Edit Sale' : 'Set Sale'}
                    </button>
                    {p.discountPercent > 0 && (
                      <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => removeSale(p)}>Remove</button>
                    )}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {modal === 'sale' && (
        <Modal title="Apply Sale Price" onClose={() => setModal(null)}
          actions={<>
            <button className="db-btn db-btn--ghost" onClick={() => setModal(null)}>Cancel</button>
            <button className="db-btn db-btn--primary" onClick={applySale} disabled={saving}>{saving ? 'Applying…' : 'Apply Sale'}</button>
          </>}>
          {saveErr && <div className="db-error">{saveErr}</div>}
          <div className="db-form">
            <div className="db-field">
              <label className="db-label">Product</label>
              <input className="db-input" value={selected?.title} disabled />
            </div>
            <div className="db-field">
              <label className="db-label">Discount Percentage</label>
              <input className="db-input" type="number" min="0" max="90" value={discount}
                onChange={e => setDiscount(e.target.value)} placeholder="e.g. 20" />
            </div>
            {discount > 0 && (
              <div className="db-field">
                <label className="db-label">Sale Price Preview</label>
                <input className="db-input" value={`€${discountedPrice}`} disabled />
              </div>
            )}
          </div>
        </Modal>
      )}
    </div>
  )
}

// ── Active Sales Tab ───────────────────────────────────────────────────────
function ActiveSalesTab() {
  const { data: women, loading: wl }  = useApi(async () => { const r = await api.get('/CardsWomen'); return r.data }, [])
  const { data: men,   loading: ml }  = useApi(async () => { const r = await api.get('/MenCards');   return r.data }, [])
  const { data: kids,  loading: kl }  = useApi(async () => { const r = await api.get('/KidsCards');  return r.data }, [])

  const loading = wl || ml || kl

  const allOnSale = [
    ...women.filter(p => p.discountPercent > 0).map(p => ({ ...p, _section: 'Women' })),
    ...men.filter(p => p.discountPercent > 0).map(p => ({ ...p, _section: 'Men' })),
    ...kids.filter(p => p.discountPercent > 0).map(p => ({ ...p, _section: 'Kids' })),
  ]

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <span className="db-stat-label">{allOnSale.length} items currently on sale</span>
      </div>
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr>
            <th></th><th>PRODUCT</th><th>SECTION</th><th>PRICE</th><th>DISCOUNT</th>
          </tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={5} className="db-empty">Loading…</td></tr>
            ) : allOnSale.length === 0 ? (
              <tr><td colSpan={5} className="db-empty">No active sales at the moment.</td></tr>
            ) : allOnSale.map(p => (
              <tr key={`${p._section}-${p.id}`}>
                <td><img className="db-img-preview" src={p.imageUrl || 'https://placehold.co/56x56?text=No+Img'} alt={p.title} /></td>
                <td>{p.title}</td>
                <td><span className="db-badge db-badge--grey">{p._section}</span></td>
                <td>€{Number(p.price).toFixed(2)}</td>
                <td><span className="db-badge db-badge--green">{p.discountPercent}% OFF</span></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

// ── Main ───────────────────────────────────────────────────────────────────
export default function SalesManagerDashboard() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const [activeTab, setActiveTab] = useState('overview')

  const { data: women } = useApi(async () => { const r = await api.get('/CardsWomen'); return r.data }, [])
  const { data: men }   = useApi(async () => { const r = await api.get('/MenCards');   return r.data }, [])
  const { data: kids }  = useApi(async () => { const r = await api.get('/KidsCards');  return r.data }, [])

  const totalOnSale = [
    ...women.filter(p => p.discountPercent > 0),
    ...men.filter(p => p.discountPercent > 0),
    ...kids.filter(p => p.discountPercent > 0),
  ].length

  const handleLogout = async () => { await logout(); navigate('/login') }

  const renderContent = () => {
    switch (activeTab) {
      case 'overview': return (
        <div>
          <div className="db-stats">
            <div className="db-stat-card"><span className="db-stat-label">Women Products</span><span className="db-stat-value">{women.length}</span></div>
            <div className="db-stat-card"><span className="db-stat-label">Men Products</span><span className="db-stat-value">{men.length}</span></div>
            <div className="db-stat-card"><span className="db-stat-label">Kids Products</span><span className="db-stat-value">{kids.length}</span></div>
            <div className="db-stat-card"><span className="db-stat-label">Items on Sale</span><span className="db-stat-value">{totalOnSale}</span></div>
          </div>
        </div>
      )
      case 'customers': return <CustomersTab />
      case 'women':     return <SectionSalesTab section="women" />
      case 'men':       return <SectionSalesTab section="men" />
      case 'kids':      return <SectionSalesTab section="kids" />
      case 'active':    return <ActiveSalesTab />
      case 'chat':      return <LiveChatTab />
      default: return null
    }
  }

  return (
    <div className="db-root">
      <aside className="db-sidebar">
        <div className="db-brand">MAIA</div>
        <div className="db-role-badge">Sales Manager</div>
        <nav className="db-nav">
          {TABS.map(tab => (
            <button key={tab.key}
              className={`db-nav-item${activeTab === tab.key ? ' db-nav-item--active' : ''}`}
              onClick={() => setActiveTab(tab.key)}>
              <span className="db-nav-icon">{tab.icon}</span>
              {tab.label}
            </button>
          ))}
        </nav>
        <div className="db-sidebar-footer">
          <div className="db-user-info">
            <span className="db-user-name">{user?.firstName} {user?.lastName}</span>
            <span className="db-user-email">{user?.email}</span>
          </div>
          <button className="db-logout-btn" onClick={handleLogout}>LOG OUT</button>
          <button className="db-home-btn" onClick={() => navigate('/')}>HOME PAGE</button>
        </div>
      </aside>
      <main className="db-main">
        <header className="db-header">
          <h1 className="db-header-title">
            {TABS.find(t => t.key === activeTab)?.label ?? 'Sales Manager'}
          </h1>
        </header>
        <div className="db-content">{renderContent()}</div>
      </main>
    </div>
  )
}
