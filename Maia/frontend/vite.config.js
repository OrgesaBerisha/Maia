import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Service ports:
// Maia (main)       → 5293
// Auth              → 5000
// KidsSection       → 5062
// MenSection        → 5018
// WomensSection     → 5182
// NotificationSvc   → 5151
// FileUploadSvc     → 5270
// SettingsService   → 5187

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Auth service
      '/api/auth':  { target: 'http://localhost:5000', changeOrigin: true, secure: false },
      '/api/users': { target: 'http://localhost:5000', changeOrigin: true, secure: false },

      // Kids section
      '/api/KidsCards':       { target: 'http://localhost:5062', changeOrigin: true, secure: false },
      '/api/KidsCategory':    { target: 'http://localhost:5062', changeOrigin: true, secure: false },
      '/api/KidsProductType': { target: 'http://localhost:5062', changeOrigin: true, secure: false },

      // Men section
      '/api/MenCards':    { target: 'http://localhost:5018', changeOrigin: true, secure: false },
      '/api/MenCategory': { target: 'http://localhost:5018', changeOrigin: true, secure: false },

      // Women section
      '/api/CardsWomen':    { target: 'http://localhost:5182', changeOrigin: true, secure: false },
      '/api/WomanCategory': { target: 'http://localhost:5182', changeOrigin: true, secure: false },
      '/api/Wishlist':      { target: 'http://localhost:5182', changeOrigin: true, secure: false },

      // Order Service — centralized cart + orders
      '/api/Cart':  { target: 'http://localhost:5200', changeOrigin: true, secure: false },
      '/api/Order': { target: 'http://localhost:5200', changeOrigin: true, secure: false },

      // Notification service
      '/api/notifications': { target: 'http://localhost:5151', changeOrigin: true, secure: false },

      // File upload service
      '/api/files': { target: 'http://localhost:5270', changeOrigin: true, secure: false },

      // Settings service
      '/api/settings': { target: 'http://localhost:5187', changeOrigin: true, secure: false },

      // Payment
      '/api/payment': { target: 'http://localhost:5200', changeOrigin: true, secure: false },

      // Maia main backend — catch-all for any remaining /api routes
      '/api': { target: 'http://localhost:5293', changeOrigin: true, secure: false },
    },
  },
})
