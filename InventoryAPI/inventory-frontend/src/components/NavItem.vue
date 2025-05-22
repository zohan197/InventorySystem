<template>
    <RouterLink
      :to="to"
      class="flex items-center w-full px-4 py-3 rounded-md transition-all text-gray-300 hover:bg-slate-700"
      :class="ui.isSidebarOpen ? 'justify-start gap-3' : 'justify-center'"
    >
      <component :is="iconComponent" class="w-5 h-5" />
      <span v-if="ui.isSidebarOpen">{{ label }}</span>
    </RouterLink>
  </template>
  
  <script setup>
  import { computed } from 'vue'
  import { useUIStore } from '@/stores/uiStore'
  import {
    LayoutDashboard,
    Boxes,
    ListOrdered,
    BarChart2,
    ChartBarStacked
  } from 'lucide-vue-next'
  
  const props = defineProps({
    to: String,
    label: String,
    icon: String
  })
  
  const ui = useUIStore()
  
  const icons = {
    LayoutDashboard,
    Boxes,
    ListOrdered,
    BarChart2,
    ChartBarStacked
  }
  
  const iconComponent = computed(() => icons[props.icon])
  </script>
  