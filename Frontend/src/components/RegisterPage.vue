<script setup>
import auth from '@/services/api/auth';

import { ref } from 'vue';
import { useRouter } from 'vue-router';

const username = ref('');
const email = ref('');
const password = ref('');
const errorMessage = ref('');

const router = useRouter();

async function handleLogin() {
    try {
        let result = await auth.register(username.value, email.value, password.value);
        if (result.success) {
            router.push('/');
        } else {
            errorMessage.value = result.message;
        }
    } catch (error) {
        errorMessage.value = error.message;
    }
}

function verifyLogin() {
    errorMessage.value = '';

    if (email.value === '' || password.value === '' || username.value === '') {
        errorMessage.value = 'Please fill in all fields';
    } else if (password.value.length < 8) {
        errorMessage.value = 'Password must be at least 8 characters';
    } else if (!email.value.includes('@') || !email.value.includes('.')) {
        errorMessage.value = 'Invalid email address';
    } else if (username.value.length < 4) {
        errorMessage.value = 'Username must be at least 4 characters';
    }

    if (errorMessage.value === '') {
        handleLogin();
    }
}

</script>

<template>
    <div class="register-form">
        <h1>Register</h1>
        <form @submit.prevent="register">
            <input class="input-field" type="text" id="username" v-model="username" placeholder="Username" required>
            <input class="input-field" type="email" id="email" v-model="email" placeholder="Email" required>
            <input class="input-field" type="password" id="password" v-model="password" placeholder="Password" required>
            <button class="submit-button" type="submit" @click.prevent="verifyLogin()">Register</button>
        </form>
        <p class="error-message">{{ errorMessage }}</p>
        <router-link class="button-redirect" :to="{ name: 'Login' }">Already have an account? Login here!</router-link>
    </div>
</template>

<style scoped>

.register-form{
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 80%;
    padding: 1rem;
    margin: 1rem;
    border: 1px solid #4437a3;
    border-radius: 5px;
    background-color: #fc92d3;
}

h1 {
    color: #cf1487;
}

.register-form form {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
}

.input-field{
    margin: 0.2rem;
    padding: 0.2rem;
    border-radius: 5px;
    border: 1px solid black;
    width: 80%;
    background-color: #dcaaf4;
}

.input-field:focus{
    outline: none;
}

.submit-button{
    display: block;
    cursor: pointer;
    border: 1px solid black;
    padding: 0.3rem;
    border-radius: 5px;
    background-color: #ffdd6b;
    color: black;
    text-wrap: bold;
    text-decoration: none;
    margin-top: 0.5rem;
    width: 40%;
}

.button-redirect {
    margin-top: 1rem;
    text-decoration: none;
    text-align: center;
}

.error-message{
    color: red;
}

@media (min-width: 768px) {
    .register-form{
        width: 40%;
    }

    .submit-button{
        width: 20%;
    }

    .input-field{
        width: 30%;
    }
}

</style>