<template>
  <div class="flex flex-col h-full">
    <!-- Sidebar Header -->
    <!-- <div
      class="flex items-center h-16 px-4 border-b border-gray-200 dark:border-gray-700"
      :class="ui.isSidebarOpen ? 'justify-between' : 'justify-center'"
    >
      <span v-if="ui.isSidebarOpen" class="text-lg font-bold text-blue-600 dark:text-blue-400">IMS</span>
      <button @click="ui.toggleSidebar" class="text-gray-600 dark:text-gray-300 cursor-pointer">
        ☰
      </button>
    </div> -->

    <!-- Sidebar Header without the hamburger -->
  <div
    class="flex items-center h-16 px-4 border-b border-gray-200 dark:border-gray-700"
    :class="ui.isSidebarOpen ? 'justify-start' : 'justify-center'"
  >
    <span v-if="ui.isSidebarOpen" class="text-lg font-bold text-blue-600 dark:text-blue-400">IMS</span>
  </div>


    <!-- Sidebar Navigation -->
    <nav class="flex-1 mt-4 space-y-2 px-2">
      <RouterLink
        to="/"
        @click="ui.isMobileSidebarOpen && ui.closeSidebar()"
        :class="[
          'sidenav-links',
          ui.isSidebarOpen ? 'flex items-center gap-3' : 'flex justify-center'
        ]"
      >
        <LayoutDashboard class="w-5 h-5" />
        <span v-if="ui.isSidebarOpen">Dashboard</span>
      </RouterLink>

      <RouterLink
        to="/products"
        @click="ui.isMobileSidebarOpen && ui.closeSidebar()"
        :class="[
          'sidenav-links',
          ui.isSidebarOpen ? 'flex items-center gap-3' : 'flex justify-center'
        ]"
      >
        <Boxes class="w-5 h-5" />
        <span v-if="ui.isSidebarOpen">Products</span>
      </RouterLink>

      <RouterLink
        to="/orders"
        @click="ui.isMobileSidebarOpen && ui.closeSidebar()"
        :class="[
          'sidenav-links',
          ui.isSidebarOpen ? 'flex items-center gap-3' : 'flex justify-center'
        ]"
      >
        <ListOrdered class="w-5 h-5" />
        <span v-if="ui.isSidebarOpen">Orders</span>
      </RouterLink>

      <RouterLink
        to="/categories"
        @click="ui.isMobileSidebarOpen && ui.closeSidebar()"
        :class="[
          'sidenav-links',
          ui.isSidebarOpen ? 'flex items-center gap-3' : 'flex justify-center'
        ]"
      >
        <ChartBarStacked class="w-5 h-5" />
        <span v-if="ui.isSidebarOpen">Categories</span>
      </RouterLink>

      <RouterLink
        to="/reports"
        @click="ui.isMobileSidebarOpen && ui.closeSidebar()"
        :class="[
          'sidenav-links',
          ui.isSidebarOpen ? 'flex items-center gap-3' : 'flex justify-center'
        ]"
      >
        <BarChart2 class="w-5 h-5" />
        <span v-if="ui.isSidebarOpen">Reports</span>
      </RouterLink>
    </nav>
  </div>
</template>

<script setup>
import { LayoutDashboard, Boxes, ListOrdered, BarChart2, ChartBarStacked } from 'lucide-vue-next'
import { useUIStore } from '@/stores/uiStore'

const ui = useUIStore()
</script>
