import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomeScreen from './HomeScreen.jsx';
import ShopPage from './ShopPage.jsx';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomeScreen />} />
        <Route path="/shop" element={<ShopPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;