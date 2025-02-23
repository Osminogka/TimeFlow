import { getToken } from '@/services/utils';

const BASE_URL = '/api/user';
const token = getToken();

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

const getUsers = async (page) => {
    return getRequest('/get/' + page);
}

const chageVisisbility = async () => {
    return getRequest('/visibility');
}
  
export default {
    createTransaction,
    getSelfTransaction,
    getFriendTransaction
};