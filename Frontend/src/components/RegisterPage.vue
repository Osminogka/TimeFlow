<script setup>
import auth from '@/services/api/auth';

import { ref } from 'vue';
import { useRouter } from 'vue-router';

const username = ref('');
const email = ref('');
const password = ref('');
const errorMessage = ref([]);

const router = useRouter();

async function handleLogin() {
    try {
        let result = await auth.register(username.value, email.value, password.value);
        if (result.success) {
            router.push('/');
        } else {
            errorMessage.value.push(result.message);
        }
    } catch (error) {
        errorMessage.value.push(error);
    }
}

function verifyLogin() {
    errorMessage.value = [];

    const passwordRegex = {
        length: /.{6,}/,
        uppercase: /[A-Z]/,
        lowercase: /[a-z]/,
        digit: /\d/,
        nonAlphanumeric: /[^a-zA-Z0-9]/,
    };

    const usernameRegex = /^[a-zA-Z0-9_]+$/;

    if (!email.value || !password.value || !username.value) {
        errorMessage.value.push('Please fill in all fields.');
    }

    if (!email.value.includes('@') || !email.value.includes('.')) {
        errorMessage.value.push('Invalid email address.');
    }

    if (!usernameRegex.test(username.value)) {
        errorMessage.value.push('Username can only contain letters, numbers, and underscores.');
    }

    if (username.value.length < 4) {
        errorMessage.value.push('Username must be at least 4 characters.');
    }

    if (!passwordRegex.length.test(password.value)) {
        errorMessage.value.push('Password must be at least 6 characters.');
    }

    if (!passwordRegex.uppercase.test(password.value)) {
        errorMessage.value.push('Password must contain at least one uppercase letter.');
    }

    if (!passwordRegex.lowercase.test(password.value)) {
        errorMessage.value.push('Password must contain at least one lowercase letter.');
    }

    if (!passwordRegex.digit.test(password.value)) {
        errorMessage.value.push('Password must contain at least one digit.');
    }

    if (!passwordRegex.nonAlphanumeric.test(password.value)) {
        errorMessage.value.push('Password must contain at least one special character.');
    }

    if (errorMessage.value.length === 0) {
        handleLogin();
    }
}

</script>

<template>
    <div class="register-form">
        <h1>🔐 Register</h1>
        <form @submit.prevent="register">
            <input class="input-field" type="text" id="username" v-model="username" placeholder="Username" required>
            <input class="input-field" type="email" id="email" v-model="email" placeholder="Email" required>
            <input class="input-field" type="password" id="password" v-model="password" placeholder="Password" required>
            <button class="submit-button" type="submit" @click.prevent="verifyLogin()">Register</button>
        </form>
        <div v-if="errorMessage.length > 0">
            <p class="error-message" v-for="(error,key) in errorMessage" v-bind:key="key">{{ error }}</p>
        </div>
        <router-link class="button-redirect" :to="{ name: 'Login' }">Already have an account? Login here!</router-link>
    </div>
</template>

<style scoped>

.register-form {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 90%;
    max-width: 400px;
    padding: 0.5rem;
    border-radius: 12px;
    background: linear-gradient(135deg, #fc92d3, #f373b9);
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
    backdrop-filter: blur(8px);
    text-align: center;
    color: #fff;
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
}

.register-form:hover {
    transform: scale(1.02);
    box-shadow: 0 6px 20px rgba(0, 0, 0, 0.2);
}

h1 {
    color: #fff;
    font-size: 1.8rem;
    margin-bottom: 1rem;
}

.register-form form {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
}

.input-field {
    width: 90%;
    max-width: 300px;
    margin: 0.5rem 0;
    padding: 0.7rem;
    border-radius: 8px;
    border: none;
    outline: none;
    background-color: rgba(255, 255, 255, 0.3);
    color: #fff;
    font-size: 1rem;
    transition: background 0.3s ease-in-out, transform 0.1s ease-in-out;
}

.input-field::placeholder {
    color: rgba(255, 255, 255, 0.7);
}

.input-field:focus {
    background-color: rgba(255, 255, 255, 0.5);
    transform: scale(1.02);
}

.submit-button {
    cursor: pointer;
    border: none;
    padding: 0.7rem 1.2rem;
    border-radius: 8px;
    background-color: #ffdd6b;
    color: black;
    font-weight: bold;
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
    margin-top: 2rem;
    width: 60%;
    max-width: 200px;
}

.submit-button:hover {
    transform: scale(1.05);
    box-shadow: 0 3px 10px rgba(0, 0, 0, 0.2);
    background-color: #ffd43b;
}

.error-message {
    color: #ff4d4d;
    font-weight: bold;
    margin-top: 0.8rem;
}

.button-redirect {
    margin-top: 1em;
}

</style>
