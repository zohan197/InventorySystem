<template>
  <MainLayout>
  <div class="container mx-auto p-4">
    <h1 class="text-2xl font-bold mb-4">Product List</h1>
    <HeaderControls
      :categories="categories"
      v-model:selectedCategory="productStore.selectedCategory"
      v-model:searchQuery="productStore.searchQuery"
      @add-clicked="openAddModal"
      :buttonText="'Add Product'"
      :isProductPage="true"
    />
    
    <ProductTable
      :products="paginatedProducts"
      @edit-clicked="openEditModal"
      @delete-clicked="openDeleteModal"
    />
    
    <PaginationControls
      :start="start"
      :end="end"
      :total="totalProducts"
      :currentPage="currentPage"
      :totalPages="totalPages"
      @page-change="changePage"
    />
    <!-- Product Form Modal -->
    <ProductFormModal 
    v-if="showModal" 
    :product="selectedProduct" 
    :categories="categories" 
    @close="closeModal" 
    @saved="onProductSaved" 
    />
    <!-- Confirmation Modal -->
    <MessageConfirmation
    v-if="showConfirm"
    title="Delete Product"
    message="Are you sure you want to delete this product?"
    @cancel="closeConfirm"
    @confirm="deleteProduct"
  />

  </div>
  </MainLayout>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import HeaderControls from '@/components/HeaderControls.vue';
import ProductTable from '@/pages/Product/ProductTable.vue'; // Table component for displaying products
import PaginationControls from '@/components/PaginationControls.vue';
import { useProductStore } from '@/stores/productStore'; // assuming Pinia store for state management
import ProductFormModal from '@/pages/Product/ProductFormModal.vue'; // Modal for adding/editing products
import MainLayout from '@/Layouts/MainLayout.vue';
import MessageConfirmation from '@/components/MessageConfirm.vue'; // Confirmation modal for delete action
import api from '@/axios'; // API service for fetching data

// Define Props
const categories = ref([]); // Replace with actual categories
const currentPage = ref(0);
const pageSize = 10; // Adjust based on your desired page size

// Pinia store for handling products
const productStore = useProductStore();

// Get filtered and paginated products from the store
const products = computed(() => productStore.filteredProducts);
const totalProducts = computed(() => productStore.totalCount);

onMounted(() => {
  fetchCategories();
  productStore.fetchProducts(); // Fetch products when the component is mounted
});

// Get paginated products
const paginatedProducts = computed(() => {
  const start = currentPage.value * pageSize;
  const end = start + pageSize;
  return products.value.slice(start, end);
});

//Modal
const showModal = ref(false);
const showConfirm = ref(false);
const selectedProduct = ref(null);

const totalPages = computed(() => Math.ceil(totalProducts.value / pageSize));

// Pagination controls
const start = computed(() => currentPage.value * pageSize + 1);
const end = computed(() => Math.min((currentPage.value + 1) * pageSize, totalProducts.value));

function changePage(page) {
  if (page >= 0 && page < totalPages.value) {
    currentPage.value = page;
  }
}

const openAddModal = (product) => {
    selectedProduct.value = product;
    showModal.value = true;
};

const openEditModal = (product) => {
  selectedProduct.value = product;
  showModal.value = true;
};

const openDeleteModal = (product) => {
  selectedProduct.value = product;
  showConfirm.value = true;
};

const closeModal = () => {
  showModal.value = false;
  selectedProduct.value = null;
};

const closeConfirm = () => {
  showConfirm.value = false;
  selectedProduct.value = null;
};

const fetchCategories = async () => {
  try {
    const { data } = await api.get('/categories');
    categories.value = data;
  } catch (error) {
    console.error("Error fetching categories:", error);
  }
};

// Update the product store when a product is saved
const onProductSaved = (savedProduct) => {
  const existingIndex = productStore.products.findIndex(p => p.id === savedProduct.id);
  if (existingIndex !== -1) {
    // ✅ UPDATE existing product in store (reactive)
    productStore.products.splice(existingIndex, 1, savedProduct);
  } else {
    // ✅ ADD new product to the end (reactive)
    productStore.products.push(savedProduct);
  }
};

// Delete product from the store and API
const deleteProduct = async () => {
  try {
    await api.delete(`/products/${selectedProduct.value.id}`);
    productStore.products = productStore.products.filter(p => p.id !== selectedProduct.value.id);
    closeConfirm();
  } catch (error) {
    console.error('Failed to delete product:', error);
  }
};

</script>
