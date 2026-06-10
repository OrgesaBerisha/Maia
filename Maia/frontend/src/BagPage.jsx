import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useCart } from './CartContext.jsx'
import { useWishlist } from './WishlistContext.jsx'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import './BagPage.css'

function IconBagFilled() {
  return (
    <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
      <path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4H6z"/>
      <path d="M3 6h18M16 10a4 4 0 0 1-8 0" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
    </svg>
  )
}

function BagItem({ item, onDelete, onSave, onMoveToCart, isFav }) {
  return (
    <div className="bag-item">
      <img src={item.image} alt={item.name} className="bag-item-img" />
      <div className="bag-item-details">
        <span className="bag-item-name">{item.name}</span>
        <span className="bag-item-size">{item.size} |</span>
        <span className="bag-item-price">{item.price}</span>
        <div className="bag-item-actions">
          <button className="bag-action" onClick={onDelete}>DELETE</button>
          <span className="bag-action-sep">|</span>
          {isFav
            ? <button className="bag-action" onClick={onMoveToCart}>MOVE TO BAG</button>
            : <button className="bag-action" onClick={onSave}>SAVE</button>
          }
        </div>
      </div>
    </div>
  )
}

function BagPage() {
  const [tab, setTab] = useState('bag')
  const { bag, removeFromBag, saveForLater, addToBag } = useCart()
  const { items: wishlistItems, toggleWishlist } = useWishlist()

  const moveWishlistToCart = async (item) => {
    await addToBag({ id: item.productId, name: item.name, image: item.image, price: item.price, productSource: (item.source ?? 'WOMAN').toLowerCase() })
    await toggleWishlist({ id: item.productId, source: item.source ?? 'WOMAN' })
  }

  const bagTotal = bag.reduce((sum, i) => {
    const num = parseFloat(i.price)
    return isNaN(num) ? sum : sum + num
  }, 0)

  return (
    <div className="bag-page">
      <svg
        className="bag-blob"
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

      <header className="bag-header">
        <div className="bag-tabs">
          <button
            className={`bag-tab${tab === 'bag' ? ' bag-tab--active' : ''}`}
            onClick={() => setTab('bag')}
          >
            SHOPPING BAG
            <IconBagFilled />
            {bag.length > 0 && <span className="bag-count">{bag.length}</span>}
          </button>
          <button
            className={`bag-tab${tab === 'favorites' ? ' bag-tab--active' : ''}`}
            onClick={() => setTab('favorites')}
          >
            FAVORITES
            {wishlistItems.length > 0 && <span className="bag-count">{wishlistItems.length}</span>}
          </button>
        </div>
        <SiteLogo />
      </header>

      <main className="bag-main">
        {tab === 'bag' && (
          <>
            {bag.length === 0 ? (
              <p className="bag-empty">YOUR BAG IS EMPTY</p>
            ) : (
              <>
                {bag.map((item, i) => (
                  <BagItem
                    key={`${item.id}-${item.size}-${i}`}
                    item={item}
                    isFav={false}
                    onDelete={() => removeFromBag(item.id, item.size)}
                    onSave={() => saveForLater(item.id, item.size)}
                  />
                ))}
                <div className="bag-summary">
                  <div className="bag-divider" />
                  <div className="bag-total-row">
                    <span className="bag-total-label">TOTAL</span>
                    <span className="bag-total-value">{bagTotal.toFixed(2)} EUR</span>
                  </div>
                  <Link to="/checkout" className="bag-checkout-btn">PROCEED TO CHECKOUT</Link>
                </div>
              </>
            )}
          </>
        )}

        {tab === 'favorites' && (
          <>
            {wishlistItems.length === 0 ? (
              <p className="bag-empty">NO SAVED ITEMS</p>
            ) : (
              wishlistItems.map((item, i) => (
                <BagItem
                  key={`${item.wishlistItemId}-${i}`}
                  item={{ ...item, id: item.productId, size: '' }}
                  isFav={true}
                  onDelete={() => toggleWishlist({ id: item.productId, source: item.source ?? 'WOMAN' })}
                  onMoveToCart={() => moveWishlistToCart(item)}
                />
              ))
            )}
          </>
        )}
      </main>

      <BottomNav />
    </div>
  )
}

export default BagPage
