<template>
  <div class="min-h-screen flex flex-col bg-gray-50 dark:bg-gray-900 text-gray-900 dark:text-gray-100 font-sans">
    <header class="h-16 z-30 fixed top-0 w-full">
      <TopNav />
    </header>

    <div class="flex flex-1 pt-16">
      <ResponsiveNav />

      <main
        :class="[
          'flex-1 p-6 transition-all duration-300 ease-in-out',
          ui.isSidebarOpen ? 'ml-64' : 'ml-16'
        ]"
      >
        <slot />
      </main>
    </div>
  </div>
</template>

<script setup>
import TopNav from '@/components/TopNav.vue'
import ResponsiveNav from '@/components/ResponsiveNav.vue'
import { useUIStore } from '@/stores/uiStore'

const ui = useUIStore()
</script>
