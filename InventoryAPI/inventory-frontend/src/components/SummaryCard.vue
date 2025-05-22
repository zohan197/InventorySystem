<template>
    <div class="bg-white dark:bg-gray-800 p-4 rounded-2xl shadow-md flex items-center justify-between">
      <div>
        <h4 class="text-sm text-gray-500 dark:text-gray-400">{{ title }}</h4>
        <p class="text-2xl font-bold text-gray-800 dark:text-white">{{ value }}</p>
      </div>
      <slot />
    </div>
  </template>
  
  <script setup>
  defineProps({
    title: String,
    value: [String, Number]
  })
  </script>
  