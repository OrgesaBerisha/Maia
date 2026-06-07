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
    catch (e) {
      if (e?.response?.status === 401) setErr('Session expired — please log out and log back in.')
      else setErr(e?.response?.data?.message ?? 'Failed to load.')
    }
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

  const deleteUser = async (u) => {
    if (!confirm(`Delete customer ${u.firstName} ${u.lastName}?`)) return
    try { await api.delete(`/users/${u.userID}`); reload() }
    catch (e) { alert(e?.response?.data?.message ?? 'Delete failed.') }
  }

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
            <th>NAME</th><th>EMAIL</th><th>JOINED</th><th>ACTIONS</th>
          </tr></thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={4} className="db-empty">Loading…</td></tr>
            ) : filtered.length === 0 ? (
              <tr><td colSpan={4} className="db-empty">No customers found.</td></tr>
            ) : filtered.map(u => (
              <tr key={u.userID ?? u.email}>
                <td>{u.firstName} {u.lastName}</td>
                <td>{u.email}</td>
                <td>{u.createdAt ? new Date(u.createdAt).toLocaleDateString() : '—'}</td>
                <td>
                  <div style={{ display: 'flex', gap: 6 }}>
                    <button className="db-btn db-btn--danger db-btn--sm" onClick={() => deleteUser(u)}>Delete</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

// ── Staff Tab ──────────────────────────────────────────────────────────────
const MANAGER_ROLES = ['WomenManager', 'MenManager', 'KidsManager', 'SalesManager']

function StaffTab() {
  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
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

  const staff = allUsers.filter(u => MANAGER_ROLES.includes(u.roleType))
  const filtered = staff.filter(u =>
    !q || `${u.firstName} ${u.lastName} ${u.email}`.toLowerCase().includes(q.toLowerCase())
  )

  const openAdd = () => {
    setForm({ firstName: '', lastName: '', email: '', password: '', roleType: 'WomenManager' })
    setFormErr(''); setModal('add')
  }

  const openEdit = (u) => {
    setSelected(u)
    setForm({ firstName: u.firstName, lastName: u.lastName, email: u.email, password: '', roleType: u.roleType ?? '' })
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
                <td><span className={`db-badge ${ROLE_COLORS[u.roleType] ?? 'db-badge--grey'}`}>{u.roleType ?? '—'}</span></td>
                <td><span className={`db-badge ${u.isActive ? 'db-badge--green' : 'db-badge--red'}`}>{u.isActive ? 'Active' : 'Disabled'}</span></td>
                <td>
                  <div style={{ display: 'flex', gap: 6 }}>
                    <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => openEdit(u)}>Edit</button>
                    <button className="db-btn db-btn--ghost db-btn--sm" onClick={() => api.put(`/users/${u.userID}/status`, !u.isActive, { headers: { 'Content-Type': 'application/json' } }).then(reload).catch(e => alert(e?.response?.data?.message ?? 'Status update failed.'))}>
                      {u.isActive ? 'Disable' : 'Enable'}
                    </button>
                    <button className="db-btn db-btn--danger db-btn--sm" onClick={() => deleteUser(u)}>Delete</button>
                  </div>
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
                {roles.filter(r => MANAGER_ROLES.includes(r.roleType)).map(r => (
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

const ADMIN_FALLBACK_CATS = {
  women: [{id:1,name:'Tops'},{id:2,name:'Dresses'},{id:3,name:'Bottoms'},{id:4,name:'Outerwear'},{id:5,name:'Swimwear'},{id:6,name:'Matching Sets'},{id:7,name:'Footwear'},{id:8,name:'Accessories'}],
  men:   [{id:1,name:'Tops'},{id:2,name:'Bottoms'},{id:3,name:'Suits & Formalwear'},{id:4,name:'Outerwear'},{id:5,name:'Swimwear'},{id:6,name:'Footwear'},{id:7,name:'Accessories'}],
  kids:  [{id:1,name:'Baby'},{id:2,name:'Girls'},{id:3,name:'Boys'},{id:4,name:'Sleepwear'},{id:5,name:'Swimwear'},{id:6,name:'Footwear'},{id:7,name:'Accessories'},{id:8,name:'Sale'}],
}
const ADMIN_FALLBACK_TYPES = {
  kids: [{ id:1,name:'Boys'},{id:2,name:'Girls'}],
}

// ── Products Tab (reusable for Women/Men/Kids) ─────────────────────────────
function ProductsTab({ section }) {
  const cfg = {
    women: { endpoint: '/CardsWomen', catKey: 'womanCategoryId', catEndpoint: '/WomanCategory', idKey: 'id' },
    men:   { endpoint: '/MenCards',   catKey: 'menCategoryId',   catEndpoint: '/MenCategory',   idKey: 'id' },
    kids:  { endpoint: '/KidsCards',  catKey: 'kidsCategoryId',  catEndpoint: '/KidsCategory',  idKey: 'id',
             typeKey: 'kidsProductTypeId', typeEndpoint: '/KidsProductType' },
  }[section]

  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [saving, setSaving] = useState(false)
  const [formErr, setFormErr] = useState('')
  const [form, setForm] = useState({ title: '', description: '', price: '', imageUrl: '', categoryName: '', typeName: '' })

  const { data: products, loading, error, reload } = useApi(async () => {
    const r = await api.get(cfg.endpoint)
    return r.data
  }, [section])

  const { data: categories } = useApi(async () => {
    try { const r = await api.get(cfg.catEndpoint); return r.data }
    catch { return [] }
  }, [section])

  const { data: types } = useApi(async () => {
    if (!cfg.typeEndpoint) return []
    try { const r = await api.get(cfg.typeEndpoint); return r.data }
    catch { return [] }
  }, [section])

  const cats = categories.length > 0 ? categories : (ADMIN_FALLBACK_CATS[section] ?? [])
  const typs = types.length > 0 ? types : (ADMIN_FALLBACK_TYPES[section] ?? [])

  const getCatNameFromProduct = (p) => {
    if (section === 'women') return p.category ?? ''
    if (section === 'men')   return p.menCategoryName ?? ''
    if (section === 'kids')  return p.kidsCategoryName ?? ''
    return ''
  }

  const filtered = products.filter(p =>
    !q || p.title?.toLowerCase().includes(q.toLowerCase())
  )

  const openAdd = () => {
    setForm({ title: '', description: '', price: '', imageUrl: '', categoryName: '', typeName: '' })
    setFormErr(''); setModal('add')
  }

  const openEdit = (p) => {
    setSelected(p)
    setForm({
      title: p.title, description: p.description ?? '', price: p.price,
      imageUrl: p.imageUrl ?? '', categoryName: getCatNameFromProduct(p),
      typeName: p.kidsProductTypeName ?? ''
    })
    setFormErr(''); setModal('edit')
  }

  const save = async () => {
    setSaving(true); setFormErr('')
    const price = parseFloat(form.price)
    const cat = cats.find(c => c.name.toLowerCase() === form.categoryName.trim().toLowerCase())
    if (isNaN(price) || price <= 0) { setFormErr('Enter a valid price.'); setSaving(false); return }
    if (!form.categoryName.trim()) { setFormErr('Category is required.'); setSaving(false); return }
    if (!cat) { setFormErr(`Unknown category. Available: ${cats.map(c => c.name).join(', ')}`); setSaving(false); return }
    if (cfg.typeKey) {
      const typ = typs.find(t => t.name.toLowerCase() === form.typeName.trim().toLowerCase())
      if (!form.typeName.trim()) { setFormErr('Product type is required.'); setSaving(false); return }
      if (!typ) { setFormErr(`Unknown type. Available: ${typs.map(t => t.name).join(', ')}`); setSaving(false); return }
    }
    const typ = cfg.typeKey ? typs.find(t => t.name.toLowerCase() === form.typeName.trim().toLowerCase()) : null
    const body = { title: form.title, description: form.description, price, imageUrl: form.imageUrl }
    body[cfg.catKey] = cat.id
    if (cfg.typeKey && typ) body[cfg.typeKey] = typ.id
    try {
      if (modal === 'add') await api.post(cfg.endpoint, body)
      else await api.put(`${cfg.endpoint}/${selected[cfg.idKey]}`, body)
      setModal(null); reload()
    } catch (e) {
      const d = e?.response?.data
      setFormErr(d?.message ?? (d?.errors ? Object.values(d.errors).flat().join('; ') : null) ?? d?.title ?? 'Save failed.')
    }
    finally { setSaving(false) }
  }

  const del = async (p) => {
    if (!confirm(`Delete "${p.title}"?`)) return
    try { await api.delete(`${cfg.endpoint}/${p[cfg.idKey]}`); reload() }
    catch (e) {
      const d = e?.response?.data
      alert(d?.message ?? d?.title ?? 'Delete failed.')
    }
  }

  const getCategoryName = (p) => {
    if (section === 'women') return p.category || p.womanCategoryId || '—'
    if (section === 'men')   return p.menCategoryName || p.menCategoryId || '—'
    if (section === 'kids')  return p.kidsCategoryName || p.kidsCategoryId || '—'
    return '—'
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
                <td>{getCategoryName(p)}</td>
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
                <input className="db-input" list="admin-cats" value={form.categoryName} onChange={e => setForm(f => ({ ...f, categoryName: e.target.value }))} placeholder={`e.g. ${cats[0]?.name ?? 'category'}…`} />
                <datalist id="admin-cats">{cats.map(c => <option key={c.id} value={c.name} />)}</datalist>
              </div>
            </div>
            {cfg.typeKey && (
              <div className="db-field">
                <label className="db-label">Product Type</label>
                <input className="db-input" list="admin-types" value={form.typeName} onChange={e => setForm(f => ({ ...f, typeName: e.target.value }))} placeholder="e.g. Boys, Girls" />
                <datalist id="admin-types">{typs.map(t => <option key={t.id} value={t.name} />)}</datalist>
              </div>
            )}
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
  const [section, setSection] = useState('women')
  const [q, setQ] = useState('')
  const [modal, setModal] = useState(null)
  const [selected, setSelected] = useState(null)
  const [discount, setDiscount] = useState('')
  const [saving, setSaving] = useState(false)
  const [saveErr, setSaveErr] = useState('')

  const endpoint = { women: '/CardsWomen', men: '/MenCards', kids: '/KidsCards' }[section]

  const { data: products, loading, reload } = useApi(async () => {
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
    } catch (e) { setSaveErr(e?.response?.data?.message ?? 'Failed to apply sale.') }
    finally { setSaving(false) }
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
            <th></th><th>TITLE</th><th>ORIGINAL PRICE</th><th>SALE</th><th>ACTIONS</th>
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

// ── Main Admin Dashboard ────────────────────────────────────────────────────
export default function AdminDashboard() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const [activeTab, setActiveTab] = useState('overview')

  const { data: allUsers }  = useApi(async () => { const r = await api.get('/users');            return r.data }, [])
  const { data: customers } = useApi(async () => { const r = await api.get('/users/customers');  return r.data }, [])
  const { data: women }     = useApi(async () => { const r = await api.get('/CardsWomen');        return r.data }, [])
  const { data: men }       = useApi(async () => { const r = await api.get('/MenCards');          return r.data }, [])
  const { data: kids }      = useApi(async () => { const r = await api.get('/KidsCards');         return r.data }, [])

  const staff = allUsers.filter(u => u.roleType !== 'Customer')

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
          <button className="db-home-btn" onClick={() => navigate('/')}>HOME PAGE</button>
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
