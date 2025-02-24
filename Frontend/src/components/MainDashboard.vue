<script setup>
import PieChart from '@/components/PieChartComponent.vue';

import authApi from '@/services/utils'
import transaction from '@/services/api/transaction';

import { ref } from 'vue';

const transactions = ref(transaction.test_transactions);

function logout(){
    authApi.clearToken();
    window.location.href = '/';
}

</script>

<template>
    <div>
        <h1>Dashboard</h1>
        <p>Here is the dashboard</p>
        <div class="chart-container">
            <PieChart :transactions="transactions" />
        </div>
    </div>
    <button @click.prevent="logout()">Logout</button>
</template>

<style scoped>

.chart-container {
  width: 400px;  
  height: 400px; 
}

</style>