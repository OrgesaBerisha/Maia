import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { CartProvider } from './CartContext.jsx';
import HomeScreen from './HomeScreen.jsx';
import ShopPage from './ShopPage.jsx';
import SearchPage from './SearchPage.jsx';
import BagPage from './BagPage.jsx';
import ProfilePage from './ProfilePage.jsx';
import CheckoutPage from './CheckoutPage.jsx';

function App() {
  return (
    <CartProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/"       element={<HomeScreen />} />
          <Route path="/shop"   element={<ShopPage />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/bag"     element={<BagPage />} />
          <Route path="/profile"  element={<ProfilePage />} />
          <Route path="/checkout" element={<CheckoutPage />} />
        </Routes>
      </BrowserRouter>
    </CartProvider>
  );
}

export default App;