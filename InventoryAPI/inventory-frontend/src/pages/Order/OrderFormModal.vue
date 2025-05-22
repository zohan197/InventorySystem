<template>
    <div class="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
        <div class="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md z-60 ">   
            <h2 class="text-2xl font-semibold text-gray-800 dark:text-white mb-6">
                Create Order
            </h2>
            <form @submit.prevent="saveOrder" class="space-y-4 overflow-y-auto max-h-[90vh]">
                <!-- Customer Name Input -->
                <div>
                    <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">Customer Name</label>
                    <input
                        v-model="customerName"
                        type="text"
                        placeholder="Enter customer name"
                        class="w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400"
                    />
                    <p v-if="errors.customerName" class="text-red-500 text-sm">{{ errors.customerName }}</p>
                </div>

                <!-- Order Date Input -->
                <div>
                    <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">Order Date</label>
                    <input
                        v-model="orderDate"
                        type="date"
                        class="w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400"
                    />
                    <p v-if="errors.orderDate" class="text-red-500 text-sm">{{ errors.orderDate }}</p>
                </div>

                <!-- Order Items Section -->
                <div>
                <label class="block text-gray-700 dark:text-gray-300 mb-2 font-medium">Order Items</label>
                
                <div v-for="(item, index) in orderItems" :key="index" class="mb-4 p-3 border rounded-lg dark:border-gray-600">
                    <div class="">
                        <!-- Product Select -->
                        <select v-model="item.productId" @change="updateUnitPrice(index)" class="w-full mb-2 px-3 py-2 border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-100 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500">
                            <option value="">Select Product</option>
                            <option v-for="product in products" :key="product.id" :value="product.id">{{ product.name }}</option>
                        </select>

                        <!-- Quantity Input -->
                        <input type="number" min="1" v-model.number="item.quantity" @input="updateTotalAmount" placeholder="Qty" class="w-full mb-2 px-3 py-2 border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-100 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500" />

                        <!-- Footer Row: Price, Total, Delete Button -->
                        <div class="flex flex-row justify-between items-end w-full gap-2">
                        <!-- Price Label -->
                        <div class="flex-1 text-sm text-gray-700 dark:text-gray-200">
                            Price: ${{ typeof item.unitPrice === 'number' ? item.unitPrice.toFixed(2) : '0.00' }}
                        </div>

                        <!-- Total Label -->
                        <div class="flex-1 text-sm font-semibold text-gray-800 dark:text-white">
                            Total: ${{ (item.unitPrice * item.quantity)?.toFixed(2) || '0.00' }}
                        </div>

                        <!-- Delete Button -->
                        <button
                            @click.prevent="removeItem(index)"
                            class="text-red-600 hover:text-red-800 font-bold cursor-pointer self-end"
                        >
                            ✕
                        </button>
                    </div>
                    </div>
                </div>

                <button @click.prevent="addItem" class="mt-2 px-3 py-1 text-sm text-white bg-green-600 rounded hover:bg-green-700">
                    + Add Product
                </button>
                </div>

                <!-- Total Amount Input -->
                <div>
                    <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">Total Amount</label>
                    <input
                        v-model.number="totalAmount"
                        disabled
                        readonly
                        type="number"
                        step="0.01" 
                        min="0" 
                        placeholder="Enter total amount"
                        class="w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400"
                    />
                    <p v-if="errors.totalAmount" class="text-red-500 text-sm">{{ errors.totalAmount }}</p>
                </div>
                <div class="flex justify-end gap-4">
                    <button @click="emit('close')" class="px-5 py-2 border rounded-lg border-gray-300 text-gray-600 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700 transition">
                        Cancel
                    </button>
                    <!-- Submit Button -->
                    <button type="submit" class="px-5 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition">
                        Save Order
                    </button>
                </div>
                
            </form>
        </div>
    </div>
</template>

<script setup>
    import { watch, ref, onMounted } from 'vue';
    import api from '@/axios';
    import { useForm, useField } from 'vee-validate';
    import * as yup from 'yup';
    import { useToast } from 'vue-toastification';

    const props = defineProps(['order']);
    const emit = defineEmits(['close', 'save']);

    const orderItems = ref([]);
    const products = ref([]);

    const addItem = () => {
        orderItems.value.push({ productId: '', quantity: 1, unitPrice: 0 });
    };
    const removeItem = (index) => {
        orderItems.value.splice(index, 1);
        updateTotalAmount();
    };

    const updateUnitPrice = (index) => {
    const productId = orderItems.value[index].productId;
    const product = products.value.find(p => p.id === productId);
        if (product) {
            orderItems.value[index].unitPrice = product.unitPrice;
            updateTotalAmount();
        }else{
            orderItems.value[index].unitPrice = 0;
            orderItems.value[index].quantity = 0;
            updateTotalAmount();
        }
    };

    const updateTotalAmount = () => {
        const total = orderItems.value.reduce((sum, item) => {
        if (!item.productId || !item.unitPrice || !item.quantity) return sum;
        return sum + (item.unitPrice * item.quantity);
        }, 0);
        totalAmount.value = parseFloat(total.toFixed(2));
    };

    onMounted(async () => {
        const res = await api.get('/products');
        products.value = res.data;
    });

    const toast = useToast();
    const schema = yup.object({
        customerName: yup.string().required('Customer name is required'),
        orderDate: yup.date()
        .transform((value, originalValue) => (originalValue === '' ? null : value))
        .typeError('Please input a valid date')
        .nullable()
        .required('Order date is required'),
    });

    const { handleSubmit, errors } = useForm({
        validationSchema: schema,
        initialValues: {
            customerName: props.order ? props.order.customerName : '',
            orderDate: props.order ? props.order.orderDate : null,
            totalAmount: props.order ? props.order.totalAmount : 0,
        },
    });

    const { value: customerName } = useField('customerName');
    const { value: orderDate } = useField('orderDate');
    const { value: totalAmount } = useField('unitPrice');

    watch(() => props.order, (newOrder) => {
        if (newOrder) {
            customerName.value = newOrder.customerName;
            orderDate.value = newOrder.orderDate;
            totalAmount.value = newOrder.totalAmount;
        }
    });

    const saveOrder = handleSubmit(async () => {
    try {
            let savedOrder;
            const orderData = {
                customerName: customerName.value,
                orderDate: orderDate.value,
                totalAmount: totalAmount.value,
                items: orderItems.value
                .filter(item => item.productId) // ✅ ignore empty productId
                .map(item => ({
                    productId: item.productId,
                    quantity: item.quantity,
                    unitPrice: item.unitPrice
                }))
            };

            const res = await api.post('/orders', orderData);
            savedOrder = res.data;
            console.log('Saved Order:', savedOrder);
            toast.success('Order created successfully!');
            emit('saved', savedOrder);
            emit('close');
        } catch (error) {
            toast.error(`Failed to save the order: ${error.response.data || 'Unknown error'}`);
            console.error('Error saving order:', error);
        }
    });


</script>