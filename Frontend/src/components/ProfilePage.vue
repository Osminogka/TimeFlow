<script setup>
import CustomHideShow from '@/view/CustomHideShow.vue';

import { ref, onMounted, reactive, computed } from 'vue';
import authApi from '@/services/utils';
import transactionApi from '@/services/api/transaction';
import friendsApi from '@/services/api/friends';

class DateOnly {
    constructor(year, month, day) {
        this.year = year;
        this.month = month;
        this.day = day;
    }

    static fromDateTimeString(dateTimeString) {
        const date = new Date(dateTimeString);
        return new DateOnly(date.getFullYear(), date.getMonth() + 1, date.getDate());
    }

    toString() {
        return `${this.year}-${String(this.month).padStart(2, '0')}-${String(this.day).padStart(2, '0')}`;
    }
}

const user = authApi.user;
const friends = ref([]);
const requests = ref([]);
const transactions = ref([]);
const page = ref(0);
const loading = ref(false);
const transactionListRef = ref(null);

async function fetchTransactions() {
    if (loading.value) return;
    loading.value = true;
    let newTransactions = [];

    try{
        
        const response = await transactionApi.getRecentTransactions(page.value);
        if(response.success){
            newTransactions = response.enum;
        }
        else{
            console.log(response.message);
        }
    }
    catch (error) {
        console.error(error);
        loading.value = false;
        return;
    }
    
    if (newTransactions.length > 0) {
        transactions.value.push(...newTransactions);
        page.value++; 
    }

    loading.value = false;
}

function handleScroll() {
    console.log('scrolling');
    const el = transactionListRef.value;
    if (el && el.scrollHeight - el.scrollTop <= el.clientHeight + 10) {
        fetchTransactions();
    }
}

const groupedTransactions = computed(() => {
    if (!transactions.value.length) return {};

    const sortedTransactions = [...transactions.value]
        .map(tx => ({
            ...tx,
            dateOnly: DateOnly.fromDateTimeString(tx.date)
        }))
        .sort((a, b) => {
            if (a.dateOnly.year !== b.dateOnly.year) return b.dateOnly.year - a.dateOnly.year;
            if (a.dateOnly.month !== b.dateOnly.month) return b.dateOnly.month - a.dateOnly.month;
            return b.dateOnly.day - a.dateOnly.day;
        });

    return sortedTransactions.reduce((acc, transaction) => {
        const dateStr = transaction.dateOnly.toString();
        if (!acc[dateStr]) {
            acc[dateStr] = [];
        }
        acc[dateStr].push(transaction);
        return acc;
    }, {});
});


function logout() {
    authApi.clearToken();
    window.location.href = '/';
}

async function fetchRequests() {
    try{
        const response = await friendsApi.getFriendRequest();
        if(response.success){
            requests.value = response.enum;
        }
        else{
            console.log(response.message);
        }
    }
    catch (error) {
        console.error(error);
    }
}

async function fetchFriends() {
    try{
        const response = await friendsApi.getFriendList();
        if(response.success){
            friends.value = response.enum;
        }
        else{
            console.log(response.message);
        }
    }
    catch (error) {
        console.error(error);
    }
}

onMounted(async () => {
    await fetchTransactions();
    await fetchRequests();
    await fetchFriends();
    if (transactionListRef.value) {
        transactionListRef.value.addEventListener("scroll", handleScroll);
    }
});

const showInterface = reactive({
    showTransactions: false,
    showFriends: false,
    showRequests: false
});

function showList(list){
  showInterface[list] = !showInterface[list];
}

async function approveRequest(){
    console.log('Approve request');
}

async function rejectRequest() {
    console.log('Reject request');
}

</script>

<template>
    <div class="container">
        <div class="profile-container">
            <h1 class="profile-header">Profile</h1>

            <div class="user-info">
                <div class="info-item">
                    <label>Username</label>
                    <p>{{ user.name }}</p>
                </div>
                <div class="info-item">
                    <label>Email</label>
                    <p>{{ user.email }}</p>
                </div>
            </div>

            <div class="section">
                <CustomHideShow :showStatistical="true" :showType="'showRequests'" :list="requests" :show-interface="showInterface.showRequests" @showList="showList">
                    Requests
                </CustomHideShow>
                <Transition name="navlists">
                <div v-if="showInterface.showRequests">
                    <div v-if="requests.length" class="scrollable-list">
                        <ul class="list">
                            <li v-for="request in requests" :key="request.id" class="list-item">
                                <div class="container-request">
                                    <p>{{ request }}</p>
                                    <div class="action-buttons-container">
                                        <button class="custom-button approve-button" @click="approveRequest(request)"></button>
                                        <button class="custom-button reject-button" @click="rejectRequest(request)"></button>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div v-else>
                        <p class="empty-text">No requests yet.</p>
                    </div>
                </div>
                </Transition>
            </div>

            <div class="section">
                <CustomHideShow :showStatistical="true" :showType="'showFriends'" :list="friends" :show-interface="showInterface.showFriends" @showList="showList">
                    Friends
                </CustomHideShow>
                <Transition name="navlists">
                <div v-if="showInterface.showFriends">
                    <div v-if="friends.length" class="scrollable-list">
                        <ul class="list">
                            <li v-for="friend in friends" :key="friend.id" class="list-item">
                                {{ friend }}
                            </li>
                        </ul>
                    </div>
                    <div v-else>
                        <p class="empty-text">No friends yet.</p>
                    </div>
                </div>
                </Transition>
            </div>

            <div class="section">
                <CustomHideShow :showStatistical="false" :showType="'showTransactions'" :list="transactionApi.test_transactions" :show-interface="showInterface.showTransactions" @showList="showList">
                    Transactions
                </CustomHideShow>
                <Transition name="navlists">
                <div v-if="showInterface.showTransactions">
                    <div v-if="transactions.length">
                        <div ref="transactionListRef" class="scrollable-list" @scroll="handleScroll">
                            <div v-for="(txList, date) in groupedTransactions" :key="date">
                                <h3 class="date-header">{{ date }}</h3>
                                <ul class="list">
                                    <li v-for="transaction in txList" :key="transaction.id" class="list-item">
                                        <span class="category">{{ transaction.category }}</span> -
                                        <strong class="amount">{{ transaction.amount }}$</strong>
                                        <div class="description">
                                            <span>{{ transaction.description }}</span>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div v-if="loading" class="loading">Loading more transactions...</div>
                        </div>
                    </div>
                    <div v-else>
                        <p class="empty-text">No transactions yet.</p>
                    </div>
                </div>
                </Transition>
            </div>

            <button class="logout-btn" @click.prevent="logout">Logout</button>
        </div>
    </div>
</template>

<style scoped>
.container{
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
}

.profile-container {
    width: 100%;
    max-width: 500px;
    margin: 1em;
    padding: 2rem;
    border: 2px solid #4437a3;
    border-radius: 10px;
    background: linear-gradient(90deg, #fc92d3, #dcaaf4);
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    text-align: center;
    animation: fadeIn 0.5s ease-in-out;
}

.profile-header {
    font-size: 2rem;
    font-weight: bold;
    color: #cf1487;
}

.user-info {
    padding: 1rem;
    background: rgba(255, 255, 255, 0.2);
    border-radius: 8px;
    text-align: left;
    display: flex;
    flex-direction: column;
}

.info-item {
    display: flex;
    flex-direction: column;
}

label {
    font-size: 0.7rem;
    color: #666;
    font-weight: bold;
    text-transform: uppercase;
    letter-spacing: 0.5px;
}

p {
    font-size: 1rem;
    font-weight: bold;
    color: #222;
}


.section {
    margin-top: 1.5rem;
    padding: 1rem;
    background: rgba(255, 255, 255, 0.2);
    border-radius: 8px;
}

.section-title {
    font-size: 1.5rem;
    color: #4437a3;
}

.list {
    list-style: none;
    padding: 0;
}

.list-item {
    background: rgba(255, 255, 255, 0.3);
    margin: 5px 0;
    padding: 10px;
    border-radius: 5px;
    transition: transform 0.2s ease-in-out;
    max-width: auto;
}

.empty-text {
    color: #666;
    font-style: italic;
}

.logout-btn {
    margin-top: 1.5rem;
    padding: 0.7rem 1.5rem;
    background: #e74c3c;
    color: white;
    border: none;
    border-radius: 8px;
    font-size: 1rem;
    cursor: pointer;
    transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
}

.logout-btn:hover {
    transform: scale(1.05);
    opacity: 0.9;
}

@keyframes fadeIn {
    from { opacity: 0; transform: translateY(-10px); }
    to { opacity: 1; transform: translateY(0); }
}

.category {
    font-size: 1rem;
    font-weight: bold;
    color: #ff007f;
}

.amount {
    font-size: 1.2rem;
    font-weight: bold;
    color: #ff5e62;
}

.description {
    font-size: 0.9rem;
    color: #020202;
    margin-top: 5px;
}

.scrollable-list {
    max-height: 400px; /* Adjust based on your UI */
    overflow-y: auto;
    border: 1px solid #ccc;
    padding: 10px;
}

.container-request{
    display: flex;
    justify-content: space-between;
    align-items: center;
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

.action-buttons-container{
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 4.5em;
}

.approve-button{
    background-image: url('../assets/svgs/check.svg');
    
}

.reject-button{
    background-image: url('../assets/svgs/xmark.svg');
}

</style>
