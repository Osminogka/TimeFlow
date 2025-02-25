import { createApp } from 'vue'
import App from './App.vue'
import router from '@/router'
import { Chart, registerables } from 'chart.js';

import { library } from '@fortawesome/fontawesome-svg-core';

import { faUser, faHome } from '@fortawesome/free-solid-svg-icons';
import { faTwitter } from '@fortawesome/free-brands-svg-icons';

import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

// Add icons to library
library.add(faUser, faHome, faTwitter);

Chart.register(...registerables);

createApp(App).component('font-awesome-icon', FontAwesomeIcon).use(router).mount('#app')
