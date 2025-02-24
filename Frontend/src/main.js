import { createApp } from 'vue'
import App from './App.vue'
import router from '@/router'
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

createApp(App).use(router).mount('#app')
