<script setup>

</script>

<template>
    <div>
        <h1>Register</h1>
        <form @submit.prevent="register">
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="username" required>
        <label for="password">Password:</label>
        <input type="password" id="password" v-model="password" required>
        <button type="submit">Register</button>
        </form>
    </div>
</template>