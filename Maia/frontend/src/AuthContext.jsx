import { createContext, useContext, useState, useCallback } from 'react'
import api from './api/axios.js'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    try { return JSON.parse(localStorage.getItem('maia_user') ?? 'null') }
    catch { return null }
  })

  const login = useCallback(async (email, password) => {
    const { data } = await api.post('/auth/login', { email, password })
    const userInfo = { email, isLoggedIn: data.isLoggedIn }
    setUser(userInfo)
    localStorage.setItem('maia_user', JSON.stringify(userInfo))
    return data
  }, [])

  const register = useCallback(async (firstName, lastName, email, password) => {
    const { data } = await api.post('/auth/register', { firstName, lastName, email, password })
    return data
  }, [])

  const logout = useCallback(async () => {
    try { await api.post('/auth/logout') } catch { /* continue */ }
    setUser(null)
    localStorage.removeItem('maia_user')
  }, [])

  return (
    <AuthContext.Provider value={{ user, isLoggedIn: !!user, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}
