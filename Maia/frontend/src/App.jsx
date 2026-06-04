import { lazy, Suspense } from 'react'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { CartProvider } from './CartContext.jsx'
import { AuthProvider } from './AuthContext.jsx'
import { NotificationProvider } from './NotificationContext.jsx'
import ProtectedRoute from './ProtectedRoute.jsx'

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

const PurchasesPage      = lazy(() => import('./PurchasesPage.jsx'))
const ContactDataPage    = lazy(() => import('./ContactDataPage.jsx'))
const StoresPage         = lazy(() => import('./StoresPage.jsx'))
const NotificationsPage  = lazy(() => import('./NotificationsPage.jsx'))

const AdminDashboard        = lazy(() => import('./dashboards/AdminDashboard.jsx'))
const SalesManagerDashboard = lazy(() => import('./dashboards/SalesManagerDashboard.jsx'))
const WomenManagerDashboard = lazy(() => import('./dashboards/WomenManagerDashboard.jsx'))
const MenManagerDashboard   = lazy(() => import('./dashboards/MenManagerDashboard.jsx'))
const KidsManagerDashboard  = lazy(() => import('./dashboards/KidsManagerDashboard.jsx'))

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
                <Route path="/profile"       element={<ProfilePage />} />
                <Route path="/purchases"    element={<PurchasesPage />} />
                <Route path="/contact"      element={<ContactDataPage />} />
                <Route path="/stores"       element={<StoresPage />} />
                <Route path="/notifications" element={<NotificationsPage />} />
                <Route path="/checkout" element={<CheckoutPage />} />
                <Route path="/login"           element={<LoginPage />} />
                <Route path="/register"        element={<RegisterPage />} />
                <Route path="/forgot-password" element={<ForgotPasswordPage />} />
                <Route path="/reset-password"  element={<ResetPasswordPage />} />

                <Route path="/dashboard/admin" element={
                  <ProtectedRoute allowedRoles={['Admin']}>
                    <AdminDashboard />
                  </ProtectedRoute>
                } />
                <Route path="/dashboard/sales" element={
                  <ProtectedRoute allowedRoles={['SalesManager']}>
                    <SalesManagerDashboard />
                  </ProtectedRoute>
                } />
                <Route path="/dashboard/women" element={
                  <ProtectedRoute allowedRoles={['WomenManager']}>
                    <WomenManagerDashboard />
                  </ProtectedRoute>
                } />
                <Route path="/dashboard/men" element={
                  <ProtectedRoute allowedRoles={['MenManager']}>
                    <MenManagerDashboard />
                  </ProtectedRoute>
                } />
                <Route path="/dashboard/kids" element={
                  <ProtectedRoute allowedRoles={['KidsManager']}>
                    <KidsManagerDashboard />
                  </ProtectedRoute>
                } />
              </Routes>
            </Suspense>
          </BrowserRouter>
        </CartProvider>
      </NotificationProvider>
    </AuthProvider>
  )
}

export default App
