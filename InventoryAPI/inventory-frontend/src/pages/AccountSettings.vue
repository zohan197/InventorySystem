<template>
    <MainLayout>
    <div class="max-w-3xl mx-auto p-6">
      <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-100 mb-6">Account Settings</h1>
      
      <form @submit.prevent="saveSettings" class="space-y-6 bg-white dark:bg-gray-800 p-6 rounded-lg shadow">
        <!-- UserName -->
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">Username</label>
          <input
            v-model="name"
            type="text"
            class="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          <p v-if="errors.name" class="text-red-500 text-sm">{{ errors.name }}</p>
        </div>
  
        <!-- Old Password-->
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">Old Password</label>
          <input
            v-model="oldPassword"
            type="password"
            class="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          <p v-if="errors.oldPassword" class="text-red-500 text-sm">{{ errors.oldPassword }}</p>
          <p class="text-xs text-gray-500 mt-1">Enter your current password to confirm changes.</p>
          
        </div>
        <!-- Password -->
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">New Password</label>
          <input
            v-model="password"
            type="password"
            class="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          <p v-if="errors.password" class="text-red-500 text-sm">{{ errors.password }}</p>
          <p class="text-xs text-gray-500 mt-1">Leave blank to keep your current password.</p>
          
        </div>
  
        <!-- Submit -->
        <div class="flex justify-end">
          <button
            type="submit"
            class="bg-blue-600 hover:bg-blue-700 text-white font-semibold px-6 py-2 rounded"
          >
            Save Changes
          </button>
        </div>
      </form>
    </div>

    </MainLayout>
</template>
  
<script setup>
import { useForm, useField } from 'vee-validate'
import * as yup from 'yup'
import { useToast } from 'vue-toastification'
import { useAuthStore } from '@/stores/authStore'
import MainLayout from '@/Layouts/MainLayout.vue'
import api from '@/axios'

const auth = useAuthStore()
const toast = useToast()

const schema = yup.object({
  name: yup.string().required('Username is required'),
  oldPassword: yup.string().required('Old password is required'),
  password: yup.string()
})

const { handleSubmit, errors } = useForm({ 
  validationSchema: schema,
  initialValues: {
    name: auth.user ? auth.user : '',
    oldPassword: '',
    password: ''
  }
  })

const { value: name } = useField('name')
const { value: oldPassword } = useField('oldPassword')
const { value: password } = useField('password')

const saveSettings = handleSubmit(async (values) => {
  try {
    await api.post('/auth/update', {
      Username: values.name,
      OldPassword: values.oldPassword,
      Password: values.password
    })
    toast.success('Account updated successfully!')
  } catch (error) {
    toast.error('Failed to update account. Please try again.')
    console.error('Error saving settings:', error)
  }
})
</script>

  