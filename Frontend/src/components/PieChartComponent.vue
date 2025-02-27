<script setup>
import { computed, defineProps, ref } from 'vue';
import { Pie } from 'vue-chartjs';
import { onClickOutside } from '@vueuse/core';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import transactionApi from '@/services/api/transaction';

ChartJS.register(ArcElement, Tooltip, Legend, ChartDataLabels);

const props = defineProps({
  transactions: Array,
  forSelf: {
    type: Boolean,
    default: true
  }
});


const isModalOpen = ref(false);
const modalRef = ref(null);
const transactionData = ref({
    amount: '',
    description: '',
    category: '',
    date: ''
});

onClickOutside(modalRef, () => {
  isModalOpen.value = false;
});

function openModal(category) {
  isModalOpen.value = true;
  transactionData.value.category = category;
  const now = new Date();
  const dateTime = new transactionApi.DateOnly(now.getFullYear(), now.getMonth() + 1, now.getDate());
  transactionData.value.date = dateTime.toDateTimeString();
}

async function sendTransaction(){
  try{
    let response = await transactionApi.createTransaction(transactionData.value);
    if(response.success){
        transactionApi.thisMonthTransactions.value.push(transactionData.value);
        isModalOpen.value = false;
        transactionData.value = {
            amount: '',
            description: '',
            category: '',
            date: ''
        };
    }
  } catch (error) {
    console.error(error);
  }
}

function closeModal() {
    isModalOpen.value = false;
}

function submitTransaction() {
    console.log('Transaction submitted:', transactionData.value);
    isModalOpen.value = false;
}

const chartData = computed(() => {
  const categories = {
    "Food": 0,
    "Transport": 0,
    "Entertainment": 0,
    "Health": 0,
    "Education": 0,
    "Other": 0
  };
  
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
        label: (tooltipItem) => {
          const total = tooltipItem.dataset.data.reduce((acc, val) => acc + val, 0);
          const value = tooltipItem.raw;
          const percentage = ((value / total) * 100).toFixed(1);
          return `${tooltipItem.label}: $${value} (${percentage}%)`;
        }
      }
    },
    datalabels: {
      color: '#fff',
      font: { weight: 'bold', size: 14 },
      formatter: (value, context) => {
        const total = context.dataset.data.reduce((acc, val) => acc + val, 0);
        const percentage = ((value / total) * 100).toFixed(1);
        return `${percentage}%`;
      }
    }
  }
};

const categoryValues = computed(() => {
  const total = chartData.value.datasets[0].data.reduce((acc, val) => acc + val, 0);
  return chartData.value.labels.map((category, index) => ({
    category,
    value: chartData.value.datasets[0].data[index],
    percentage: ((chartData.value.datasets[0].data[index] / total) * 100).toFixed(1),
    color: chartData.value.datasets[0].backgroundColor[index]
  }));
});
</script>

<template>
  <div class="chart-container">
    <div v-if="transactions.length === 0" class="no-transactions">
      <h2>No transactions to display</h2>
      <p>Click the plus button to add a new transaction</p>
    </div>
    <div v-else class="pie-container">
      <Pie :data="chartData" :options="chartOptions" />
    </div>
    <ul class="legend">
      <li v-for="(item, index) in categoryValues" :key="index">
        <div>
          <span class="color-box" :style="{ backgroundColor: item.color }"></span>
          {{ item.category }}: ${{ item.value }}
        </div>
        <button v-if="forSelf" class="plus-buton" @click.prevent="openModal(item.category)" />
      </li>
    </ul>
  </div>
  <Transition name="fade">
      <div v-if="isModalOpen" class="modal-overlay">
          <Transition name="slide-up">
              <div v-if="isModalOpen" ref="modalRef" class="modal-content">
                  <h2>{{ transactionData.category }}</h2>
                  <form @submit.prevent="submitTransaction">
                      <input v-model="transactionData.amount" type="number" placeholder="Amount" required>
                      <input v-model="transactionData.description" type="text" placeholder="Description" required>
                      <div class="modal-actions">
                          <button type="submit" class="submit-btn" @click.prevent="sendTransaction()">Save</button>
                          <button type="button" class="cancel-btn" @click="closeModal">Cancel</button>
                      </div>
                  </form>
              </div>
          </Transition>
      </div>
  </Transition>
</template>

<style scoped>
.chart-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
}

.no-transactions{
  display: flex;
  flex-direction: column;
  align-items: center;
}

.legend {
  margin-top: 20px;
  list-style: none;
  padding: 0;
  display: flex;
  gap: 15px;
  flex-wrap: wrap;
  flex-direction: column;
}

.legend li {
  display: flex;
  align-items: center;
  font-size: 16px;
  font-weight: bold;
  color: #333;
  border-bottom: 1px solid black;
  justify-content: space-between;
}

.color-box {
  width: 15px;
  height: 15px;
  display: inline-block;
  margin-right: 8px;
  border-radius: 3px;
}

.plus-buton{
  background-image: url('../assets/svgs/plus.svg');
  background-size: cover;
  width: 2.2em;
  height: 2.2em;
  border: none;
  cursor: pointer;
  background-size: auto;
  background-color: #dcaaf4;
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
  margin: 1em;
  transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
}

.plus-buton:hover {
  transform: scale(1.3);
  opacity: 0.9;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  backdrop-filter: blur(5px);
  z-index: 1000;
}

.modal-content {
  background: #fc92d3;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  width: 70%;
  text-align: center;
}

.modal-content input {
  width: 100%;
  padding: 0.5rem;
  margin: 0.5rem 0;
  border: 1px solid #4437a3;
  border-radius: 5px;
  background-color: #dcaaf4;
  outline: none;
}

.modal-actions {
  display: flex;
  justify-content: space-between;
  margin-top: 1rem;
}

.submit-btn {
  background: linear-gradient(90deg, #2ecc71, #27ae60);
  color: white;
  padding: 0.6rem 1.2rem;
  border: none;
  border-radius: 8px;
  font-weight: bold;
  font-size: 1rem;
  cursor: pointer;
  transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
  box-shadow: 0px 4px 10px rgba(39, 174, 96, 0.3);
}

.submit-btn:hover {
  transform: scale(1.05);
  opacity: 0.9;
}

.cancel-btn {
  background: linear-gradient(90deg, #ff5e62, #e74c3c);
  color: white;
  padding: 0.6rem 1.2rem;
  border: none;
  border-radius: 8px;
  font-weight: bold;
  font-size: 1rem;
  cursor: pointer;
  transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
  box-shadow: 0px 4px 10px rgba(231, 76, 60, 0.3);
}

.cancel-btn:hover {
  transform: scale(1.05);
  opacity: 0.9;
}

/* Animations */

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease-in-out;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

.slide-up-enter-active, .slide-up-leave-active {
  transition: transform 0.3s ease-in-out, opacity 0.3s ease-in-out;
}
.slide-up-enter-from, .slide-up-leave-to {
  opacity: 0;
  transform: translateY(20px);
}

@media (min-width: 768px) {
  .modal-content {
    width: 30%;
  }

  .chart-container{
    display: flex;
    flex-direction: row;
    gap: 10rem;
    width: 100%;
    align-items: center;
    justify-content: center;
  }

  .pie-container{
    width:30%;
    height:auto;
  }
}
</style>