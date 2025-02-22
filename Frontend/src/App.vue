<script setup>
//#dcaaf4 #4437a3 #fe79cc #ffdd6b
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
