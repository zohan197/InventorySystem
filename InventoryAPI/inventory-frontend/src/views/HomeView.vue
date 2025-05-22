<template>
  <MainLayout>
    <div class="p-4 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
      <!-- Summary Cards -->
      <SummaryCard title="Total Orders" :value="dashboardData.totalOrders" />
      <SummaryCard title="Total Revenue" :value="dashboardData.totalRevenue" prefix="$" />
      <SummaryCard title="Active Products" :value="dashboardData.activeProducts" />
      <SummaryCard title="Low Stock Items" :value="dashboardData.lowStockCount" />
    </div>

    <!-- Charts -->
    <div class="p-4 grid grid-cols-1 lg:grid-cols-2 gap-4">
      <Card>
        <CardTitle>Sales Over Time</CardTitle>
        <CardContent>
          <v-chart :option="salesChartOptions" autoresize style="height: 300px;" />
        </CardContent>
      </Card>
      <Card>
        <CardTitle>Top Products</CardTitle>
        <CardContent>
          <v-chart :option="topProductsOptions" autoresize style="height: 300px;" />
        </CardContent>
      </Card>
    </div>
  </MainLayout>
</template>

<script setup>
import { ref, onMounted, nextTick } from 'vue'
import { use } from 'echarts/core'
import { BarChart, LineChart } from 'echarts/charts'
import { GridComponent, TooltipComponent, LegendComponent, TitleComponent } from 'echarts/components'
import { CanvasRenderer } from 'echarts/renderers'
import VChart from 'vue-echarts'
import MainLayout from '@/Layouts/MainLayout.vue'
import SummaryCard from '@/components/SummaryCard.vue'
import { Card, CardTitle, CardContent } from '@/components/ui/card'
import api from '@/axios'
import { useAuthStore } from '@/stores/authStore';
import  router  from '@/router'; 

use([
  TitleComponent,
  TooltipComponent,
  LegendComponent,
  GridComponent,
  BarChart,
  LineChart,
  CanvasRenderer
])

const dashboardData = ref({
  totalOrders: 0,
  totalRevenue: 0,
  activeProducts: 0,
  lowStockCount: 0,
  salesOverTime: [],    // [{ date: '2025-05-01', total: 100 }, ...]
  topProducts: []       // [{ name: 'Mouse', quantity: 50 }, ...]
})

const salesChartOptions = ref({})
const topProductsOptions = ref({})

function buildCharts() {
  // Check if salesOverTime is populated
  if (dashboardData.value.salesOverTime && dashboardData.value.salesOverTime.length > 0) {
    salesChartOptions.value = {
      title: { text: '' },
      tooltip: { trigger: 'axis' },
      xAxis: { type: 'category', data: dashboardData.value.salesOverTime.map(d => d.date) },
      yAxis: { type: 'value' },
      series: [{ name: 'Sales', type: 'line', data: dashboardData.value.salesOverTime.map(d => d.total) }]
    }
  } else {
    console.error('Sales data is empty or missing.');
  }

  // Check if topProducts is populated
  if (dashboardData.value.topProducts && dashboardData.value.topProducts.length > 0) {
    topProductsOptions.value = {
      title: { text: '' },
      tooltip: {},
      xAxis: { type: 'category', data: dashboardData.value.topProducts.map(p => p.name) },
      yAxis: { type: 'value' },
      series: [{ name: 'Quantity', type: 'bar', data: dashboardData.value.topProducts.map(p => p.quantity) }]
    }
  } else {
    console.error('Top products data is empty or missing.');
  }
}

onMounted(async () => {
  try {
    const { data } = await api.get('/dashboard/summary')
    dashboardData.value = data
    await nextTick(() => {
      buildCharts() // Trigger the chart rebuild after Vue has updated the DOM
    })
  } catch (err) {
    if (err.response && err.response.status === 401) {
      console.warn('Unauthorized. Redirecting to login...');
      useAuthStore().logout(); // Clear auth store
      router.push('/login') // if using Vue Router
    }else{
      console.error('Failed to fetch dashboard data:', err);
    }
  }
})
</script>
