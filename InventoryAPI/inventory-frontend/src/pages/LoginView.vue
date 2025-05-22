<template>
  <TopNav></TopNav>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900 transition-colors duration-300">
    <div class="w-full max-w-md bg-white dark:bg-gray-800 p-8 rounded-2xl shadow-xl border border-gray-200 dark:border-gray-700">
      
      <!-- Logo -->
      <div class="flex justify-center mb-6">
        <div class="w-16 h-16 bg-blue-600 dark:bg-blue-500 rounded-full flex items-center justify-center shadow-md">
          <span class="text-white text-2xl font-bold">🗃️</span>
        </div>
      </div>

      <!-- Title -->
      <h1 class="text-3xl font-bold text-center text-gray-800 dark:text-white mb-2">
        Inventory Management System
      </h1>
      <p class="text-center text-sm text-gray-500 dark:text-gray-400 mb-6">
        Please login to continue
      </p>

      <!-- Login Form -->
      <form @submit.prevent="handleLogin" class="space-y-5">
        <div>
          <label for="username" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Username</label>
          <input
            v-model="username"
            ref="usernameInput"
            id="username"
            type="text"
            placeholder="Your username"
            class="w-full px-4 py-2 rounded-md border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-2 focus:ring-blue-500 dark:focus:ring-blue-400"
          />
        </div>

        <div>
          <label for="password" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Password</label>
          <input
            v-model="password"
            id="password"
            type="password"
            placeholder="Your password"
            class="w-full px-4 py-2 rounded-md border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-2 focus:ring-blue-500 dark:focus:ring-blue-400"
          />
        </div>

        <!-- Divider -->
        <div class="border-t border-gray-200 dark:border-gray-700 my-4"></div>

        <button
          type="submit"
          class="w-full bg-blue-600 hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 text-white font-semibold py-2 px-4 rounded-md transition duration-200 shadow"
        >
          Login
        </button>
      </form>

      <!-- Error Message -->
      <p v-if="auth.error" class="text-red-500 dark:text-red-400 text-sm mt-4 text-center">{{ auth.error }}</p>

      <!-- Register Link -->
      <p class="text-sm text-center mt-6 text-gray-600 dark:text-gray-300">
        Don’t have an account?
        <RouterLink to="/register" class="text-blue-600 hover:underline dark:text-blue-400">Register</RouterLink>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import { useRouter } from 'vue-router'
import TopNav from '@/components/TopNav.vue'

const auth = useAuthStore()
const router = useRouter()
const username = ref('')
const password = ref('')

const usernameInput = ref(null)

onMounted(() => {
  // Check if it's a valid HTML element before calling focus
  if (usernameInput.value instanceof HTMLInputElement) {
    usernameInput.value.focus()
  }
})

const handleLogin = async () => {
  try{
    const res = await auth.login(username.value, password.value)
    if (auth.user) {
      router.push('/')
    }else{
      auth.error = res.error || 'Login failed. Please check your credentials.'
    }
  }catch (error) {
    console.error('Login error:', error)
    
    auth.error = 'Login failed. Please check your credentials.'
  }
}
</script>
