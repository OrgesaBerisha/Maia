import { lazy, Suspense } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { CartProvider } from './CartContext.jsx';

const HomeScreen   = lazy(() => import('./HomeScreen.jsx'));
const ShopPage     = lazy(() => import('./ShopPage.jsx'));
const SearchPage   = lazy(() => import('./SearchPage.jsx'));
const BagPage      = lazy(() => import('./BagPage.jsx'));
const ProfilePage  = lazy(() => import('./ProfilePage.jsx'));
const CheckoutPage = lazy(() => import('./CheckoutPage.jsx'));

function App() {
  return (
    <CartProvider>
      <BrowserRouter>
        <Suspense fallback={null}>
          <Routes>
            <Route path="/"         element={<HomeScreen />} />
            <Route path="/shop"     element={<ShopPage />} />
            <Route path="/search"   element={<SearchPage />} />
            <Route path="/bag"      element={<BagPage />} />
            <Route path="/profile"  element={<ProfilePage />} />
            <Route path="/checkout" element={<CheckoutPage />} />
          </Routes>
        </Suspense>
      </BrowserRouter>
    </CartProvider>
  );
}

export default App;
