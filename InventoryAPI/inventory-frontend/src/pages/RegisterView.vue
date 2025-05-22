<template>
    <TopNavList></TopNavList>
    <div class="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900 transition-colors duration-300">
      <div class="w-full max-w-md bg-white dark:bg-gray-800 p-8 rounded-2xl shadow-xl border border-gray-200 dark:border-gray-700">
        
        <!-- Logo -->
        <div class="flex justify-center mb-6">
          <div class="w-16 h-16 bg-green-600 dark:bg-green-500 rounded-full flex items-center justify-center shadow-md">
            <span class="text-white text-2xl font-bold">📝</span>
          </div>
        </div>
  
        <!-- Title -->
        <h1 class="text-3xl font-bold text-center text-gray-800 dark:text-white mb-2">
          Register Account
        </h1>
        <p class="text-center text-sm text-gray-500 dark:text-gray-400 mb-6">
          Create your account to manage inventory
        </p>
  
        <!-- Register Form -->
        <form @submit.prevent="saveAccount" class="space-y-5">
          <div>
            <label for="username" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Username</label>
            <input
              v-model="username"
              id="username"
              type="text"
              placeholder="Choose a username"
              class="w-full px-4 py-2 rounded-md border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-2 focus:ring-green-500 dark:focus:ring-green-400"
            />
            <p v-if="errors.username" class="text-red-500 text-sm">{{ errors.username }}</p>
          </div>
  
          <div>
            <label for="password" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Password</label>
            <input
              v-model="password"
              id="password"
              type="password"
              placeholder="Create a password"
              class="w-full px-4 py-2 rounded-md border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-2 focus:ring-green-500 dark:focus:ring-green-400"
            />
            <p v-if="errors.password" class="text-red-500 text-sm">{{ errors.password }}</p>
          </div>

          <!-- Confirm Password -->
          <div>
            <label for="confirmPassword" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Confirm Password</label>
            <input
              v-model="confirmPassword"
              id="confirmPassword"
              type="password"
              placeholder="Re-enter your password"
              class="w-full px-4 py-2 rounded-md border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-2 focus:ring-green-500 dark:focus:ring-green-400"
            />
            <p v-if="errors.confirmPassword" class="text-red-500 text-sm">{{ errors.confirmPassword }}</p>
          </div>

          <!-- Divider -->
          <div class="border-t border-gray-200 dark:border-gray-700 my-4"></div>
  
          <button
            type="submit"
            class="w-full bg-green-600 hover:bg-green-700 dark:bg-green-500 dark:hover:bg-green-600 text-white font-semibold py-2 px-4 rounded-md transition duration-200 shadow"
          >
            Register
          </button>
        </form>
  
        <!-- Error Message -->
        <p v-if="auth.error" class="text-red-500 dark:text-red-400 text-sm mt-4 text-center">{{ auth.error }}</p>

        <!-- Login Link -->
        <p class="text-sm text-center mt-6 text-gray-600 dark:text-gray-300">
            Already have an account?
            <RouterLink to="/login" class="text-green-600 hover:underline dark:text-green-400">Login</RouterLink>
        </p>
      </div>
    </div>
  </template>
  
  <script setup>
  
  import { useAuthStore } from '@/stores/authStore'
  import { useRouter } from 'vue-router'
  import TopNavList from '@/components/TopNavList.vue'
  import {useForm, useField} from 'vee-validate'
  import * as yup from 'yup'
  
  const auth = useAuthStore()
  const router = useRouter()

  const schema = yup.object({
    username: yup.string().required('Username is required').min(3, 'Username must be at least 3 characters long'),
    password: yup.string().required('Password is required').min(6, 'Password must be at least 6 characters long'),
    confirmPassword: yup.string()
      .oneOf([yup.ref('password'), null], 'Passwords does not match')
      .required('Confirm Password is required')
  })

  const { handleSubmit, errors } = useForm({
    validationSchema: schema,
    initialValues: {
      username: '',
      password: '',
      confirmPassword: ''
    }
  })

  const { value: username } = useField('username')
  const { value: password } = useField('password')
  const { value: confirmPassword } = useField('confirmPassword')

  const saveAccount = handleSubmit(async (values) => {
    try{
      const success = await auth.register(values.username, values.password)
      if (success) {
        router.push('/login')
      } else {
        console.error('Registration failed')
      }
    }catch (error) {
      console.error('Error during registration:', error)
    }
  })

  </script>
  