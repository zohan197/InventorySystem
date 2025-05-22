import { defineStore } from 'pinia';
import api from '@/axios';
import { useAuthStore } from '@/stores/authStore';
import router from '@/router';

export const useCategoryStore = defineStore('categoryStore', {
    state: () => ({
        categories: [],
        selectedCategory: '',
        searchQuery: '',
    }),
    getters: {
        filteredCategories(state) {
        return (state.categories ?? []).filter((c) => {
            const matchesSearch = c.name
            ?.toLowerCase()
            .includes(state.searchQuery.toLowerCase());
            return matchesSearch;
        });
        },
        totalCount(state) {
        return (state.categories ?? []).length;
        },
    },
    
    actions: {
        async fetchCategories() {
        try {
            const res = await api.get('/categories');
            this.categories = res.data ?? [];
        } catch (err) {
            if (err.response && err.response.status === 401) {
            console.warn('Unauthorized. Redirecting to login...');
            useAuthStore().logout(); // Clear auth store
            router.push('/login'); // if using Vue Router
            } else {
            console.error('Failed to fetch categories:', err);
            }
            this.categories = [];
        }
        },
    },
    });