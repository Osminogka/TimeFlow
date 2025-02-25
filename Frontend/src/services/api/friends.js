import { getToken } from '@/services/utils';

const BASE_URL = '/api/friend';
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

const sendRequest = async (username) => {
    return getRequest('/get/' + username);
}

  
export default {
    sendRequest
};