<template>
    <MainLayout>
        <div class="container mx-auto p-4">
            <h1 class="text-2xl font-bold mb-4">Category List</h1>
            <HeaderControls
                :categories="categories"
                v-model:searchQuery="categoryStore.searchQuery"
                @add-clicked="openAddModal"
                :buttonText="'Add Category'"
                :isProductPage="false"
            />

            <CategoryTable
                :categories="paginatedCategories"
                :currentPage="currentPage"
                :itemsPerPage="pageSize"
                @edit-clicked="openEditModal"
                @delete-clicked="openDeleteModal"
            />

            <PaginationControls
                :start="start"
                :end="end"
                :total="totalCategories"
                :currentPage="currentPage"
                :totalPages="totalPages"
                @page-change="changePage"
            />
            <!-- Category Form Modal -->
            <CategoryFormModal 
            v-if="showModal" 
            :category="selectedCategory" 
            @close="closeModal" 
            @saved="onCategorySaved" 
            />
            <!-- Confirmation Modal -->
            <MessageConfirmation
            v-if="showConfirm"
            title="Delete Category"
            message="Are you sure you want to delete this category?"
            @cancel="closeConfirm"
            @confirm="deleteCategory"
            />
        </div>
    </MainLayout>
</template>
<script setup>
import MainLayout from '@/Layouts/MainLayout.vue';
import HeaderControls from '@/components/HeaderControls.vue';
import CategoryTable from '@/pages/Category/CategoryTable.vue';
import PaginationControls from '@/components/PaginationControls.vue';
import CategoryFormModal from '@/pages/Category/CategoryFormModal.vue'; // Modal for adding/editing categories
import MessageConfirmation from '@/components/MessageConfirm.vue'; // Confirmation modal for delete action
import { ref, computed, onMounted } from 'vue';
import { useCategoryStore } from '@/stores/categoryStore'; // assuming Pinia store for state management
import api from '@/axios'; // API service for fetching data
import { useToast } from 'vue-toastification'

const toast = useToast()

const currentPage = ref(0);
const pageSize = 10; // Adjust based on your desired page size

const categoryStore = useCategoryStore();

const categories = computed(() => categoryStore.categories);

onMounted(() => {
    categoryStore.fetchCategories();
});

const paginatedCategories = computed(() => {
    const start = currentPage.value * pageSize;
    return categories.value.slice(start, start + pageSize);
});
const totalCategories = computed(() => categories.value.length);

const start = computed(() => currentPage.value * pageSize + 1);
const end = computed(() => Math.min(start.value + pageSize - 1, totalCategories.value));

const totalPages = computed(() => Math.ceil(totalCategories.value / pageSize));

const changePage = (page) => {
    currentPage.value = page;
};

//Modal
const showModal = ref(false);
const showConfirm = ref(false);
const selectedCategory = ref(null);

const openAddModal = () => {
    selectedCategory.value = null;
    showModal.value = true;
};
const openEditModal = (category) => {
    selectedCategory.value = category;
    showModal.value = true;
};
const openDeleteModal = (category) => {
    selectedCategory.value = category;
    showConfirm.value = true;
};
const closeConfirm = () => {
  showConfirm.value = false;
  selectedCategory.value = null;
};

        
const closeModal = () => {
    showModal.value = false;
};
const onCategorySaved = () => {
    closeModal();
    categoryStore.fetchCategories(); // Refresh categories after saving
};
const deleteCategory = async () => {
  try {
    await api.delete(`/categories/${selectedCategory.value.id}`);
    categoryStore.categories = categoryStore.categories.filter(p => p.id !== selectedCategory.value.id);
    closeConfirm();
    } catch (error) {
        toast.error('Failed to delete product:', error);
        closeConfirm();
  }
};
</script>