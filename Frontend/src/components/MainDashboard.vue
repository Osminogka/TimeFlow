<script setup>
import authApi from '@/services/utils'

function logout(){
    authApi.clearToken();
    window.location.href = '/';
}

</script>

<template>
    <div>
        <h1>Dashboard</h1>
        <p>Here is the dashboard</p>
    </div>
    <button @click.prevent="logout()">Logout</button>
</template>