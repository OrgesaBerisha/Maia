import axios from 'axios'

// All requests go through the Vite proxy — no hardcoded ports needed.
// The proxy in vite.config.js routes each /api/<path> to the right microservice.
const api = axios.create({
  baseURL: '/api',
  withCredentials: true,
})

export default api
