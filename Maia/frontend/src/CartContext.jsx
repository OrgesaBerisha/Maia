import { createContext, useContext, useState, useEffect, useCallback } from 'react'
import api from './api/axios.js'

const CartContext = createContext(null)

function mapItem(i) {
  return {
    cartItemId:    i.id,
    id:            i.productId,
    productSource: i.productSource ?? 'women',
    name:          i.productName?.toUpperCase() ?? '',
    price:         i.price ?? 0,
    image:         i.imageUrl ?? '',
    quantity:      i.quantity,
    size:          i.size ?? 'ONE SIZE',
  }
}

export function CartProvider({ children }) {
  const [bag, setBag] = useState(() => {
    try { return JSON.parse(localStorage.getItem('maia_bag') ?? '[]') } catch { return [] }
  })
  const [favorites, setFavorites] = useState(() => {
    try { return JSON.parse(localStorage.getItem('maia_favs') ?? '[]') } catch { return [] }
  })

  const fetchCart = useCallback(async () => {
    try {
      const { data } = await api.get('/Cart')
      const items = (data.items ?? []).map(mapItem)
      if (items.length > 0) {
        setBag(items)
        localStorage.setItem('maia_bag', JSON.stringify(items))
      } else {
        // backend cart is empty — sync from localStorage
        const saved = JSON.parse(localStorage.getItem('maia_bag') ?? '[]')
        if (saved.length > 0) {
          for (const item of saved) {
            try {
              await api.post('/Cart', {
                productId:     item.id,
                productSource: item.productSource ?? 'women',
                productName:   item.name,
                imageUrl:      item.image ?? '',
                price:         parseFloat(item.price) || 0,
                quantity:      item.quantity ?? 1,
              })
            } catch { /* continue */ }
          }
          try {
            const { data: synced } = await api.get('/Cart')
            const syncedItems = (synced.items ?? []).map(mapItem)
            if (syncedItems.length > 0) {
              setBag(syncedItems)
              localStorage.setItem('maia_bag', JSON.stringify(syncedItems))
            }
            // if sync failed, keep localStorage items already in state
          } catch { /* keep localStorage items */ }
        }
      }
    } catch {
      // keep existing bag from localStorage
    }
  }, [])

  useEffect(() => { fetchCart() }, [fetchCart])

  useEffect(() => {
    localStorage.setItem('maia_favs', JSON.stringify(favorites))
  }, [favorites])

  const addToBag = async (item) => {
    try {
      await api.post('/Cart', {
        productId:     item.id,
        productSource: item.productSource ?? 'women',
        productName:   item.name,
        imageUrl:      item.image ?? '',
        price:         parseFloat(item.price) || 0,
        quantity:      1,
      })
      await fetchCart()
    } catch {
      setBag(prev => {
        const next = prev.find(i => i.id === item.id) ? prev : [...prev, item]
        localStorage.setItem('maia_bag', JSON.stringify(next))
        return next
      })
    }
  }

  const removeFromBag = async (id, size, cartItemId) => {
    const idToRemove = cartItemId ?? bag.find(i => i.id === id && i.size === size)?.cartItemId
    if (idToRemove) {
      try { await api.delete(`/Cart/${idToRemove}`) } catch { /* continue */ }
    }
    setBag(prev => {
      const next = prev.filter(i => !(i.id === id && i.size === size))
      localStorage.setItem('maia_bag', JSON.stringify(next))
      return next
    })
  }

  const saveForLater = async (id, size, cartItemId) => {
    const item = bag.find(i => i.id === id && i.size === size)
    if (!item) return
    await removeFromBag(id, size, cartItemId)
    setFavorites(prev => prev.find(i => i.id === id && i.size === size) ? prev : [...prev, item])
  }

  const removeFromFavorites = async (id, size) => {
    setFavorites(prev => prev.filter(i => !(i.id === id && i.size === size)))
  }

  const moveToCart = async (id, size) => {
    const item = favorites.find(i => i.id === id && i.size === size)
    if (!item) return
    await removeFromFavorites(id, size)
    await addToBag(item)
  }

  return (
    <CartContext.Provider value={{
      bag, favorites,
      addToBag, removeFromBag,
      saveForLater, removeFromFavorites, moveToCart,
      fetchCart,
    }}>
      {children}
    </CartContext.Provider>
  )
}

export function useCart() {
  return useContext(CartContext)
}
