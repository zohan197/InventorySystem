<template>
    <div class="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
        <div class="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md z-60">
            <h2 class="text-2xl font-semibold text-gray-800 dark:text-white mb-6">
                {{ category ? 'Edit' : 'Add' }} Category
            </h2>

            <form @submit.prevent="saveCategory">
                <!-- Name Input -->
                <div class="mb-6">
                    <label class="block text-gray-700 dark:text-gray-300 mb-2 font-medium">Name</label>
                    <input
                        v-model="form.name"
                        type="text"
                        placeholder="Enter category name"
                        class="w-full p-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-gray-100 dark:placeholder-gray-400" >
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
import { ref, watch, reactive } from 'vue';
import api from '@/axios'; 

const props = defineProps(['category', 'categories']);
const emit = defineEmits(['close', 'category-saved']);

const form = reactive({
    name: ''
});

const categoryId = ref(null);

watch(() => props.category, (newCategory) => {
    if (newCategory) {
        form.name = newCategory.name;
        categoryId.value = newCategory.id;
    } else {
        form.name = '';
        categoryId.value = null;
    }
}, { immediate: true });

const saveCategory = async () => {
    try {
        if (categoryId.value) {
            await api.put(`/categories/${categoryId.value}`, form);
        } else {
            await api.post('/categories', form);
        }
        emit('saved');
        emit('close');
    } catch (error) {
        console.error('Error saving category:', error);
    }
};
</script>