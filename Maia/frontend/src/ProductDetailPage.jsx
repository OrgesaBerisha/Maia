import { useState } from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import { useCart } from './CartContext.jsx'
import { useWishlist } from './WishlistContext.jsx'
import { useAuth } from './AuthContext.jsx'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './ProductDetailPage.css'

const SIZES = ['XS', 'S', 'M', 'L', 'XL', 'XXL']

function ProductDetailPage() {
  const navigate = useNavigate()
  const location = useLocation()
  const { addToBag } = useCart()
  const { isWishlisted, toggleWishlist } = useWishlist()
  const { isLoggedIn } = useAuth()

  const product = location.state?.product
  const [selectedSize, setSelectedSize] = useState(null)
  const [added, setAdded] = useState(false)

  if (!product) {
    navigate(-1)
    return null
  }

  const handleAddToBag = () => {
    if (!selectedSize || !product) return
    addToBag({ ...product, size: selectedSize })
    setAdded(true)
    setTimeout(() => setAdded(false), 2000)
  }

  const wishlisted = isWishlisted(product.id, product.source)

  return (
    <div className="pd-page">
      <svg className="pd-blob" viewBox="0 0 1440 220" preserveAspectRatio="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
        <path d="M0,0 L1440,0 L1440,140 C1340,162 1200,178 1060,164 C920,150 800,118 660,132 C520,146 380,178 240,188 C160,194 80,190 0,196 Z" fill="#d4c5b3" />
      </svg>

      <header className="pd-header">
        <button className="pd-back" onClick={() => navigate(-1)}>←</button>
        <SiteLogo />
      </header>

      <main className="pd-main">
        <div className="pd-layout">

          {/* Left — Image */}
          <div className="pd-img-wrap">
            <img src={product.image} alt={product.name} className="pd-img" />
            {product.discountPercent && (
              <span className="pd-sale-badge">−{product.discountPercent}%</span>
            )}
            {isLoggedIn && (
              <button
                className={`pd-wishlist${wishlisted ? ' pd-wishlist--active' : ''}`}
                onClick={() => toggleWishlist(product)}
                aria-label="Wishlist"
              >
                {wishlisted ? '♥' : '♡'}
              </button>
            )}
          </div>

          {/* Right — Info */}
          <div className="pd-info">
            {product.category && <p className="pd-category">{product.category}</p>}
            <h1 className="pd-name">{product.name}</h1>

            <div className="pd-price-row">
              {product.salePrice ? (
                <>
                  <span className="pd-price pd-price--original">{product.price} EUR</span>
                  <span className="pd-price pd-price--sale">{product.salePrice} EUR</span>
                </>
              ) : (
                <span className="pd-price">{product.price} EUR</span>
              )}
            </div>

            {product.color && (
              <p className="pd-color">COLOR: <strong>{product.color.toUpperCase()}</strong></p>
            )}

            {product.description && (
              <p className="pd-description">{product.description}</p>
            )}

            <div className="pd-divider" />

            <p className="pd-size-label">SELECT SIZE</p>
            <div className="pd-sizes">
              {SIZES.map(s => (
                <button
                  key={s}
                  className={`pd-size-btn${selectedSize === s ? ' pd-size-btn--active' : ''}`}
                  onClick={() => setSelectedSize(s)}
                >
                  {s}
                </button>
              ))}
            </div>

            <button
              className={`pd-add-btn${!selectedSize ? ' pd-add-btn--disabled' : ''}${added ? ' pd-add-btn--done' : ''}`}
              onClick={handleAddToBag}
              disabled={!selectedSize}
            >
              {added ? '✓ ADDED TO BAG' : 'ADD TO BAG'}
            </button>
          </div>
        </div>
      </main>

      <BottomNav />
    </div>
  )
}

export default ProductDetailPage
