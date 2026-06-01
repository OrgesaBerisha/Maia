import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { CartProvider } from './CartContext.jsx';
import HomeScreen from './HomeScreen.jsx';
import ShopPage from './ShopPage.jsx';
import SearchPage from './SearchPage.jsx';
import BagPage from './BagPage.jsx';

function App() {
  return (
    <CartProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/"       element={<HomeScreen />} />
          <Route path="/shop"   element={<ShopPage />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/bag"    element={<BagPage />} />
        </Routes>
      </BrowserRouter>
    </CartProvider>
  );
}

export default App;