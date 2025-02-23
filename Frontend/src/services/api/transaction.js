import { getToken } from '@/services/utils';

const BASE_URL = '/api/transactions';
const token = getToken();

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
    getFriendTransaction
};