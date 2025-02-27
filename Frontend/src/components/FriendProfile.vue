<script setup>
import LoadingAnimation from '@/view/LoadingAnimation.vue';
import PieChartComponent from './PieChartComponent.vue';

import trasactionsApi from '@/services/api/transaction';
import friendApi from '@/services/api/friends';

import { computed, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const route = useRoute();
const router = useRouter();

const friendName = computed(() => route.query.username || '');
const error = ref('');
const loading = ref(false);
const transactions = ref([]);

async function getFriendStat() {
    loading.value = true;
    let now = new Date();
    let response = await trasactionsApi.getFriendTransaction(friendName.value, now.getMonth() + 1, now.getFullYear());
    if(response.success) {
        transactions.value = response.enum;
        error.value = '';
    } else {
        error.value = response.message;
    }
    loading.value = false;
}

onMounted(async () => {
    await getFriendStat();
});

async function deleteFriendButton(friendName) {
    try{
        let response = await friendApi.deleteFriend(friendName);
        if(response.success) {
            router.push({ path: '/' });
            error.value = '';
        } else {
            error.value = response.message;
        }
    } catch (e) {
        error.value = e.message;
    }
}

</script>

<template>
    <div v-if="error.length === 0">
        <div class="header">
            <h1>{{ friendName }}</h1>
            <button class="delete-button custom-button" @click="deleteFriendButton(friendName)" />
        </div>
        <div v-if="loading"><LoadingAnimation /></div>
        <div v-else>
            <div v-if="error">{{ error }}</div>
            <div v-else>
                <PieChartComponent :transactions="transactions" :for-self="false" />
            </div>
        </div>
    </div>
    <div class="error-message" v-else>
        {{ error }}
    </div>
</template>

<style scoped>

.header{
    display: flex;
    flex-direction: row;
    align-items: center;
    margin-bottom: 20px;
    gap: 1em;
}

.delete-button{
    background-image: url('../assets/svgs/trash.svg');
}

.custom-button{
    background-position: center;
    background-repeat: no-repeat;
    background-size: contain;
    width: 2.5em;
    height: 2.5em;
    border: none;
    cursor: pointer;
    background-color: inherit;
}

.error-message {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    font-size: 2.5rem;
    font-weight: bold;
    color: white;
    background: linear-gradient(135deg, #fc92d3, #f373b9);
    padding: 15px 25px;
    border-radius: 10px;
    text-align: center;
    box-shadow: 0 4px 15px rgba(255, 0, 127, 0.3);
    animation: fadeIn 0.5s ease-in-out;
}


</style>