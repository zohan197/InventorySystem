<template>
    <div class="overflow-x-auto">
      <table class="min-w-full divide-y divide-gray-200 dark:divide-gray-700 text-sm">
        <thead class="bg-gradient-to-r from-blue-100 to-blue-200 dark:from-gray-700 dark:to-gray-800 text-gray-700 dark:text-gray-200 uppercase text-xs font-semibold tracking-wide">
          <tr>
            <th class="table-header">#</th>
            <th class="table-header">Category</th>
            <th class="table-header text-center">Actions</th>
          </tr>
        </thead>
        <tbody v-if="categories.length > 0" class="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
          <tr v-for="(category, index) in categories" :key="category.id" class="hover:bg-gray-50 dark:hover:bg-gray-700 transition">
            <td class="px-6 py-4 font-medium text-gray-900 dark:text-white">{{ (currentPage * itemsPerPage) + index + 1 }}</td>
            <td class="px-6 py-4 font-medium text-gray-900 dark:text-white">{{ category.name }}</td>
            <td class="px-6 py-4 text-center">
              <button @click="$emit('edit-clicked', category)" class="text-blue-600 hover:text-blue-900 dark:text-blue-400 text-sm">
                Edit
              </button>&nbsp;
              <button @click="$emit('delete-clicked', category)" class="text-red-600 hover:text-red-900 dark:text-red-400 text-sm">
                Delete
              </button>
            </td>
          </tr>
        </tbody>
        <tbody v-else>
          <tr>
            <td colspan="4" class="text-center px-6 py-4 text-gray-500 dark:text-gray-300">
              No category found.
            </td>
          </tr>
        </tbody>
      </table>
    </div>
</template>

<script setup>
  defineProps(['categories', 'currentPage', 'itemsPerPage']);
  defineEmits(['edit-clicked']);
</script>