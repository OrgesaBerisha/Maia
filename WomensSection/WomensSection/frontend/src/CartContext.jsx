import { createContext, useContext, useState, useEffect } from 'react';

const CartContext = createContext(null);

const DEMO_BAG = [
  { id: 101, name: 'BLACK TOP',  size: 'S',        price: '20 EUR', category: 'WOMAN', image: 'https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?w=300&q=80' },
  { id: 102, name: 'WHITE TOP',  size: 'XS',       price: '20 EUR', category: 'WOMAN', image: 'https://images.unsplash.com/photo-1578587018452-892bacefd3f2?w=300&q=80' },
  { id: 103, name: 'BROWN BAG',  size: 'ONE SIZE', price: '30 EUR', category: 'WOMAN', image: 'https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=300&q=80' },
];

function load(key, fallback) {
  try {
    const raw = localStorage.getItem(key);
    return raw ? JSON.parse(raw) : fallback;
  } catch {
    return fallback;
  }
}

export function CartProvider({ children }) {
  const [bag,       setBag]       = useState(() => load('maia_bag',  DEMO_BAG));
  const [favorites, setFavorites] = useState(() => load('maia_favs', []));

  useEffect(() => { localStorage.setItem('maia_bag',  JSON.stringify(bag)); },       [bag]);
  useEffect(() => { localStorage.setItem('maia_favs', JSON.stringify(favorites)); }, [favorites]);

  const addToBag = (item) => {
    setBag(prev => {
      const exists = prev.find(i => i.id === item.id && i.size === item.size);
      return exists ? prev : [...prev, item];
    });
  };

  const removeFromBag = (id, size) =>
    setBag(prev => prev.filter(i => !(i.id === id && i.size === size)));

  const saveForLater = (id, size) => {
    const item = bag.find(i => i.id === id && i.size === size);
    if (!item) return;
    removeFromBag(id, size);
    setFavorites(prev => prev.find(i => i.id === id && i.size === size) ? prev : [...prev, item]);
  };

  const removeFromFavorites = (id, size) =>
    setFavorites(prev => prev.filter(i => !(i.id === id && i.size === size)));

  const moveToCart = (id, size) => {
    const item = favorites.find(i => i.id === id && i.size === size);
    if (!item) return;
    removeFromFavorites(id, size);
    setBag(prev => prev.find(i => i.id === id && i.size === size) ? prev : [...prev, item]);
  };

  return (
    <CartContext.Provider value={{ bag, favorites, addToBag, removeFromBag, saveForLater, removeFromFavorites, moveToCart }}>
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  return useContext(CartContext);
}
