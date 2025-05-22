import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import LoginView from '@/pages/LoginView.vue'
import { useAuthStore } from '@/stores/authStore'
import RegisterView from '@/pages/RegisterView.vue'
import ProductsView from '@/pages/ProductList.vue'

const requireAuth = async (to, from, next) => {
  const auth = useAuthStore()
  const isAuth = await auth.isAuthenticated()
  if (!isAuth) {
    next('/login')
  } else {
    next()
  }
}

const redirectIfAuthenticated = async (to, from, next) => {
  const auth = useAuthStore()
  const isAuth = await auth.isAuthenticated()
  if (isAuth) {
    next('/')
  } else {
    next()
  }
}


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Dashboard',
      component: HomeView,
      beforeEnter: requireAuth
    },
    {
      path: '/login',
      name: 'Login',
      component: LoginView,
      beforeEnter: redirectIfAuthenticated,
    },
    { 
      path: '/register',
      name: 'Register',
      component: RegisterView 
    },
    { 
      path: '/products',
      name: 'Products',
      component: ProductsView,
      beforeEnter: requireAuth
    },
    {
      path: '/account-settings',
      name: 'AccountSettings',
      component: () => import('@/pages/AccountSettings.vue'),
      beforeEnter: requireAuth
    },
    {
      path: '/categories',
      name: 'Categories',
      component: () => import('@/pages/CategoryList.vue'),
      beforeEnter: requireAuth
    },
    {
      path: '/orders',
      name: 'Orders',
      component: () => import('@/pages/OrderList.vue'),
      beforeEnter: requireAuth
    },
    {
      path: '/reports',
      name: 'Reports',
      component: () => import('@/pages/Reports.vue'),
      beforeEnter: requireAuth
    }
  ],
})

export default router
