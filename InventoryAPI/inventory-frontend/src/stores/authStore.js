import { defineStore } from 'pinia'
import api from '@/axios'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    error: '',
  }),
  actions: {
    async login(username, password) {
      try {
        await api.post('/auth/login', {
          username,
          password,
        })
        await this.isAuthenticated()
        this.error = ''
      } catch (err) {
        this.error = err.response?.data || 'Login failed'
        console.error(err)
      }
    },
    async register(username, password) {
      try {
        await api.post('/auth/register', {
          username,
          password,
        })
        this.error = ''
        return true
      } catch (err) {
        this.error = err.response?.data || 'Registration failed'
        console.error(err)
        return false
      }
    },
    async logout() {
      try {
        await api.post('/auth/logout')
        this.user = null
      } catch (err) {
        console.error('Logout failed:', err)
      }
    },
    async isAuthenticated() {
      try {
        const response = await api.get('/auth/me')
        this.user = response.data.username // or whatever structure you return
        console.log('User:', this.user)
        return true
      } catch {
        this.user = null
        return false
      }
    }
  },
  persist: {
    paths: ['user'],
    storage: localStorage,
  },
})
