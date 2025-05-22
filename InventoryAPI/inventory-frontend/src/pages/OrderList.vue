<template>
    <MainLayout>
        <div class="container mx-auto p-4">
            <h1 class="text-2xl font-bold mb-4">Order List</h1>

            <HeaderControls
            :categories="categories"
            v-model:selectedCategory="orderStore.selectedOrder"
            v-model:searchQuery="orderStore.searchQuery"
            @add-clicked="openAddModal"
            :buttonText="'Create Order'"
            :isProductPage="false"
            />

            <OrderTable
            :orders="paginatedOrders"
            @delete-clicked="openDeleteModal"
            />

            <PaginationControls
            :start="start"
            :end="end"
            :total="totalOrders"
            :currentPage="currentPage"
            :totalPages="totalPages"
            @page-change="changePage"
            />

            <OrderFormModal
            v-if="showModal"
            :order="selectedOrder"
            @close="closeModal"
            @saved="onOrderSaved"
            />

            <MessageConfirm
            v-if="showConfirm"
            title="Delete Order"
            message="Are you sure you want to delete this order?"
            @cancel="closeConfirm"
            @confirm="deleteOrder"
            />
        </div>
    </MainLayout>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import MainLayout from '@/Layouts/MainLayout.vue';
import HeaderControls from '@/components/HeaderControls.vue';
import OrderTable from '@/pages/Order/OrderTable.vue';
import PaginationControls from '@/components/PaginationControls.vue';
import OrderFormModal from '@/pages/Order/OrderFormModal.vue';
import { useOrderStore } from '@/stores/orderStore';
import MessageConfirm from '@/components/MessageConfirm.vue';
import api from '@/axios'; // API service for fetching data

const currentPage = ref(0);
const pageSize = 10;

const orderStore = useOrderStore();
const orders = computed(() => orderStore.filteredOrders);
const totalOrders = computed(() => orderStore.totalCount);

onMounted(() => {
    orderStore.fetchOrders();
});

const paginatedOrders = computed(() => {
    const start = currentPage.value * pageSize;
    const end = start + pageSize;
    return orders.value.slice(start, end);
});

const showModal = ref(false);
const showConfirm = ref(false);
const selectedOrder = ref(null);

const totalPages = computed(() => Math.ceil(totalOrders.value / pageSize));

const start = computed(() => currentPage.value * pageSize + 1);
const end = computed(() => Math.min((currentPage.value + 1) * pageSize, totalOrders.value));

function changePage(page) {
    if (page >= 0 && page < totalPages.value) {
        currentPage.value = page;
    }
}

const openAddModal = () => {
    selectedOrder.value = null;
    showModal.value = true;
};
const openDeleteModal = (product) => {
  selectedOrder.value = product;
  showConfirm.value = true;
};
const closeModal = () => {
    showModal.value = false;
};
const closeConfirm = () => {
    showConfirm.value = false;
    selectedOrder.value = null;
};
const onOrderSaved = (savedOrder) => {
    orderStore.orders.unshift(savedOrder);
};

const deleteOrder = async () => {
    try {
        await api.delete(`/orders/${selectedOrder.value.id}`);
        orderStore.fetchOrders(currentPage.value, pageSize);
        closeConfirm();
    } catch (error) {
        console.error('Error deleting order:', error);
    }
};


</script>