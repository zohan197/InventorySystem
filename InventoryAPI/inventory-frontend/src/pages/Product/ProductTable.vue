<template>
    <div class="overflow-x-auto">
      <table class="min-w-full divide-y divide-gray-200 dark:divide-gray-700 text-sm">
        <thead class="bg-gradient-to-r from-blue-100 to-blue-200 dark:from-gray-700 dark:to-gray-800 text-gray-700 dark:text-gray-200 uppercase text-xs font-semibold tracking-wide">
          <tr>
            <th class="table-header">Name</th>
            <th class="table-header">Category</th>
            <th class="table-header">Stock</th>
            <th class="table-header">Price</th>
            <th class="table-header">Status</th>
            <th class="table-header text-center">Actions</th>
          </tr>
        </thead>
        <tbody v-if="products.length > 0" class="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
          <tr v-for="product in products" :key="product.id" class="hover:bg-gray-50 dark:hover:bg-gray-700 transition">
            <td class="px-6 py-4 font-medium text-gray-900 dark:text-white">{{ product.name }}</td>
            <td class="px-6 py-4 text-gray-600 dark:text-gray-300">{{ product.categoryName || 'Uncategorized' }}</td>
            <td class="px-6 py-4">{{ product.stock }}</td>
            <td class="px-6 py-4">{{ formatCurrency(product.unitPrice) }}</td>
            <td class="px-6 py-4">
              <span
                :class="product.isActive ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200' : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200'"
                class="px-2 py-1 rounded text-xs font-semibold"
              >
                {{ product.isActive ? 'Active' : 'Inactive' }}
              </span>
            </td>
            <td class="px-6 py-4 text-center">
              <button @click="$emit('edit-clicked', product)" class="text-blue-600 hover:text-blue-900 dark:text-blue-400 text-sm">
                Edit
              </button>&nbsp;
              <button @click="$emit('delete-clicked', product)" class="text-red-600 hover:text-red-900 dark:text-red-400 text-sm">
                Delete
              </button>
            </td>
          </tr>
        </tbody>
        <tbody v-else>
          <tr>
            <td colspan="6" class="text-center px-6 py-4 text-gray-500 dark:text-gray-300">
              No products found.
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>
  
  <script setup>
    defineProps(['products']);
    defineEmits(['edit-clicked']);
    const formatCurrency = (value) => {
      if (value === null || value === undefined) return '$0.00'; // Handle edge cases

      // Convert to number and format as currency
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
      }).format(value);
    };
  </script>
  