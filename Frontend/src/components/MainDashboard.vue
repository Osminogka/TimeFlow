<script setup>
import PieChart from '@/components/PieChartComponent.vue';
import LoadingAnimation from '@/view/LoadingAnimation.vue';

import transaction from '@/services/api/transaction';

import { onMounted, ref } from 'vue';
import { useRouter, useRoute } from 'vue-router';

import { onClickOutside } from '@vueuse/core';

const transactions = ref(transaction.thisMonthTransactions.value);
const router = useRouter();
const route = useRoute();
const menuOpen = ref(false);
const menuRef = ref(null);
const loading = ref(false);

async function fetchTransactions() {
    try{
        loading.value = true;
        let time = new Date();
        let reposne = await transaction.getSelfTransaction(time.getMonth() + 1, time.getFullYear());
        if(reposne.success){
            transactions.value = reposne.enum;
        }
    } catch (error) {
        console.error(error);
    }
    loading.value = false;
}

function goToFriends() {
    router.push('/addfriend');
    toggleMenu();
}

function goToProfile() {
    router.push({ name: 'Profile' });
    toggleMenu();
}

function toggleMenu() {
    menuOpen.value = !menuOpen.value;
}

onClickOutside(menuRef, () => {
    menuOpen.value = false;
});

onMounted(async () => {
    await fetchTransactions();
});
</script>

<template>
    <header class="dashboard-header">
        <h1 class="header-text" @click="router.push('/')">TimeFlow</h1>
        <div class="menu">
            <button class="menu-button" @click="toggleMenu" :class="{ rotated: menuOpen }">☰</button>
            <Transition name="dropdown">
                <div v-if="menuOpen" ref="menuRef" class="menu-dropdown">
                    <button class="redirect-button" @click="goToFriends">Friends</button>
                    <button class="redirect-button" @click="goToProfile">Profile</button>
                </div>
            </Transition>
        </div>
    </header>

    <LoadingAnimation v-if="loading" />
    <router-view v-else class="dashboard-container">
        <component 
            :is="route.path === '/' ? PieChart : route.matched[0]?.components.default" 
            v-bind="route.path === '/' ? { transactions: transactions } : {}" 
        />
    </router-view>

</template>

<style scoped>
@import '@/assets/css/animations.css';

.dashboard-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
    background-color: #4437a3;
}

.menu-button {
    background: none;
    border: none;
    font-size: 1.8rem;
    cursor: pointer;
    color: white;
    transition: transform 0.3s ease-in-out;
}

.menu-button.rotated {
    transform: rotate(90deg);
}

.menu {
    position: relative;
}

.menu-dropdown {
    position: absolute;
    right: 0;
    background: #f373b9;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
    border-radius: 5px;
    padding: 0.5rem;
    z-index: 10;
    width: 150px;
}

.menu-dropdown button {
    display: block;
    width: 100%;
    padding: 0.5rem;
    border: none;
    background: none;
    text-align: left;
    cursor: pointer;
    font-size: 1rem;
}

.menu-dropdown button:hover {
    background-color: #f449a7;
}

.logout-button {
    color: red;
}

.dropdown-enter-active, .dropdown-leave-active {
    transition: transform 0.3s ease, opacity 0.3s ease;
}

.dropdown-enter-from, .dropdown-leave-to {
    transform: translateY(-10px);
    opacity: 0;
}

.dashboard-container {
    text-align: center;
    margin-top: 2rem;
}

.chart-container {
    width: 90%;  
    height: auto; 
    margin: 1rem auto;
}

.header-text {
    cursor: pointer;
    font-size: 1.8rem;
    font-weight: bold;
    background: linear-gradient(90deg, #ff007f, #ff5e62);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
}

.header-text:hover {
    transform: scale(1.05);
    opacity: 0.9;
}

@media (min-width: 768px) {
    .chart-container {
        width: 30%;
    }
}
</style>
