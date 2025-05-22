<template>
    <MainLayout>
        <div class="container mx-auto p-4">
            <h1 class="text-2xl font-bold mb-4">Download Reports</h1>
            <div class="p-6 space-y-4">

            <div class="flex flex-col sm:flex-row sm:items-end gap-4">
            <!-- Date Filters -->
            <div class="flex-1">
                <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">Start Date</label>
                <input type="date" v-model="startDate" 
                class="input w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400" />
            </div>
            <div class="flex-1">
                <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">End Date</label>
                <input type="date" v-model="endDate" 
                class="input w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400" />
            </div>

            <!-- Download Button -->
            <button @click="downloadReport"
                    class="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition">
                Download Report
            </button>
            </div>
            </div>

        </div>
    </MainLayout>
</template>

<script setup>
defineOptions({
  name: 'ReportsPage'
});
import MainLayout from '@/Layouts/MainLayout.vue';
import { ref } from 'vue';
import ExcelJS from 'exceljs';
import api from '@/axios'; // Ensure this is set up to handle base URL + auth if needed
import { useAuthStore } from '@/stores/authStore';
import  router  from '@/router'; 

const startDate = ref('');
const endDate = ref('');

async function downloadReport() {
  try {
    const response = await api.get('/reports/summary', {
      params: {
        startDate: startDate.value || null,
        endDate: endDate.value || null,
      }
    });

    const { orders, orderItems, products, categories } = response.data;

    const workbook = new ExcelJS.Workbook();

    // --- Orders Sheet ---
    const ordersSheet = workbook.addWorksheet('Orders');
    ordersSheet.columns = [
      { header: 'ID', key: 'id' },
      { header: 'Customer', key: 'customerName' },
      { header: 'Order Date', key: 'orderDate' },
      { header: 'Total Amount', key: 'totalAmount' }
    ];
    ordersSheet.addRows(orders);

    // --- Order Items Sheet ---
    const orderItemsSheet = workbook.addWorksheet('Order Items');
    orderItemsSheet.columns = [
      { header: 'ID', key: 'id' },
      { header: 'Order ID', key: 'orderId' },
      { header: 'Product', key: 'productName' },
      { header: 'Quantity', key: 'quantity' },
      { header: 'Unit Price', key: 'unitPrice' },
      { header: 'Subtotal', key: 'subtotal' }
    ];
    orderItemsSheet.addRows(orderItems);

    // --- Products Sheet ---
    const productsSheet = workbook.addWorksheet('Products');
    productsSheet.columns = [
      { header: 'ID', key: 'id' },
      { header: 'Name', key: 'name' },
      { header: 'Price', key: 'unitPrice' }
    ];
    productsSheet.addRows(products);

    // --- Categories Sheet ---
    const categoriesSheet = workbook.addWorksheet('Categories');
    categoriesSheet.columns = [
      { header: 'ID', key: 'id' },
      { header: 'Name', key: 'name' }
    ];
    categoriesSheet.addRows(categories);

    // --- Export ---
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    });

    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = `report_${Date.now()}.xlsx`;
    link.click();
  } catch (err) {
    if (err.response && err.response.status === 401) {
      console.warn('Unauthorized. Redirecting to login...');
      useAuthStore().logout(); // Clear auth store
      router.push('/login') // if using Vue Router
    }else{
      console.error('Failed to download report:', err);
    }
  }
}

</script>


<style scoped>
.input {
  @apply w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md bg-white dark:bg-gray-800 text-gray-800 dark:text-white;
}
</style>