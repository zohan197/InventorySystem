<template>
    <div class="fixed inset-0 bg-black/40 backdrop-blur-sm flex items-center justify-center z-50">
      <div class="bg-white dark:bg-gray-800 rounded-xl shadow-xl max-w-md w-full p-6">
        <h2 class="text-lg font-semibold text-gray-800 dark:text-white mb-4">
          {{ title || 'Are you sure?' }}
        </h2>
  
        <p class="text-sm text-gray-600 dark:text-gray-300 mb-6">
          {{ message || 'This action cannot be undone. Do you want to proceed?' }}
        </p>
  
        <div class="flex justify-end space-x-3">
          <button
            @click="$emit('cancel')"
            class="px-4 py-2 text-gray-700 dark:text-gray-300 border border-gray-300 dark:border-gray-600 rounded hover:bg-gray-100 dark:hover:bg-gray-700 transition"
          >
            Cancel
          </button>
          <button
            @click="$emit('confirm')"
            class="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700 transition"
          >
            Confirm
          </button>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>

  
  defineProps({
    title: String,
    message: String
  })
  </script>
  