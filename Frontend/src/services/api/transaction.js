import { getToken } from '@/services/utils';

const BASE_URL = '/api/transactions';
const token = getToken();

const test_transactions = [
    { amount: 120, category: 'Food', description: 'Lunch at a restaurant', date: '2024-02-01' },
    { amount: 50, category: 'Transport', description: 'Bus ticket', date: '2024-02-02' },
    { amount: 80, category: 'Entertainment', description: 'Movie night', date: '2024-02-03' },
    { amount: 200, category: 'Shopping', description: 'New shoes', date: '2024-02-04' },
    { amount: 150, category: 'Bills', description: 'Electricity bill', date: '2024-02-05' },
    { amount: 90, category: 'Food', description: 'Groceries', date: '2024-02-06' },
    { amount: 30, category: 'Transport', description: 'Taxi ride', date: '2024-02-07' },
    { amount: 100, category: 'Health', description: 'Doctor appointment', date: '2024-02-08' },
    { amount: 60, category: 'Entertainment', description: 'Concert ticket', date: '2024-02-09' },
    { amount: 110, category: 'Food', description: 'Dinner with friends', date: '2024-02-10' },
    { amount: 75, category: 'Shopping', description: 'New t-shirt', date: '2024-02-11' },
    { amount: 200, category: 'Bills', description: 'Water bill', date: '2024-02-12' },
    { amount: 40, category: 'Transport', description: 'Gas for car', date: '2024-02-13' },
    { amount: 100, category: 'Entertainment', description: 'Video game purchase', date: '2024-02-14' },
    { amount: 50, category: 'Health', description: 'Gym membership', date: '2024-02-15' },
  ];
  

const postRequest = async (endpoint, data) => {
    const url = `${BASE_URL}${endpoint}`;
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Baerer ${token}`
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
            'Authorization': `Baerer ${token}`
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
    return getRequest('/self/' + month + '/' + year);
}

const getFriendTransaction = async (friendName, month, year) => {
    return getRequest('/friend/' + friendName + '/' + month + '/' + year);
}
  
export default {
    createTransaction,
    getSelfTransaction,
    getFriendTransaction,
    test_transactions
};