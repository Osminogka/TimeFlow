<script setup>
import { computed, defineProps } from 'vue';
import { Pie } from 'vue-chartjs';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';

ChartJS.register(ArcElement, Tooltip, Legend);

const props = defineProps({
  transactions: Array
});

const chartData = computed(() => {
  const categories = {};
  
  props.transactions.forEach(tx => {
    categories[tx.category] = (categories[tx.category] || 0) + tx.amount;
  });

  return {
    labels: Object.keys(categories),
    datasets: [
      {
        data: Object.values(categories),
        backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40'],
        borderColor: '#ffffff',
        borderWidth: 2
      }
    ]
  };
});

const chartOptions = {
  responsive: true,
  plugins: {
    legend: { display: false },
    tooltip: {
      callbacks: {
        label: (tooltipItem) => `${tooltipItem.label}: $${tooltipItem.raw}`
      }
    },
    datalabels: {
      color: '#fff',
      font: { weight: 'bold', size: 14 },
      formatter: (value, context) => context.chart.data.labels[context.dataIndex]
    }
  }
};

const categoryValues = computed(() => {
  return Object.entries(chartData.value.labels).map(([index, category]) => ({
    category,
    value: chartData.value.datasets[0].data[index],
    color: chartData.value.datasets[0].backgroundColor[index]
  }));
});
</script>

<template>
  <div class="chart-container">
    <Pie :data="chartData" :options="chartOptions" />
    
    <ul class="legend">
      <li v-for="(item, index) in categoryValues" :key="index">
        <span class="color-box" :style="{ backgroundColor: item.color }"></span>
        {{ item.category }}: ${{ item.value }}
      </li>
    </ul>
  </div>
</template>

<style scoped>
.chart-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
}

.legend {
  margin-top: 20px;
  list-style: none;
  padding: 0;
  display: flex;
  gap: 15px;
  flex-wrap: wrap;
}

.legend li {
  display: flex;
  align-items: center;
  font-size: 16px;
  font-weight: bold;
  color: #333;
}

.color-box {
  width: 15px;
  height: 15px;
  display: inline-block;
  margin-right: 8px;
  border-radius: 3px;
}
</style>
