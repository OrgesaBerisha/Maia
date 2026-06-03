import { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { orderApi } from './api/axios.js';

const CartContext = createContext(null);

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
  };
}

export function CartProvider({ children }) {
  const [bag,       setBag]       = useState([]);
  const [favorites, setFavorites] = useState(() => {
    try { return JSON.parse(localStorage.getItem('maia_favs') ?? '[]'); } catch { return []; }
  });

  const fetchCart = useCallback(async () => {
    try {
      const { data } = await orderApi.get('/Cart');
      setBag((data.items ?? []).map(mapItem));
    } catch {
      setBag([]);
    }
  }, []);

  useEffect(() => { fetchCart(); }, [fetchCart]);

  useEffect(() => {
    localStorage.setItem('maia_favs', JSON.stringify(favorites));
  }, [favorites]);

  const addToBag = async (item) => {
    try {
      await orderApi.post('/Cart', {
        productId:     item.id,
        productSource: item.productSource ?? 'women',
        productName:   item.name,
        imageUrl:      item.image ?? '',
        price:         parseFloat(item.price) || 0,
        quantity:      1,
      });
      await fetchCart();
    } catch {
      setBag(prev => prev.find(i => i.id === item.id) ? prev : [...prev, item]);
    }
  };

  const removeFromBag = async (id, size, cartItemId) => {
    const idToRemove = cartItemId ?? bag.find(i => i.id === id && i.size === size)?.cartItemId;
    if (idToRemove) {
      try { await orderApi.delete(`/Cart/${idToRemove}`); } catch { /* continue */ }
    }
    setBag(prev => prev.filter(i => !(i.id === id && i.size === size)));
  };

  const saveForLater = async (id, size, cartItemId) => {
    const item = bag.find(i => i.id === id && i.size === size);
    if (!item) return;
    await removeFromBag(id, size, cartItemId);
    setFavorites(prev => prev.find(i => i.id === id && i.size === size) ? prev : [...prev, item]);
  };

  const addToWishlist = async (item) => {
    setFavorites(prev => prev.find(i => i.id === item.id) ? prev : [...prev, item]);
  };

  const removeFromFavorites = async (id, size) => {
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
