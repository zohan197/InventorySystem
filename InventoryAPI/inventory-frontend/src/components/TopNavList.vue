<template>
  <div class="flex items-center justify-between w-full px-4 h-16">
    <!-- Left: Logo + Sidebar Toggle -->
    <div class="flex items-center space-x-4">
      <!-- Mobile Hamburger -->
      <button
        @click="ui.toggleSidebar"
        v-if="auth.user"
        class="lg:hidden text-gray-600 dark:text-gray-300 focus:outline-none"
      >
        ☰
      </button>

      <!-- Title -->
      <div class="text-xl font-bold text-blue-600 dark:text-blue-400">
        <div v-if="!auth.user">IMS</div>
        <div v-else></div>
      </div>
    </div>

    <!-- Right: Auth Links or User Dropdown -->
    <div class="flex items-center space-x-4">
      <RouterLink
        v-if="!auth.user"
        to="/login"
        class="flex items-center gap-1 text-gray-700 dark:text-gray-200 hover:text-blue-600 dark:hover:text-blue-400"
      >
        <span>Login</span>
      </RouterLink>

      <RouterLink
        v-if="!auth.user"
        to="/register"
        class="flex items-center gap-1 text-gray-700 dark:text-gray-200 hover:text-blue-600 dark:hover:text-blue-400"
      >
        <span>Sign Up</span>
      </RouterLink>

      <div class="relative" ref="dropdownRef" v-if="auth.user">
        <button
          @click="toggleMenu"
          class="flex items-center gap-2 px-3 py-2 rounded-md bg-gray-100 dark:bg-gray-800 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
        >
          <UserIcon class="w-4 h-4" />
          <span class="text-sm">{{ auth.user || 'User' }}</span>
          <ChevronDown class="w-4 h-4" />
        </button>

        <div
          v-if="showMenu"
          class="absolute right-0 mt-2 w-48 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow-md z-50"
        >
          <button
            @click="goToSettings"
            class="w-full text-left px-4 py-2 text-sm text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700"
          >
            <Settings class="inline w-4 h-4 mr-2" />
            Account Settings
          </button>
          <button
            @click="logout"
            class="w-full text-left px-4 py-2 text-sm text-red-600 dark:text-red-400 hover:bg-red-50 dark:hover:bg-gray-700"
          >
            <LogOut class="inline w-4 h-4 mr-2" />
            Logout
          </button>
        </div>
      </div>
    </div>
  </div>
</template>


<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { onClickOutside } from '@vueuse/core'
import { LogOut, Settings, User as UserIcon, ChevronDown } from 'lucide-vue-next'

import { useUIStore } from '@/stores/uiStore'

const ui = useUIStore()

const showMenu = ref(false)
const dropdownRef = ref(null)
onClickOutside(dropdownRef, () => (showMenu.value = false))

const toggleMenu = () => (showMenu.value = !showMenu.value)

const auth = useAuthStore()
const router = useRouter()

const logout = async() => {
  await auth.logout()
  if (!auth.user) {
    router.push('/login')
  }
}

function goToSettings() {
  showMenu.value = false
  router.push('/account-settings')
}
</script>
