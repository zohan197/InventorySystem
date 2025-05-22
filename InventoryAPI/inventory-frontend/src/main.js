import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import  createPersistedState  from 'pinia-plugin-persistedstate';
import App from './App.vue'
import router from './router'
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";
//Charts
import VueECharts from 'vue-echarts';
import { use } from 'echarts/core';

import {
  CanvasRenderer
} from 'echarts/renderers'
import {
  BarChart,
  LineChart
} from 'echarts/charts'
import {
  GridComponent,
  TooltipComponent,
  TitleComponent,
  LegendComponent
} from 'echarts/components'

// Register modules
use([
  CanvasRenderer,
  BarChart,
  LineChart,
  GridComponent,
  TooltipComponent,
  TitleComponent,
  LegendComponent
])

// Register component globally
const app = createApp(App)

// Register the component
const pinia = createPinia()
pinia.use(createPersistedState)

app.component('v-chart', VueECharts)
app.use(pinia)
app.use(router)
app.use(Toast, {
  position: "top-right",
  timeout: 3000,
  closeOnClick: true,
  pauseOnFocusLoss: true,
  draggable: true,
  progress: true,
  progressBar: true,
  maxToasts: 5,
  newestOnTop: false,
});

app.mount('#app')


