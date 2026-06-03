import { lazy, Suspense } from 'react'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { CartProvider } from './CartContext.jsx'
import { AuthProvider } from './AuthContext.jsx'
import { NotificationProvider } from './NotificationContext.jsx'

const HomeScreen   = lazy(() => import('./HomeScreen.jsx'))
const ShopPage     = lazy(() => import('./ShopPage.jsx'))
const SearchPage   = lazy(() => import('./SearchPage.jsx'))
const BagPage      = lazy(() => import('./BagPage.jsx'))
const ProfilePage  = lazy(() => import('./ProfilePage.jsx'))
const CheckoutPage = lazy(() => import('./CheckoutPage.jsx'))
const LoginPage           = lazy(() => import('./LoginPage.jsx'))
const RegisterPage        = lazy(() => import('./RegisterPage.jsx'))
const ForgotPasswordPage  = lazy(() => import('./ForgotPasswordPage.jsx'))
const ResetPasswordPage   = lazy(() => import('./ResetPasswordPage.jsx'))

function App() {
  return (
    <AuthProvider>
      <NotificationProvider>
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
                <Route path="/login"           element={<LoginPage />} />
                <Route path="/register"        element={<RegisterPage />} />
                <Route path="/forgot-password" element={<ForgotPasswordPage />} />
                <Route path="/reset-password"  element={<ResetPasswordPage />} />
              </Routes>
            </Suspense>
          </BrowserRouter>
        </CartProvider>
      </NotificationProvider>
    </AuthProvider>
  )
}

export default App
