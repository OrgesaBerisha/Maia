import { createContext, useContext, useState, useEffect, useCallback } from 'react';
import api from './api/axios.js';

const CartContext = createContext(null);

function mapItem(i) {
  return {
    cartItemId: i.id,
    id:         i.productId,
    name:       i.productName?.toUpperCase() ?? '',
    price:      `${i.price} EUR`,
    image:      i.productImage ?? '',
    quantity:   i.quantity,
    size:       i.size ?? 'ONE SIZE',
  };
}

export function CartProvider({ children }) {
  const [bag,       setBag]       = useState([]);
  const [favorites, setFavorites] = useState(() => {
    try { return JSON.parse(localStorage.getItem('maia_favs') ?? '[]'); } catch { return []; }
  });

  /* ── Fetch cart from backend ── */
  const fetchCart = useCallback(async () => {
    try {
      const { data } = await api.get('/Cart');
      setBag((data.items ?? []).map(mapItem));
    } catch {
      setBag([]);
    }
  }, []);

  useEffect(() => { fetchCart(); }, [fetchCart]);

  /* persist favorites locally */
  useEffect(() => {
    localStorage.setItem('maia_favs', JSON.stringify(favorites));
  }, [favorites]);

  /* ── Cart API actions ── */
  const addToBag = async (item) => {
    try {
      await api.post('/Cart', { productId: item.id, quantity: 1 });
      await fetchCart();
    } catch {
      /* fallback: add locally so UI doesn't break */
      setBag(prev => prev.find(i => i.id === item.id) ? prev : [...prev, item]);
    }
  };

  const removeFromBag = async (id, size, cartItemId) => {
    const idToRemove = cartItemId ?? bag.find(i => i.id === id && i.size === size)?.cartItemId;
    if (idToRemove) {
      try {
        await api.delete(`/Cart/${idToRemove}`);
      } catch { /* continue */ }
    }
    setBag(prev => prev.filter(i => !(i.id === id && i.size === size)));
  };

  const saveForLater = async (id, size, cartItemId) => {
    const item = bag.find(i => i.id === id && i.size === size);
    if (!item) return;
    await removeFromBag(id, size, cartItemId);
    setFavorites(prev => prev.find(i => i.id === id && i.size === size) ? prev : [...prev, item]);
  };

  /* ── Wishlist API actions ── */
  const addToWishlist = async (item) => {
    try {
      await api.post('/Wishlist', { productId: item.id });
    } catch { /* continue */ }
    setFavorites(prev => prev.find(i => i.id === item.id) ? prev : [...prev, item]);
  };

  const removeFromFavorites = async (id, size) => {
    const item = favorites.find(i => i.id === id && i.size === size);
    if (item?.wishlistItemId) {
      try { await api.delete(`/Wishlist/${item.wishlistItemId}`); } catch { /* continue */ }
    }
    setFavorites(prev => prev.filter(i => !(i.id === id && i.size === size)));
  };

  const moveToCart = async (id, size) => {
    const item = favorites.find(i => i.id === id && i.size === size);
    if (!item) return;
    await removeFromFavorites(id, size);
    await addToBag(item);
  };

  return (
    <CartContext.Provider value={{
      bag, favorites,
      addToBag, removeFromBag,
      saveForLater, addToWishlist,
      removeFromFavorites, moveToCart,
      fetchCart,
    }}>
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  return useContext(CartContext);
}
