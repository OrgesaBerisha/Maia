import { useState, useEffect, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthContext.jsx'
import api from '../api/axios.js'
import './DashboardLayout.css'

const TABS = [
  { key: 'overview',    label: 'Overview',    icon: '◈' },
  { key: 'products',    label: 'Products',    icon: '◆' },
  { key: 'categories',  label: 'Categories',  icon: '◎' },
  { key: 'sales',       label: 'Sales',       icon: '◑' },
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

const FALLBACK_MEN_CATS = [
  { id: 1, name: 'Tops' },               { id: 2, name: 'Bottoms' },
  { id: 3, name: 'Suits & Formalwear' }, { id: 4, name: 'Outerwear' },
  { id: 5, name: 'Swimwear' },           { id: 6, name: 'Footwear' },
  { id: 7, name: 'Accessories' },        { id: 8, name: 'Sale' },
]

// ── Products Tab ───────────────────────────────────────────────────────────
function ProductsTab({ categories }) {
  const cats = categories.length > 0 ? categories : FALLBACK_MEN_CATS

  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [saving, setSaving] = useState(false)
  const [formErr, setFormErr] = useState('')
  const [form, setForm] = useState({
    title: '', description: '', price: '', imageUrl: '', categoryName: '', color: ''
  })

  const { data: products, loading, error, reload } = useApi(async () => {
    const r = await api.get('/MenCards')
    return r.data
  }, [])

  const filtered = products.filter(p =>
    !q || p.title?.toLowerCase().includes(q.toLowerCase())
  )

  const openAdd = () => {
    setForm({ title: '', description: '', price: '', imageUrl: '', categoryName: '', color: '' })
    setFormErr(''); setModal('add')
  }

  const openEdit = (p) => {
    setSelected(p)
    setForm({
      title: p.title ?? '', description: p.description ?? '',
      price: String(p.price ?? ''), imageUrl: p.imageUrl ?? '',
      categoryName: p.menCategoryName ?? '', color: p.color ?? ''
    })
    setFormErr(''); setModal('edit')
  }

  const save = async () => {
    setSaving(true); setFormErr('')
    const price = parseFloat(form.price)
    const cat = cats.find(c => c.name.toLowerCase() === form.categoryName.trim().toLowerCase())
    if (!form.title?.trim()) { setFormErr('Title is required.'); setSaving(false); return }
    if (isNaN(price) || price <= 0) { setFormErr('Enter a valid price.'); setSaving(false); return }
    if (!form.categoryName.trim()) { setFormErr('Category is required.'); setSaving(false); return }
    if (!cat) { setFormErr(`Unknown category. Available: ${cats.map(c => c.name).join(', ')}`); setSaving(false); return }
    const body = {
      title: form.title.trim(), description: form.description.trim(),
      price, menCategoryId: cat.id,
      imageUrl: form.imageUrl.trim() || null, color: form.color.trim() || null
    }
    try {
      if (modal === 'add') await api.post('/MenCards', body)
      else await api.put(`/MenCards/${selected.id}`, body)
      setModal(null); reload()
    } catch (e) {
      const d = e?.response?.data
      setFormErr(d?.message ?? (d?.errors ? Object.values(d.errors).flat().join('; ') : null) ?? d?.title ?? 'Save failed.')
    }
    finally { setSaving(false) }
  }

  const del = async (p) => {
    if (!window.confirm(`Delete "${p.title}"?`)) return
    try { await api.delete(`/MenCards/${p.id}`); reload() }
    catch (e) { window.alert(e?.response?.data?.message ?? e?.response?.data?.title ?? 'Delete failed.') }
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
          <thead><tr><th></th><th>TITLE</th><th>PRICE</th><th>CATEGORY</th><th>ACTIONS</th></tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={5} className="db-empty">Loading…</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan={5} className="db-empty">No products found.</td></tr>
            ) : filtered.map(p => (
              <tr key={p.id}>
                <td><img className="db-img-preview" src={p.imageUrl || 'https://placehold.co/56x56?text=No+Img'} alt={p.title} onError={e => { e.target.src = 'https://placehold.co/56x56?text=No+Img' }} /></td>
                <td>{p.title}</td>
                <td>
                  {p.discountPercent > 0 ? (
                    <span>
                      <span style={{ textDecoration: 'line-through', color: '#9a8e82', marginRight: 6 }}>€{Number(p.price).toFixed(2)}</span>
                      <span style={{ color: '#22a355' }}>€{(p.price * (1 - p.discountPercent / 100)).toFixed(2)}</span>
                    </span>
                  ) : `€${Number(p.price).toFixed(2)}`}
                  {p.discountPercent > 0 && <span className="db-badge db-badge--green" style={{ marginLeft: 6 }}>{p.discountPercent}% OFF</span>}
                </td>
                <td>{p.menCategoryName || '—'}</td>
                <td>
                  <div style={{ display: 'flex', gap: 6 }}>
                    <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => openEdit(p)}>Edit</button>
                    <button className="db-btn db-btn--danger db-btn--sm" onClick={() => del(p)}>Delete</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {modal && (
        <Modal title={modal === 'add' ? 'Add Product' : 'Edit Product'} onClose={() => setModal(null)}
          actions={<>
            <button className="db-btn db-btn--ghost" onClick={() => setModal(null)}>Cancel</button>
            <button className="db-btn db-btn--primary" onClick={save} disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
          </>}>
          {formErr && <div className="db-error">{formErr}</div>}
          <div className="db-form">
            <div className="db-field">
              <label className="db-label">Title *</label>
              <input className="db-input" value={form.title} onChange={e => setForm(f => ({ ...f, title: e.target.value }))} placeholder="e.g. Classic White Tee" />
            </div>
            <div className="db-form-row">
              <div className="db-field">
                <label className="db-label">Price (€) *</label>
                <input className="db-input" type="number" step="0.01" min="0.01" value={form.price} onChange={e => setForm(f => ({ ...f, price: e.target.value }))} placeholder="0.00" />
              </div>
              <div className="db-field">
                <label className="db-label">Category *</label>
                <input className="db-input" list="men-cats" value={form.categoryName} onChange={e => setForm(f => ({ ...f, categoryName: e.target.value }))} placeholder="e.g. Tops, Outerwear…" />
                <datalist id="men-cats">{cats.map(c => <option key={c.id} value={c.name} />)}</datalist>
              </div>
            </div>
            <div className="db-field">
              <label className="db-label">Image URL</label>
              <input className="db-input" value={form.imageUrl} onChange={e => setForm(f => ({ ...f, imageUrl: e.target.value }))} placeholder="https://example.com/image.jpg (optional)" />
              {form.imageUrl && <img src={form.imageUrl} alt="preview" style={{ width: 80, height: 80, objectFit: 'cover', borderRadius: 6, marginTop: 6 }} onError={e => { e.target.style.display = 'none' }} />}
            </div>
            <div className="db-field">
              <label className="db-label">Color</label>
              <input className="db-input" value={form.color} onChange={e => setForm(f => ({ ...f, color: e.target.value }))} placeholder="e.g. Blue, Black (optional)" />
            </div>
            <div className="db-field">
              <label className="db-label">Description</label>
              <textarea className="db-textarea" value={form.description} onChange={e => setForm(f => ({ ...f, description: e.target.value }))} placeholder="Product description (optional)" />
            </div>
          </div>
        </Modal>
      )}
    </div>
  )
}

// ── Categories Tab ─────────────────────────────────────────────────────────
function CategoriesTab() {
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [form, setForm] = useState({ name: '' })
  const [saving, setSaving] = useState(false)
  const [formErr, setFormErr] = useState('')

  const { data: items, loading, error, reload } = useApi(async () => {
    const r = await api.get('/MenCategory')
    return r.data
  }, [])

  const openAdd = () => { setForm({ name: '' }); setFormErr(''); setModal('add') }
  const openEdit = (c) => { setSelected(c); setForm({ name: c.name }); setFormErr(''); setModal('edit') }

  const save = async () => {
    setSaving(true); setFormErr('')
    if (!form.name.trim()) { setFormErr('Name is required.'); setSaving(false); return }
    try {
      if (modal === 'add') await api.post('/MenCategory', form)
      else await api.put(`/MenCategory/${selected.id}`, form)
      setModal(null); reload()
    } catch (e) { setFormErr(e?.response?.data?.message ?? 'Save failed.') }
    finally { setSaving(false) }
  }

  const del = async (c) => {
    if (!window.confirm(`Delete category "${c.name}"?`)) return
    try { await api.delete(`/MenCategory/${c.id}`); reload() }
    catch (e) { window.alert(e?.response?.data?.message ?? 'Delete failed.') }
  }

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <span className="db-stat-label">{items.length} categories</span>
        <button className="db-btn db-btn--primary" onClick={openAdd}>+ Add Category</button>
      </div>
      {error && <div className="db-error">{error}</div>}
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr><th>NAME</th><th>ACTIONS</th></tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={2} className="db-empty">Loading…</td></tr>
            ) : items.length === 0 ? (
              <tr><td colSpan={2} className="db-empty">No categories found.</td></tr>
            ) : items.map(c => (
              <tr key={c.id}>
                <td>{c.name}</td>
                <td>
                  <div style={{ display: 'flex', gap: 6 }}>
                    <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => openEdit(c)}>Edit</button>
                    <button className="db-btn db-btn--danger db-btn--sm" onClick={() => del(c)}>Delete</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {modal && (
        <Modal title={modal === 'add' ? 'Add Category' : 'Edit Category'} onClose={() => setModal(null)}
          actions={<>
            <button className="db-btn db-btn--ghost" onClick={() => setModal(null)}>Cancel</button>
            <button className="db-btn db-btn--primary" onClick={save} disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
          </>}>
          {formErr && <div className="db-error">{formErr}</div>}
          <div className="db-form">
            <div className="db-field">
              <label className="db-label">Name</label>
              <input className="db-input" value={form.name} onChange={e => setForm({ name: e.target.value })} autoFocus placeholder="e.g. Tops" />
            </div>
          </div>
        </Modal>
      )}
    </div>
  )
}

// ── Sales Tab ──────────────────────────────────────────────────────────────
function SalesTab() {
  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [discount, setDiscount] = useState('')
  const [saving, setSaving] = useState(false)
  const [saveErr, setSaveErr] = useState('')

  const { data: products, loading, error, reload } = useApi(async () => {
    const r = await api.get('/MenCards')
    return r.data
  }, [])

  const filtered = products
    .filter(p => !q || p.title?.toLowerCase().includes(q.toLowerCase()))
    .sort((a, b) => ((b.discountPercent || 0) > 0 ? 1 : 0) - ((a.discountPercent || 0) > 0 ? 1 : 0))

  const openSale = (p) => {
    setSelected(p); setDiscount(p.discountPercent ?? ''); setSaveErr(''); setModal('sale')
  }

  const applySale = async () => {
    setSaving(true); setSaveErr('')
    const pct = parseInt(discount) || 0
    try {
      await api.put(`/MenCards/${selected.id}`, {
        title: selected.title,
        imageUrl: selected.imageUrl || null,
        price: selected.price,
        menCategoryId: selected.menCategoryId,
        description: selected.description || '',
        color: selected.color || null,
        discountPercent: pct,
      })
      setModal(null); reload()
    } catch (e) { setSaveErr(e?.response?.data?.message ?? 'Failed to apply sale.') }
    finally { setSaving(false) }
  }

  const removeSale = async (p) => {
    try {
      await api.put(`/MenCards/${p.id}`, {
        title: p.title,
        imageUrl: p.imageUrl || null,
        price: p.price,
        menCategoryId: p.menCategoryId,
        description: p.description || '',
        color: p.color || null,
        discountPercent: 0,
      })
      reload()
    } catch (e) { window.alert(e?.response?.data?.message ?? 'Failed.') }
  }

  const discountedPrice = selected
    ? (parseFloat(selected.price) * (1 - (parseInt(discount) || 0) / 100)).toFixed(2)
    : ''

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <input className="db-search" placeholder="Search products…" value={q} onChange={e => setQ(e.target.value)} />
        <span className="db-stat-label" style={{ flexShrink: 0 }}>
          {products.filter(p => p.discountPercent > 0).length} items on sale
        </span>
      </div>
      {error && <div className="db-error">{error}</div>}
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr><th></th><th>PRODUCT</th><th>ORIGINAL PRICE</th><th>SALE</th><th>ACTIONS</th></tr></thead>
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
                  {p.discountPercent > 0
                    ? <span className="db-badge db-badge--green">{p.discountPercent}% OFF</span>
                    : <span className="db-badge db-badge--grey">No sale</span>}
                </td>
                <td>
                  <div style={{ display: 'flex', gap: 6 }}>
                    <button className="db-btn db-btn--primary db-btn--sm" onClick={() => openSale(p)}>
                      {p.discountPercent > 0 ? 'Edit Sale' : 'Set Sale'}
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
        <Modal
          title={selected?.discountPercent > 0 ? 'Edit Sale Price' : 'Apply Sale Price'}
          onClose={() => setModal(null)}
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

// ── Main ───────────────────────────────────────────────────────────────────
export default function MenManagerDashboard() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const [activeTab, setActiveTab] = useState('overview')

  const { data: products }   = useApi(async () => { const r = await api.get('/MenCards');    return r.data }, [])
  const { data: categories } = useApi(async () => { try { const r = await api.get('/MenCategory'); return r.data } catch { return [] } }, [])

  const handleLogout = async () => { await logout(); navigate('/login') }

  const renderContent = () => {
    switch (activeTab) {
      case 'overview': return (
        <div className="db-stats">
          <div className="db-stat-card"><span className="db-stat-label">Total Products</span><span className="db-stat-value">{products.length}</span></div>
          <div className="db-stat-card"><span className="db-stat-label">Categories</span><span className="db-stat-value">{categories.length}</span></div>
          <div className="db-stat-card"><span className="db-stat-label">On Sale</span><span className="db-stat-value">{products.filter(p => p.discountPercent > 0).length}</span></div>
        </div>
      )
      case 'products':   return <ProductsTab categories={categories} />
      case 'categories': return <CategoriesTab />
      case 'sales':      return <SalesTab />
      default: return null
    }
  }

  return (
    <div className="db-root">
      <aside className="db-sidebar">
        <div className="db-brand">MAIA</div>
        <div className="db-role-badge">Men Manager</div>
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
          <button className="db-home-btn" onClick={() => { navigate('/'); window.scrollTo(0, 0) }}>HOME PAGE</button>
        </div>
      </aside>
      <main className="db-main">
        <header className="db-header">
          <h1 className="db-header-title">{TABS.find(t => t.key === activeTab)?.label ?? 'Men Manager'}</h1>
        </header>
        <div className="db-content">{renderContent()}</div>
      </main>
    </div>
  )
}
