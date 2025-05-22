<template>
    <div class="overflow-x-auto">
        <table class="min-w-full divide-y border-collapse divide-gray-200 dark:divide-gray-700 text-sm">
            <thead class="bg-gradient-to-r from-blue-100 to-blue-200 dark:from-gray-700 dark:to-gray-800 text-gray-700 dark:text-gray-200 uppercase text-xs font-semibold tracking-wide">
                <tr>
                    <th class="table-header">Order ID</th>
                    <th class="table-header">Customer Name</th>
                    <th class="table-header">Order Date</th>
                    <th class="table-header">Total Amount</th>
                    <th class="table-header text-center">Actions</th>
                </tr>
            </thead>
            <tbody v-if="orders.length > 0" class="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
            <template v-for="order in orders" :key="order.id">
                <tr
                @click="toggleOrderDetails(order.id)"
                class="hover:bg-gray-50 dark:hover:bg-gray-700 transition"
                :class="{ 'border-b-0': expandedOrderId === order.id }"
                >
                    <td class="px-6 py-4 font-medium text-gray-900 dark:text-white">{{ order.id }}</td>
                    <td class="px-6 py-4 text-gray-600 dark:text-gray-300">{{ order.customerName }}</td>
                    <td class="px-6 py-4">{{ formatDate(order.orderDate) }}</td>
                    <td class="px-6 py-4">{{ formatCurrency(order.totalAmount) }}</td>
                    <td class="px-6 py-4 text-center">
                        <button @click.stop="$emit('delete-clicked', order)" 
                        class="text-red-600 hover:text-red-900 dark:text-red-400 text-sm">
                            Delete
                        </button>
                    </td>
                </tr>
                <!-- Expanded details row -->
                <tr
                v-if="expandedOrderId === order.id"
                class="bg-gray-50 dark:bg-gray-800 transition"
                >
                <td colspan="5" class="px-6 py-4">
                    <div class="bg-white dark:bg-gray-900 border border-gray-200 dark:border-gray-700 rounded-md p-4">
                    <h4 class="text-sm font-semibold text-gray-800 dark:text-gray-100 mb-2">Order Items</h4>
                    <ul class="list-disc list-inside space-y-1">
                        <li
                        v-for="item in order.items"
                        :key="item.productId"
                        class="text-sm text-gray-700 dark:text-gray-300"
                        >
                        {{ item.productName }} - {{ item.quantity }} pcs @ ${{ item.unitPrice.toFixed(2) }} =
                        ${{ (item.quantity * item.unitPrice).toFixed(2) }}
                        </li>
                    </ul>
                    </div>
                </td>
                </tr>
            </template>
            </tbody>
            <tbody v-else>
                <tr>
                    <td colspan="6" class="text-center px-6 py-4 text-gray-500 dark:text-gray-300">
                        No orders found.
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup>
    import { ref } from 'vue';

    defineProps(['orders']);
    defineEmits(['edit-clicked']);
    const formatCurrency = (value) => {
        if (value === null || value === undefined) return '$0.00'; // Handle edge cases

        // Convert to number and format as currency
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
        }).format(value);
    };
    const formatDate = (dateString) => {
        const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
        return new Date(dateString).toLocaleDateString('en-US', options);
    };

    const expandedOrderId = ref(null);

    function toggleOrderDetails(orderId) {
    expandedOrderId.value = expandedOrderId.value === orderId ? null : orderId;
    }
    
</script>