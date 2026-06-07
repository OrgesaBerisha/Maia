import { Link } from 'react-router-dom'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './StoresPage.css'

const STORES = [
  {
    city: 'PRISHTINË',
    address: 'Bulevardi Nënë Tereza 15',
    postalCode: '10000 Prishtinë, Kosovë',
    hours: 'Hën–Sht  09:00–20:00   ·   Die  10:00–18:00',
    phone: '+383 38 200 300',
  },
  {
    city: 'PRIZREN',
    address: 'Sheshi Shadërvan 4',
    postalCode: '20000 Prizren, Kosovë',
    hours: 'Hën–Sht  09:00–20:00   ·   Die  10:00–18:00',
    phone: '+383 29 230 100',
  },
  {
    city: 'GJAKOVË',
    address: 'Rruga UÇK 22',
    postalCode: '50000 Gjakovë, Kosovë',
    hours: 'Hën–Sht  09:00–20:00   ·   Die  10:00–17:00',
    phone: '+383 390 320 200',
  },
  {
    city: 'GJILAN',
    address: 'Bulevardi Bill Clinton 8',
    postalCode: '60000 Gjilan, Kosovë',
    hours: 'Hën–Sht  09:00–20:00   ·   Die  10:00–17:00',
    phone: '+383 280 320 400',
  },
]

function StoresPage() {
  return (
    <div className="stores-page">
      <svg
        className="stores-blob"
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

      <header className="stores-header">
        <Link to="/profile" className="stores-back">← BACK</Link>
        <SiteLogo />
        <span className="stores-header-label">STORES</span>
      </header>

      <main className="stores-main">
        <h1 className="stores-heading">OUR STORES</h1>
        <p className="stores-sub">Vizitoni njërin nga dyqanet tona në Kosovë.</p>

        <div className="stores-grid">
          {STORES.map(store => (
            <div key={store.city} className="store-card">
              <h2 className="store-city">{store.city}</h2>
              <div className="store-divider" />
              <p className="store-address">{store.address}</p>
              <p className="store-postal">{store.postalCode}</p>
              <p className="store-hours">{store.hours}</p>
              <a
                href={`tel:${store.phone.replace(/\s/g, '')}`}
                className="store-phone"
              >
                {store.phone}
              </a>
            </div>
          ))}
        </div>
      </main>

      <BottomNav />
    </div>
  )
}

export default StoresPage
