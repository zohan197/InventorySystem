<template>
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-4">
      <div class="flex items-center gap-2">
        <select v-if="isProductPage"
          v-model="localSelectedCategory"
          @change="updateSelectedCategory"
          class="px-3 py-2 border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-100 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="">All Categories</option>
          <option v-for="category in categories" :key="category.id" :value="String(category.id)">
            {{ category.name }}
          </option>
        </select>
        
  
        <input
          v-model="localSearchQuery"
          type="text"
          placeholder="Search..."
          class="px-3 py-2 border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-100 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>
  
      <button
        @click="$emit('add-clicked')"
        class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 text-sm rounded-md shadow transition"
      >
        + {{ buttonText }}
      </button>
    </div>
  </template>
  
  <script setup>
  import { ref, watch } from 'vue';

  const props = defineProps({
    categories: Array,
    selectedCategory: String,
    searchQuery: String,
    buttonText: {
      type: String,
      default: 'Add New',
    },
    isProductPage: {
      type: Boolean,
      default: false,
    },
  });

  const emit = defineEmits([
    'add-clicked',
    'update:selectedCategory',
    'update:searchQuery',
  ]);

  const localSelectedCategory = ref(props.selectedCategory);
  const localSearchQuery = ref(props.searchQuery);

  watch(localSelectedCategory, (newVal) => {
    emit('update:selectedCategory', newVal);
  });

  watch(localSearchQuery, (newVal) => {
    emit('update:searchQuery', newVal);
  });
</script>


  