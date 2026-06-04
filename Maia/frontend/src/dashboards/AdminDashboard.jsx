import { useState, useEffect, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../AuthContext.jsx'
import api from '../api/axios.js'
import './DashboardLayout.css'
import './AdminDashboard.css'

const TABS = [
  { key: 'overview',  label: 'Overview',       icon: '◈' },
  { key: 'users',     label: 'Customers',       icon: '◉' },
  { key: 'staff',     label: 'Staff',           icon: '◎' },
  { key: 'women',     label: 'Women Section',   icon: '◇' },
  { key: 'men',       label: 'Men Section',     icon: '◆' },
  { key: 'kids',      label: 'Kids Section',    icon: '◈' },
  { key: 'sales',     label: 'Sales',           icon: '◑' },
]

const ROLE_COLORS = {
  Admin: 'db-badge--blue',
  SalesManager: 'db-badge--warm',
  WomenManager: 'db-badge--green',
  MenManager:   'db-badge--blue',
  KidsManager:  'db-badge--warm',
  Customer:     'db-badge--grey',
}

function useApi(fetcher, deps = []) {
  const [data, setData]   = useState([])
  const [loading, setL]   = useState(true)
  const [error, setErr]   = useState('')

  const load = useCallback(async () => {
    setL(true); setErr('')
    try { setData(await fetcher()) }
    catch (e) { setErr(e?.response?.data?.message ?? 'Failed to load.') }
    finally { setL(false) }
  }, deps)

  useEffect(() => { load() }, [load])
  return { data, loading, error, reload: load, setData }
}

// ── Modal ──────────────────────────────────────────────────────────────────
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

// ── Overview Tab ───────────────────────────────────────────────────────────
function OverviewTab({ stats }) {
  return (
    <div>
      <div className="db-stats">
        {stats.map(s => (
          <div key={s.label} className="db-stat-card">
            <span className="db-stat-label">{s.label}</span>
            <span className="db-stat-value">{s.value}</span>
          </div>
        ))}
      </div>
    </div>
  )
}

// ── Customers Tab ──────────────────────────────────────────────────────────
function CustomersTab() {
  const [q, setQ] = useState('')
  const { data: users, loading, error, reload } = useApi(async () => {
    const r = await api.get('/users/customers')
    return r.data
  }, [])

  const filtered = users.filter(u =>
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
          <thead><tr>
            <th>NAME</th><th>EMAIL</th><th>JOINED</th>
          </tr></thead>
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

// ── Staff Tab ──────────────────────────────────────────────────────────────
function StaffTab() {
  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null) // null | 'add' | 'edit' | 'role'
  const [selected, setSelected] = useState(null)
  const [saving, setSaving] = useState(false)
  const [formErr, setFormErr] = useState('')
  const [form, setForm] = useState({ firstName: '', lastName: '', email: '', password: '', roleType: 'WomenManager' })

  const { data: allUsers, loading, error, reload } = useApi(async () => {
    const r = await api.get('/users')
    return r.data
  }, [])

  const { data: roles } = useApi(async () => {
    const r = await api.get('/users/roles')
    return r.data
  }, [])

  const staff = allUsers.filter(u => u.role?.roleType !== 'Customer')
  const filtered = staff.filter(u =>
    !q || `${u.firstName} ${u.lastName} ${u.email}`.toLowerCase().includes(q.toLowerCase())
  )

  const openAdd = () => {
    setForm({ firstName: '', lastName: '', email: '', password: '', roleType: 'WomenManager' })
    setFormErr(''); setModal('add')
  }

  const openEdit = (u) => {
    setSelected(u)
    setForm({ firstName: u.firstName, lastName: u.lastName, email: u.email, password: '', roleType: u.role?.roleType ?? '' })
    setFormErr(''); setModal('edit')
  }

  const save = async () => {
    setSaving(true); setFormErr('')
    try {
      if (modal === 'add') {
        await api.post('/users/staff', form)
      } else {
        await api.put(`/users/${selected.userID}`, {
          firstName: form.firstName,
          lastName: form.lastName,
          email: form.email,
          password: form.password || undefined,
        })
        const role = roles.find(r => r.roleType === form.roleType)
        if (role) await api.put('/users/role', { userID: selected.userID, newRoleID: role.roleID })
      }
      setModal(null); reload()
    } catch (e) {
      setFormErr(e?.response?.data?.message ?? 'Save failed.')
    } finally { setSaving(false) }
  }

  const deleteUser = async (u) => {
    if (!confirm(`Delete ${u.firstName} ${u.lastName}?`)) return
    try { await api.delete(`/users/${u.userID}`); reload() }
    catch (e) { alert(e?.response?.data?.message ?? 'Delete failed.') }
  }

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <input className="db-search" placeholder="Search staff…" value={q} onChange={e => setQ(e.target.value)} />
        <button className="db-btn db-btn--primary" onClick={openAdd}>+ Add Staff</button>
      </div>
      {error && <div className="db-error">{error}</div>}
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr>
            <th>NAME</th><th>EMAIL</th><th>ROLE</th><th>STATUS</th><th>ACTIONS</th>
          </tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={5} className="db-empty">Loading…</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan={5} className="db-empty">No staff found.</td></tr>
            ) : filtered.map(u => (
              <tr key={u.userID}>
                <td>{u.firstName} {u.lastName}</td>
                <td>{u.email}</td>
                <td><span className={`db-badge ${ROLE_COLORS[u.role?.roleType] ?? 'db-badge--grey'}`}>{u.role?.roleType ?? '—'}</span></td>
                <td><span className={`db-badge ${u.isActive ? 'db-badge--green' : 'db-badge--red'}`}>{u.isActive ? 'Active' : 'Disabled'}</span></td>
                <td className="db-actions-cell">
                  <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => openEdit(u)}>Edit</button>
                  <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => api.put(`/users/${u.userID}/status`, !u.isActive).then(reload)}>
                    {u.isActive ? 'Disable' : 'Enable'}
                  </button>
                  <button className="db-btn db-btn--danger db-btn--sm" onClick={() => deleteUser(u)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {modal && (
        <Modal
          title={modal === 'add' ? 'Add Staff Member' : 'Edit Staff Member'}
          onClose={() => setModal(null)}
          actions={<>
            <button className="db-btn db-btn--ghost" onClick={() => setModal(null)}>Cancel</button>
            <button className="db-btn db-btn--primary" onClick={save} disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
          </>}
        >
          {formErr && <div className="db-error">{formErr}</div>}
          <div className="db-form">
            <div className="db-form-row">
              <div className="db-field">
                <label className="db-label">First Name</label>
                <input className="db-input" value={form.firstName} onChange={e => setForm(f => ({ ...f, firstName: e.target.value }))} />
              </div>
              <div className="db-field">
                <label className="db-label">Last Name</label>
                <input className="db-input" value={form.lastName} onChange={e => setForm(f => ({ ...f, lastName: e.target.value }))} />
              </div>
            </div>
            <div className="db-field">
              <label className="db-label">Email</label>
              <input className="db-input" type="email" value={form.email} onChange={e => setForm(f => ({ ...f, email: e.target.value }))} />
            </div>
            <div className="db-field">
              <label className="db-label">Password {modal === 'edit' && '(leave blank to keep)'}</label>
              <input className="db-input" type="password" value={form.password} onChange={e => setForm(f => ({ ...f, password: e.target.value }))} />
            </div>
            <div className="db-field">
              <label className="db-label">Role</label>
              <select className="db-select" value={form.roleType} onChange={e => setForm(f => ({ ...f, roleType: e.target.value }))}>
                {roles.filter(r => r.roleType !== 'Customer').map(r => (
                  <option key={r.roleID} value={r.roleType}>{r.roleType}</option>
                ))}
              </select>
            </div>
          </div>
        </Modal>
      )}
    </div>
  )
}

// ── Products Tab (reusable for Women/Men/Kids) ─────────────────────────────
function ProductsTab({ section }) {
  const cfg = {
    women: { endpoint: '/CardsWomen', catKey: 'womanCategoryId', catLabel: 'Category', catEndpoint: '/WomanCategory', idKey: 'id' },
    men:   { endpoint: '/MenCards',   catKey: 'menCategoryId',   catLabel: 'Category', catEndpoint: '/MenCategory',   idKey: 'id' },
    kids:  { endpoint: '/KidsCards',  catKey: 'kidsCategoryId',  catLabel: 'Category', catEndpoint: '/KidsCategory',  idKey: 'id' },
  }[section]

  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [saving, setSaving] = useState(false)
  const [formErr, setFormErr] = useState('')
  const [form, setForm] = useState({ title: '', description: '', price: '', imageUrl: '', [cfg.catKey]: '' })

  const { data: products, loading, error, reload } = useApi(async () => {
    const r = await api.get(`/api${cfg.endpoint}`)
    return r.data
  }, [section])

  const { data: categories } = useApi(async () => {
    try { const r = await api.get(`/api${cfg.catEndpoint}`); return r.data }
    catch { return [] }
  }, [section])

  const filtered = products.filter(p =>
    !q || p.title?.toLowerCase().includes(q.toLowerCase())
  )

  const openAdd = () => {
    setForm({ title: '', description: '', price: '', imageUrl: '', [cfg.catKey]: categories[0]?.id ?? '' })
    setFormErr(''); setModal('add')
  }

  const openEdit = (p) => {
    setSelected(p)
    setForm({ title: p.title, description: p.description ?? '', price: p.price, imageUrl: p.imageUrl ?? '', [cfg.catKey]: p[cfg.catKey] ?? '' })
    setFormErr(''); setModal('edit')
  }

  const save = async () => {
    setSaving(true); setFormErr('')
    const body = { ...form, price: parseFloat(form.price) }
    try {
      if (modal === 'add') await api.post(`/api${cfg.endpoint}`, body)
      else await api.put(`/api${cfg.endpoint}/${selected[cfg.idKey]}`, body)
      setModal(null); reload()
    } catch (e) { setFormErr(e?.response?.data?.message ?? 'Save failed.') }
    finally { setSaving(false) }
  }

  const del = async (p) => {
    if (!confirm(`Delete "${p.title}"?`)) return
    try { await api.delete(`/api${cfg.endpoint}/${p[cfg.idKey]}`); reload() }
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
            <th></th><th>TITLE</th><th>PRICE</th><th>CATEGORY</th><th>ACTIONS</th>
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
                <td>{p.womanCategory?.name ?? p.menCategory?.name ?? p.kidsCategory?.name ?? p[cfg.catKey] ?? '—'}</td>
                <td className="db-actions-cell">
                  <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => openEdit(p)}>Edit</button>
                  <button className="db-btn db-btn--danger db-btn--sm" onClick={() => del(p)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {modal && (
        <Modal
          title={modal === 'add' ? 'Add Product' : 'Edit Product'}
          onClose={() => setModal(null)}
          actions={<>
            <button className="db-btn db-btn--ghost" onClick={() => setModal(null)}>Cancel</button>
            <button className="db-btn db-btn--primary" onClick={save} disabled={saving}>{saving ? 'Saving…' : 'Save'}</button>
          </>}
        >
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
                <label className="db-label">Category</label>
                <select className="db-select" value={form[cfg.catKey]} onChange={e => setForm(f => ({ ...f, [cfg.catKey]: e.target.value }))}>
                  {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
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

// ── Sales Tab ──────────────────────────────────────────────────────────────
function SalesTab() {
  const [section, setSection] = useState('kids')
  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [discount, setDiscount] = useState('')
  const [saving, setSaving] = useState(false)

  const endpoint = { women: '/CardsWomen', men: '/MenCards', kids: '/KidsCards' }[section]

  const { data: products, loading, reload } = useApi(async () => {
    const r = await api.get(`/api${endpoint}`)
    return r.data
  }, [section])

  const filtered = products.filter(p =>
    !q || p.title?.toLowerCase().includes(q.toLowerCase())
  )

  const openSale = (p) => {
    setSelected(p)
    setDiscount(p.discountPercent ?? '')
    setModal('sale')
  }

  const applyDiscount = async () => {
    setSaving(true)
    try {
      if (section === 'kids') {
        await api.patch(`/api/KidsCards/${selected.id}/sale`, { discountPercent: parseInt(discount) || 0 })
      } else {
        const newPrice = parseFloat(selected.originalPrice ?? selected.price) * (1 - (parseInt(discount) || 0) / 100)
        await api.put(`/api${endpoint}/${selected.id}`, { ...selected, price: parseFloat(newPrice.toFixed(2)) })
      }
      setModal(null); reload()
    } catch (e) { alert(e?.response?.data?.message ?? 'Failed.') }
    finally { setSaving(false) }
  }

  return (
    <div className="db-section">
      <div className="db-toolbar">
        <div style={{ display: 'flex', gap: 8 }}>
          {['women', 'men', 'kids'].map(s => (
            <button key={s} className={`db-btn ${section === s ? 'db-btn--primary' : 'db-btn--ghost'}`}
              onClick={() => { setSection(s); setQ('') }}>
              {s.charAt(0).toUpperCase() + s.slice(1)}
            </button>
          ))}
        </div>
        <input className="db-search" placeholder="Search products…" value={q} onChange={e => setQ(e.target.value)} />
      </div>
      <div className="db-table-wrap">
        <table className="db-table">
          <thead><tr>
            <th></th><th>TITLE</th><th>PRICE</th><th>DISCOUNT</th><th>ACTIONS</th>
          </tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={5} className="db-empty">Loading…</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan={5} className="db-empty">No products.</td></tr>
            ) : filtered.map(p => (
              <tr key={p.id}>
                <td><img className="db-img-preview" src={p.imageUrl || 'https://placehold.co/56x56?text=No+Img'} alt={p.title} /></td>
                <td>{p.title}</td>
                <td>€{Number(p.price).toFixed(2)}</td>
                <td>{p.discountPercent ? <span className="db-badge db-badge--green">{p.discountPercent}% off</span> : '—'}</td>
                <td className="db-actions-cell">
                  <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => openSale(p)}>Set Sale</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {modal === 'sale' && (
        <Modal title="Set Discount" onClose={() => setModal(null)}
          actions={<>
            <button className="db-btn db-btn--ghost" onClick={() => setModal(null)}>Cancel</button>
            <button className="db-btn db-btn--primary" onClick={applyDiscount} disabled={saving}>{saving ? 'Applying…' : 'Apply'}</button>
          </>}>
          <div className="db-form">
            <div className="db-field">
              <label className="db-label">Product</label>
              <input className="db-input" value={selected?.title} disabled />
            </div>
            <div className="db-field">
              <label className="db-label">Discount % (0 = remove sale)</label>
              <input className="db-input" type="number" min="0" max="100" value={discount}
                onChange={e => setDiscount(e.target.value)} placeholder="e.g. 20" />
            </div>
          </div>
        </Modal>
      )}
    </div>
  )
}

// ── Main Admin Dashboard ────────────────────────────────────────────────────
export default function AdminDashboard() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const [activeTab, setActiveTab] = useState('overview')

  const { data: allUsers } = useApi(async () => { const r = await api.get('/users'); return r.data }, [])
  const { data: customers } = useApi(async () => { const r = await api.get('/users/customers'); return r.data }, [])
  const { data: women } = useApi(async () => { const r = await api.get('/api/CardsWomen'); return r.data }, [])
  const { data: men }   = useApi(async () => { const r = await api.get('/api/MenCards');   return r.data }, [])
  const { data: kids }  = useApi(async () => { const r = await api.get('/api/KidsCards');  return r.data }, [])

  const staff = allUsers.filter(u => u.role?.roleType !== 'Customer')

  const stats = [
    { label: 'Customers',       value: customers.length },
    { label: 'Staff Members',   value: staff.length },
    { label: 'Women Products',  value: women.length },
    { label: 'Men Products',    value: men.length },
    { label: 'Kids Products',   value: kids.length },
  ]

  const handleLogout = async () => { await logout(); navigate('/login') }

  const renderContent = () => {
    switch (activeTab) {
      case 'overview': return <OverviewTab stats={stats} />
      case 'users':    return <CustomersTab />
      case 'staff':    return <StaffTab />
      case 'women':    return <ProductsTab section="women" />
      case 'men':      return <ProductsTab section="men" />
      case 'kids':     return <ProductsTab section="kids" />
      case 'sales':    return <SalesTab />
      default: return null
    }
  }

  return (
    <div className="db-root">
      <aside className="db-sidebar">
        <div className="db-brand">MAIA</div>
        <div className="db-role-badge">Admin</div>
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
            {TABS.find(t => t.key === activeTab)?.label ?? 'Admin Dashboard'}
          </h1>
        </header>
        <div className="db-content">{renderContent()}</div>
      </main>
    </div>
  )
}
