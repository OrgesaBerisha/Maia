import { useState, useEffect, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthContext.jsx'
import api from '../api/axios.js'
import './DashboardLayout.css'

const TABS = [
  { key: 'overview',     label: 'Overview',      icon: '◈' },
  { key: 'products',     label: 'Products',      icon: '◉' },
  { key: 'categories',   label: 'Categories',    icon: '◎' },
  { key: 'types',        label: 'Product Types', icon: '◇' },
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

// ── Products Tab ───────────────────────────────────────────────────────────
function ProductsTab() {
  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [saving, setSaving] = useState(false)
  const [formErr, setFormErr] = useState('')
  const [form, setForm] = useState({
    title: '', description: '', price: '', imageUrl: '',
    kidsCategoryId: '', kidsProductTypeId: '', discountPercent: ''
  })

  const { data: products, loading, error, reload } = useApi(async () => {
    const r = await api.get('/api/KidsCards')
    return r.data
  }, [])

  const { data: categories } = useApi(async () => {
    try { const r = await api.get('/api/KidsCategory'); return r.data }
    catch { return [] }
  }, [])

  const { data: types } = useApi(async () => {
    try { const r = await api.get('/api/KidsProductType'); return r.data }
    catch { return [] }
  }, [])

  const filtered = products.filter(p =>
    !q || p.title?.toLowerCase().includes(q.toLowerCase())
  )

  const openAdd = () => {
    setForm({
      title: '', description: '', price: '', imageUrl: '',
      kidsCategoryId: categories[0]?.id ?? '',
      kidsProductTypeId: types[0]?.id ?? '',
      discountPercent: ''
    })
    setFormErr(''); setModal('add')
  }

  const openEdit = (p) => {
    setSelected(p)
    setForm({
      title: p.title, description: p.description ?? '', price: p.price,
      imageUrl: p.imageUrl ?? '', kidsCategoryId: p.kidsCategoryId ?? '',
      kidsProductTypeId: p.kidsProductTypeId ?? '', discountPercent: p.discountPercent ?? ''
    })
    setFormErr(''); setModal('edit')
  }

  const save = async () => {
    setSaving(true); setFormErr('')
    const body = {
      ...form,
      price: parseFloat(form.price),
      kidsCategoryId: parseInt(form.kidsCategoryId),
      kidsProductTypeId: parseInt(form.kidsProductTypeId),
      discountPercent: form.discountPercent !== '' ? parseInt(form.discountPercent) : null,
    }
    try {
      if (modal === 'add') await api.post('/api/KidsCards', body)
      else await api.put(`/api/KidsCards/${selected.id}`, body)
      setModal(null); reload()
    } catch (e) { setFormErr(e?.response?.data?.message ?? 'Save failed.') }
    finally { setSaving(false) }
  }

  const applyDiscount = async (p, pct) => {
    try { await api.patch(`/api/KidsCards/${p.id}/sale`, { discountPercent: pct }); reload() }
    catch (e) { alert(e?.response?.data?.message ?? 'Failed.') }
  }

  const del = async (p) => {
    if (!confirm(`Delete "${p.title}"?`)) return
    try { await api.delete(`/api/KidsCards/${p.id}`); reload() }
    catch (e) { alert(e?.response?.data?.message ?? 'Delete failed.') }
  }

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <input className="db-search" placeholder="Search products…" value={q} onChange={e => setQ(e.target.value)} />
        <button className="db-btn db-btn--primary" onClick={openAdd}>+ Add Product</button>
      </div>
      {error && <div className="db-error">{error}</div>}
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr>
            <th></th><th>TITLE</th><th>PRICE</th><th>CATEGORY</th><th>TYPE</th><th>SALE</th><th>ACTIONS</th>
          </tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={7} className="db-empty">Loading…</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan={7} className="db-empty">No products found.</td></tr>
            ) : filtered.map(p => (
              <tr key={p.id}>
                <td><img className="db-img-preview" src={p.imageUrl || 'https://placehold.co/56x56?text=No+Img'} alt={p.title} /></td>
                <td>{p.title}</td>
                <td>€{Number(p.price).toFixed(2)}</td>
                <td>{p.kidsCategory?.name ?? p.kidsCategoryId ?? '—'}</td>
                <td>{p.kidsProductType?.name ?? p.kidsProductTypeId ?? '—'}</td>
                <td>
                  {p.discountPercent > 0
                    ? <span className="db-badge db-badge--green">{p.discountPercent}% OFF</span>
                    : '—'}
                </td>
                <td style={{ display: 'flex', gap: 6, flexWrap: 'wrap' }}>
                  <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => openEdit(p)}>Edit</button>
                  {p.discountPercent > 0
                    ? <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => applyDiscount(p, 0)}>Remove Sale</button>
                    : null}
                  <button className="db-btn db-btn--danger db-btn--sm" onClick={() => del(p)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {modal && (
        <Modal title={modal === 'add' ? 'Add Product' : 'Edit Product'}
          onClose={() => setModal(null)}
          actions={<>
            <button className="db-btn db-btn--ghost" onClick={() => setModal(null)}>Cancel</button>
            <button className="db-btn db-btn--primary" onClick={save} disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
          </>}>
          {formErr && <div className="db-error">{formErr}</div>}
          <div className="db-form">
            <div className="db-field">
              <label className="db-label">Title</label>
              <input className="db-input" value={form.title} onChange={e => setForm(f => ({ ...f, title: e.target.value }))} />
            </div>
            <div className="db-form-row">
              <div className="db-field">
                <label className="db-label">Price (€)</label>
                <input className="db-input" type="number" step="0.01" value={form.price} onChange={e => setForm(f => ({ ...f, price: e.target.value }))} />
              </div>
              <div className="db-field">
                <label className="db-label">Discount %</label>
                <input className="db-input" type="number" min="0" max="100" value={form.discountPercent} onChange={e => setForm(f => ({ ...f, discountPercent: e.target.value }))} placeholder="0" />
              </div>
            </div>
            <div className="db-form-row">
              <div className="db-field">
                <label className="db-label">Category</label>
                <select className="db-select" value={form.kidsCategoryId} onChange={e => setForm(f => ({ ...f, kidsCategoryId: e.target.value }))}>
                  {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                </select>
              </div>
              <div className="db-field">
                <label className="db-label">Product Type</label>
                <select className="db-select" value={form.kidsProductTypeId} onChange={e => setForm(f => ({ ...f, kidsProductTypeId: e.target.value }))}>
                  {types.map(t => <option key={t.id} value={t.id}>{t.name}</option>)}
                </select>
              </div>
            </div>
            <div className="db-field">
              <label className="db-label">Image URL</label>
              <input className="db-input" value={form.imageUrl} onChange={e => setForm(f => ({ ...f, imageUrl: e.target.value }))} placeholder="https://…" />
            </div>
            {form.imageUrl && (
              <img src={form.imageUrl} alt="preview" style={{ width: 80, height: 80, objectFit: 'cover', borderRadius: 6 }} />
            )}
            <div className="db-field">
              <label className="db-label">Description</label>
              <textarea className="db-textarea" value={form.description} onChange={e => setForm(f => ({ ...f, description: e.target.value }))} />
            </div>
          </div>
        </Modal>
      )}
    </div>
  )
}

// ── Simple CRUD Tab (for categories and types) ─────────────────────────────
function SimpleListTab({ label, endpoint }) {
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [form, setForm] = useState({ name: '' })
  const [saving, setSaving] = useState(false)
  const [formErr, setFormErr] = useState('')

  const { data: items, loading, error, reload } = useApi(async () => {
    const r = await api.get(`/api${endpoint}`)
    return r.data
  }, [endpoint])

  const openAdd = () => { setForm({ name: '' }); setFormErr(''); setModal('add') }
  const openEdit = (c) => { setSelected(c); setForm({ name: c.name }); setFormErr(''); setModal('edit') }

  const save = async () => {
    setSaving(true); setFormErr('')
    try {
      if (modal === 'add') await api.post(`/api${endpoint}`, form)
      else await api.put(`/api${endpoint}/${selected.id}`, form)
      setModal(null); reload()
    } catch (e) { setFormErr(e?.response?.data?.message ?? 'Save failed.') }
    finally { setSaving(false) }
  }

  const del = async (c) => {
    if (!confirm(`Delete "${c.name}"?`)) return
    try { await api.delete(`/api${endpoint}/${c.id}`); reload() }
    catch (e) { alert(e?.response?.data?.message ?? 'Delete failed.') }
  }

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <span className="db-stat-label">{items.length} {label.toLowerCase()}</span>
        <button className="db-btn db-btn--primary" onClick={openAdd}>+ Add {label.slice(0, -1)}</button>
      </div>
      {error && <div className="db-error">{error}</div>}
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr><th>NAME</th><th>ACTIONS</th></tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={2} className="db-empty">Loading…</td></tr>
            ) : items.length === 0 ? (
              <tr><td colSpan={2} className="db-empty">No {label.toLowerCase()} found.</td></tr>
            ) : items.map(c => (
              <tr key={c.id}>
                <td>{c.name}</td>
                <td style={{ display: 'flex', gap: 6 }}>
                  <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => openEdit(c)}>Edit</button>
                  <button className="db-btn db-btn--danger db-btn--sm" onClick={() => del(c)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {modal && (
        <Modal title={modal === 'add' ? `Add ${label.slice(0, -1)}` : `Edit ${label.slice(0, -1)}`}
          onClose={() => setModal(null)}
          actions={<>
            <button className="db-btn db-btn--ghost" onClick={() => setModal(null)}>Cancel</button>
            <button className="db-btn db-btn--primary" onClick={save} disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
          </>}>
          {formErr && <div className="db-error">{formErr}</div>}
          <div className="db-form">
            <div className="db-field">
              <label className="db-label">Name</label>
              <input className="db-input" value={form.name} onChange={e => setForm({ name: e.target.value })} autoFocus />
            </div>
          </div>
        </Modal>
      )}
    </div>
  )
}

// ── Main ───────────────────────────────────────────────────────────────────
export default function KidsManagerDashboard() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const [activeTab, setActiveTab] = useState('overview')

  const { data: products }   = useApi(async () => { const r = await api.get('/api/KidsCards');       return r.data }, [])
  const { data: categories } = useApi(async () => { try { const r = await api.get('/api/KidsCategory');    return r.data } catch { return [] } }, [])
  const { data: types }      = useApi(async () => { try { const r = await api.get('/api/KidsProductType'); return r.data } catch { return [] } }, [])

  const handleLogout = async () => { await logout(); navigate('/login') }

  const renderContent = () => {
    switch (activeTab) {
      case 'overview': return (
        <div className="db-stats">
          <div className="db-stat-card"><span className="db-stat-label">Total Products</span><span className="db-stat-value">{products.length}</span></div>
          <div className="db-stat-card"><span className="db-stat-label">Categories</span><span className="db-stat-value">{categories.length}</span></div>
          <div className="db-stat-card"><span className="db-stat-label">Product Types</span><span className="db-stat-value">{types.length}</span></div>
          <div className="db-stat-card"><span className="db-stat-label">On Sale</span><span className="db-stat-value">{products.filter(p => p.discountPercent > 0).length}</span></div>
        </div>
      )
      case 'products':   return <ProductsTab />
      case 'categories': return <SimpleListTab label="Categories" endpoint="/KidsCategory" />
      case 'types':      return <SimpleListTab label="Product Types" endpoint="/KidsProductType" />
      default: return null
    }
  }

  return (
    <div className="db-root">
      <aside className="db-sidebar">
        <div className="db-brand">MAIA</div>
        <div className="db-role-badge">Kids Manager</div>
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
        </div>
      </aside>
      <main className="db-main">
        <header className="db-header">
          <h1 className="db-header-title">
            {TABS.find(t => t.key === activeTab)?.label ?? 'Kids Manager'}
          </h1>
        </header>
        <div className="db-content">{renderContent()}</div>
      </main>
    </div>
  )
}
