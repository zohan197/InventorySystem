import { defineStore } from 'pinia'
import api from '@/axios'
import { useAuthStore } from '@/stores/authStore'
import router from '@/router'

export const useProductStore = defineStore('productStore', {
  state: () => ({
    products: [],
    selectedCategory: '',
    searchQuery: '',
  }),
  getters: {
    filteredProducts(state) {
      return (state.products ?? []).filter((p) => {
        const matchesCategory =
          !state.selectedCategory || state.selectedCategory === ''
            ? true
            : String(p.categoryId) === state.selectedCategory;

        const matchesSearch = p.name
          ?.toLowerCase()
          .includes(state.searchQuery.toLowerCase());
    
        return matchesCategory && matchesSearch;
      });
    },
    totalCount(state) {
      if (!state.selectedCategory || state.selectedCategory === '') {
        return (state.products ?? []).length;
      }
      return (state.products ?? []).filter(
        (p) => String(p.categoryId) === state.selectedCategory
      ).length;
    }
  },

  actions: {
    async fetchProducts() {
      try {
        const res = await api.get('/products')
        this.products = res.data ?? []
        console.log('Products fetched:', this.products)
      } catch (err) {
        if (err.response && err.response.status === 401) {
          console.warn('Unauthorized. Redirecting to login...');
          useAuthStore().logout(); // Clear auth store
          router.push('/login') // if using Vue Router
        } else {
          console.error('Failed to fetch products:', err);
        }
        this.products = []
      }
    },
  },
})
