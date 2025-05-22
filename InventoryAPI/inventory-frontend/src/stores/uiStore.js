// stores/uiStore.js
import { defineStore } from 'pinia'

export const useUIStore = defineStore('ui', {
  state: () => ({
    isSidebarOpen: true,
    isMobileSidebarOpen : false,
  }),
  actions: {
    toggleSidebar() {
      this.isSidebarOpen = !this.isSidebarOpen
      this.isMobileSidebarOpen = !this.isSidebarOpen
    },
    openSidebar() {
      this.isSidebarOpen = true
      this.isMobileSidebarOpen = true
    },
    closeSidebar() {
      this.isSidebarOpen = false
      this.isMobileSidebarOpen = false
    },
  },
})
