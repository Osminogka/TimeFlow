import { getToken } from '@/services/utils';

const BASE_URL = '/api/friends';
const token = getToken();

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

const sendRequest = async (username) => {
    return getRequest('/send/' + username);
}

const getFriendRequest = async () => {
    return getRequest('/requests');
}

const getFriendList = async () => {
    return getRequest('/list');
}

const acceptRequest = async (username) => {
    return getRequest('/accept/' + username);
}

const rejectRequest = async (username) => {
    return getRequest('/reject/' + username);
}
  
export default {
    sendRequest,
    getFriendRequest,
    getFriendList,
    acceptRequest,
    rejectRequest,
};