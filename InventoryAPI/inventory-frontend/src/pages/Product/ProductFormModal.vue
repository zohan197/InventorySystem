<template>
  <div class="fixed inset-0  bg-black/50 flex justify-center items-center z-50">
    <div class="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md z-60">
      <h2 class="text-2xl font-semibold text-gray-800 dark:text-white mb-6">
        {{ product ? 'Edit' : 'Add' }} Product
      </h2>

      <form @submit.prevent="saveProduct" class="space-y-4">
        <!-- Name Input -->
        <div>
          <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">Name</label>
          <input
            v-model="name"
            ref="productNameInput"
            type="text"
            placeholder="Enter product name"
            class="w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400"
          />
          <p v-if="errors.name" class="text-red-500 text-sm">{{ errors.name }}</p>
        </div>
        
        <!-- Category Dropdown -->
        <div>
          <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">Category</label>
          <select
            v-model="categoryId"
            class="w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100"
          >
            <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
          </select>
          <p v-if="errors.categoryId" class="text-red-500 text-sm">{{ errors.categoryId }}</p>
        </div>
        

        <!-- Stock Input -->
        <div>
          <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">Stock</label>
          <input
            v-model.number="stock"
            type="number"
            placeholder="Enter stock quantity"
            class="w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400"
          />
          <p v-if="errors.stock" class="text-red-500 text-sm">{{ errors.stock }}</p>
        </div>
        

        <!-- Price Input -->
        <div>
          <label class="block text-gray-700 dark:text-gray-300 mb-1 font-medium">Price</label>
          <input
            v-model.number="unitPrice"
            type="number"
            step="0.01" 
            min="1" 
            placeholder="Enter product price"
            class="w-full p-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400"
          />
          <p v-if="errors.unitPrice" class="text-red-500 text-sm">{{ errors.unitPrice }}</p>
        </div>
        

        <!-- Status Checkbox -->
        <div>
          <label class="inline-flex items-center text-gray-700 dark:text-gray-300">
            <input
              type="checkbox"
              v-model="isActive"
              class="form-checkbox text-blue-500 dark:text-blue-400"
            />
            <span class="ml-2">Active</span>
          </label>
        </div>

        <!-- Action Buttons -->
        <div class="flex justify-end gap-4">
          <button
            type="button"
            @click="$emit('close')"
            class="px-5 py-2 border rounded-lg border-gray-300 text-gray-600 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700 transition"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="px-5 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
          >
            Save
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { watch, ref, onMounted } from 'vue';
import api from '@/axios';
import { useForm, useField } from 'vee-validate'
import * as yup from 'yup'
import { useToast } from 'vue-toastification'

const props = defineProps(['product', 'categories']);
const emit = defineEmits(['close', 'saved']);

const productNameInput = ref(null); // Reference to the input element for focusing

const toast = useToast();

const schema = yup.object({
  name: yup.string().required('Name is required'),
  categoryId: yup.string().required('Category is required'),
  stock: yup.number().min(0, 'Stock must be at least 0').required('Stock is required'),
  unitPrice: yup.number().min(0, 'Price must be at least 0').required('Price is required'),
});

const { handleSubmit, errors } = useForm({
  validationSchema: schema,
});

const { value: name } = useField('name', schema.name);
const { value: categoryId } = useField('categoryId', schema.categoryId);
const { value: stock } = useField('stock', schema.stock);
const { value: unitPrice } = useField('unitPrice', schema.unitPrice);
const { value: isActive } = useField('isActive', yup.boolean());

// Separate reference to product ID (used only for updates)
 const productId = ref(null);

watch(() => props.product, (newVal) => {
  if (newVal) {
    name.value = newVal.name;
    categoryId.value = newVal.categoryId;
    stock.value = newVal.stock;
    unitPrice.value = newVal.unitPrice;
    isActive.value = newVal.isActive;
    productId.value = newVal.id; // store ID separately
  } else {
    name.value = '';
    categoryId.value = null;
    stock.value = 0;
    unitPrice.value = 0;
    isActive.value = true;
    productId.value = null;
  }
}, { immediate: true });

const saveProduct = handleSubmit( async (values) => {
  try {
    let savedProduct;
    if (productId.value) {
      const res = await api.put(`/products/${productId.value}`,{
        name: values.name,
        categoryId: values.categoryId,
        isActive: values.isActive,
        stock: values.stock,
        unitPrice: values.unitPrice,
      });
      savedProduct = res.data;
    } else {
      const res = await api.post('/products', {
        name: values.name,
        categoryId: values.categoryId,
        isActive: values.isActive,
        stock: values.stock,
        unitPrice: values.unitPrice,
      });
      savedProduct = res.data;
    }
    console.log('Product saved:', savedProduct);
    toast.success(`Product ${productId.value ? 'updated' : 'added'} successfully!`);
    emit('saved', savedProduct);
    emit('close');
  } catch (err) {
    toast.error(`Failed to save product: ${err.response.data.message || 'Unknown error'}`);
    console.error('Failed to save product:', err);
  }
});

onMounted(() => {
  // Check if it's a valid HTML element before calling focus
  if (productNameInput.value instanceof HTMLInputElement) {
    productNameInput.value.focus()
  }
})
</script>

  