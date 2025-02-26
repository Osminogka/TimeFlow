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
    console.log(page);
    let response = {
        success: true,
        friendList: [
            'Alice',
            'Bob',
            'Charlie',
            'David',
            'Eve',
            'Frank',
            'Grace',
            'Heidi',
            'Ivan',
            'Judy',
            'Kevin',
        ]
    }
    return response;
    
}

const getCertainUser = async (username) => {
    console.log(username);
    let response = {
        success: true,
        friendList: [
            'Alice',
        ]
    }
    return response;
}

const chageVisisbility = async () => {
    return getRequest('/visibility');
}
  
export default {
    getUsers,
    getCertainUser,
    chageVisisbility,
};