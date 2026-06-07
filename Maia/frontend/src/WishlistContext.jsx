import { createContext, useContext, useState, useEffect, useCallback } from 'react'
import api from './api/axios.js'
import { useAuth } from './AuthContext.jsx'

const WishlistContext = createContext(null)

export function WishlistProvider({ children }) {
  const { isLoggedIn } = useAuth()
  const [items, setItems] = useState([])

  const fetchWishlist = useCallback(async () => {
    if (!isLoggedIn) return
    try {
      const { data } = await api.get('/Wishlist')
      const list = data.Items ?? data.items ?? []
      setItems(list.map(i => ({
        wishlistItemId: i.Id ?? i.id,
        productId:      i.ProductId ?? i.productId,
      })))
    } catch {
      setItems([])
    }
  }, [isLoggedIn])

  useEffect(() => {
    if (isLoggedIn) fetchWishlist()
    else setItems([])
  }, [isLoggedIn, fetchWishlist])

  const isWishlisted = useCallback(
    (productId) => items.some(i => i.productId === productId),
    [items]
  )

  const toggleWishlist = useCallback(async (product) => {
    if (!isLoggedIn) return
    const existing = items.find(i => i.productId === product.id)
    if (existing) {
      try {
        await api.delete(`/Wishlist/${existing.wishlistItemId}`)
        setItems(prev => prev.filter(i => i.productId !== product.id))
      } catch {}
    } else {
      try {
        await api.post('/Wishlist', { productId: product.id })
        await fetchWishlist()
      } catch {}
    }
  }, [isLoggedIn, items, fetchWishlist])

  return (
    <WishlistContext.Provider value={{ items, isWishlisted, toggleWishlist }}>
      {children}
    </WishlistContext.Provider>
  )
}

export function useWishlist() {
  return useContext(WishlistContext)
}
