import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
  withCredentials: true,
})

let isRefreshing = false
let failedQueue = []

const processQueue = (error) => {
  failedQueue.forEach(p => error ? p.reject(error) : p.resolve())
  failedQueue = []
}

api.interceptors.response.use(
  response => response,
  async error => {
    const original = error.config
    if (
      error.response?.status === 401 &&
      !original._retry &&
      !original.url?.includes('/auth/')
    ) {
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject })
        }).then(() => api(original)).catch(() => Promise.reject(error))
      }
      original._retry = true
      isRefreshing = true
      try {
        await api.post('/auth/refresh')
        processQueue(null)
        return api(original)
      } catch {
        processQueue(error)
        return Promise.reject(error)
      } finally {
        isRefreshing = false
      }
    }
    return Promise.reject(error)
  }
)

export default api
