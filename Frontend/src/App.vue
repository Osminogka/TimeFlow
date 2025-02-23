<script setup>
//#4437a3 #dcaaf4 #fc92d3 #ffdd6b
import Dashboard from './components/MainDashboard.vue';
import GreetingPage from './components/GreetingPage.vue';

import authApi from '@/services/utils';

</script>

<template>
  <div v-if="!authApi.isAuthenticated()">
    <GreetingPage />
  </div>
  <div v-else>
    <Dashboard />
  </div>
  <p>{{ errorMessage }}</p>
</template>
