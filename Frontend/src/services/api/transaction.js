import { getToken } from '@/services/utils';

import { ref } from 'vue';

const BASE_URL = '/api/transactions';
const token = getToken();

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

    toDateTimeString() {
        return `${this.year}-${String(this.month).padStart(2, '0')}-${String(this.day).padStart(2, '0')}T00:00:00Z`;
    }
}

const thisMonthTransactions = ref([]);

const postRequest = async (endpoint, data) => {
    const url = `${BASE_URL}${endpoint}`;
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'API request failed');
    }

    return response.json();
};

const getRequest = async (endpoint) => {
    const url = `${BASE_URL}${endpoint}`;
    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'API request failed');
    }

    return response.json();
}

const createTransaction = async (transaction) => {
    return postRequest('/create', transaction);
};

const getSelfTransaction = async (month, year,) => {
    let response = await getRequest('/self/' + month + '/' + year);
    if(response.success)
        thisMonthTransactions.value = response.enum;
    return response;
}

const getRecentTransactions = async (page) => {
    return getRequest('/recent/' + page);
}

const getFriendTransaction = async (friendName, month, year) => {
    return getRequest('/friend/' + friendName + '/' + month + '/' + year);
}
  
export default {
    createTransaction,
    getRecentTransactions,
    getSelfTransaction,
    getFriendTransaction,
    thisMonthTransactions,
    DateOnly
};