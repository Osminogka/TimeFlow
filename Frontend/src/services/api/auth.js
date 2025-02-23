const BASE_URL = '/api/authentication';

import { saveToken } from '../utils';

const postRequest = async (endpoint, data) => {
    const url = `${BASE_URL}${endpoint}`;
    const response = await fetch(url, {
        method: 'POST',
        headers: {
        'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'API request failed');
    }

    return response.json();
};


const login = async (email, password) => {
    const result = await postRequest('/login', { email, password });
    if (result.success)
        saveToken(result.message);
    return result;
};

const register = async (name, email, password) => {
    const result = await postRequest('/register', { name, email, password });
    if (result.success) 
        saveToken(result.message);
    return result;
};
  
export default {
    login,
    register,
};