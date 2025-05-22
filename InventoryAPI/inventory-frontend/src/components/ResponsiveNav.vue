<template>
    <aside
      :class="[
        'fixed top-0 left-0 h-screen z-40 transition-all duration-300 ease-in-out border-r',
        'bg-[#0f172a] border-[#1e293b] dark:border-0 text-white dark:bg-gray-800', // 👈 match TopNav
        ui.isSidebarOpen ? 'w-64' : 'w-16'
      ]"
    >
      <!-- Toggle Button -->
      <div class="flex items-center justify-center h-16 dark:border-b-1 border-gray-700">
        <button @click="ui.toggleSidebar" class="text-gray-300 hover:text-white focus:outline-none">
          <Menu class="w-6 h-6" />
        </button>
      </div>
  
      <!-- Nav Links -->
      <nav class="mt-2 flex flex-col items-center space-y-2">
        <NavItem to="/" icon="LayoutDashboard" label="Dashboard" />
        <NavItem to="/products" icon="Boxes" label="Products" />
        <NavItem to="/orders" icon="ListOrdered" label="Orders" />
        <NavItem to="/categories" icon="ChartBarStacked" label="Categories" />
        <NavItem to="/reports" icon="BarChart2" label="Reports" />
      </nav>
    </aside>
  </template>
  
  <script setup>
  import { useUIStore } from '@/stores/uiStore'
  import NavItem from './NavItem.vue'
  import { Menu } from 'lucide-vue-next'
  
  const ui = useUIStore()
  </script>
  