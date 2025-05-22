<template>
    <div class="flex items-center justify-between mt-4">
      <span class="text-sm text-gray-500 dark:text-gray-300">
        Showing {{ start + 1 }} to {{ end }} of {{ total }} items
      </span>
      <div class="flex space-x-2">
        <button
          @click="$emit('page-change', currentPage - 1)"
          :disabled="currentPage === 0"
          class="px-3 py-1 bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-100 rounded-md disabled:bg-gray-400 text-sm"
        >
          Prev
        </button>
        <button
          @click="$emit('page-change', currentPage + 1)"
          :disabled="currentPage >= totalPages - 1"
          class="px-3 py-1 bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-100 rounded-md disabled:bg-gray-400 text-sm"
        >
          Next
        </button>
      </div>
    </div>
  </template>
  
  <script setup>
  defineProps(['start', 'end', 'total', 'currentPage', 'totalPages']);
  defineEmits(['page-change']);
  </script>
  