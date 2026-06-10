import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

const e = process.env
const auth     = e.SVC_AUTH     || 'http://localhost:5000'
const womens   = e.SVC_WOMENS   || 'http://localhost:5182'
const mens     = e.SVC_MENS     || 'http://localhost:5018'
const kids     = e.SVC_KIDS     || 'http://localhost:5062'
const orders   = e.SVC_ORDERS   || 'http://localhost:5200'
const notifs   = e.SVC_NOTIFS   || 'http://localhost:5151'
const files    = e.SVC_FILES    || 'http://localhost:5270'
const settings = e.SVC_SETTINGS || 'http://localhost:5187'
const maia     = e.SVC_MAIA     || 'http://localhost:5293'

const p = (target, ws = false) => ({ target, changeOrigin: true, secure: false, ws })

export default defineConfig({
  plugins: [react()],
  server: {
    host: true,
    proxy: {
      '/api/auth':  p(auth),
      '/api/users': p(auth),

      '/api/KidsCards':       p(kids),
      '/api/KidsCategory':    p(kids),
      '/api/KidsProductType': p(kids),
      '/api/imageproxy':      p(kids),

      '/api/MenCards':    p(mens),
      '/api/MenCategory': p(mens),

      '/api/CardsWomen':            p(womens),
      '/api/WomanCategory':         p(womens),
      '/api/Wishlist':              p(womens),
      '/api/reviews':               p(womens),
      '/api/export/women-products': p(womens),
      '/api/export/men-products':   p(mens),
      '/api/export/kids-products':  p(kids),

      '/api/Cart':    p(orders),
      '/api/Order':   p(orders),
      '/api/payment': p(orders),

      '/api/notifications': p(notifs),
      '/api/chat':          p(notifs),

      '/hubs/notifications': p(notifs, true),
      '/hubs/chat':          p(notifs, true),

      '/api/files':    p(files),
      '/api/settings': p(settings),

      '/api': p(maia),
    },
  },
})
