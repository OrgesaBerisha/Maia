import { Link } from 'react-router-dom'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './StoresPage.css'

const STORES = [
  {
    city: 'MILAN',
    address: 'Via della Spiga 12',
    postalCode: '20121 Milan, Italy',
    hours: 'Mon–Sat  10:00–20:00   ·   Sun  11:00–18:00',
    phone: '+39 02 7600 1234',
  },
  {
    city: 'PARIS',
    address: '8 Rue du Faubourg Saint-Honoré',
    postalCode: '75008 Paris, France',
    hours: 'Mon–Sat  10:00–20:00   ·   Sun  12:00–18:00',
    phone: '+33 1 42 68 00 00',
  },
  {
    city: 'NEW YORK',
    address: '680 Madison Avenue',
    postalCode: 'NY 10065 New York, USA',
    hours: 'Mon–Sat  10:00–20:00   ·   Sun  12:00–18:00',
    phone: '+1 212 940 0000',
  },
  {
    city: 'LONDON',
    address: '175 New Bond Street',
    postalCode: 'W1S 4RQ London, UK',
    hours: 'Mon–Sat  10:00–19:00   ·   Sun  12:00–17:00',
    phone: '+44 20 7495 0000',
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
        <p className="stores-sub">Visit us in one of our boutiques around the world.</p>

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
