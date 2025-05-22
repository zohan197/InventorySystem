import { defineStore } from 'pinia';
import api from '@/axios';
import { useAuthStore } from '@/stores/authStore';
import router from '@/router';

export const useOrderStore = defineStore('orderStore', {
    state: () => ({
        orders: [],
        selectedOrder: '',
        searchQuery: '',
    }),
    getters: {
        filteredOrders(state) {
            return (state.orders ?? []).filter((o) => {
                const matchesSearch = o.customerName
                    ?.toLowerCase()
                    .includes(state.searchQuery.toLowerCase());
                return matchesSearch;
            });
        },
        totalCount(state) {
            return (state.orders ?? []).length;
        },
    },

    actions: {
        async fetchOrders() {
            try {
                const res = await api.get('/orders');
                this.orders = res.data ?? [];
            } catch (err) {
                if (err.response && err.response.status === 401) {
                    console.warn('Unauthorized. Redirecting to login...');
                    useAuthStore().logout(); // Clear auth store
                    router.push('/login'); // if using Vue Router
                } else {
                    console.error('Failed to fetch orders:', err);
                }
                this.orders = [];
            }
        },
    },
});