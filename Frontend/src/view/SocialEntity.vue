<script setup>
import { defineProps, defineEmits } from 'vue';

import frinedsApi from '@/services/api/friends';

const props = defineProps({
    name: String,
});

const emits = defineEmits(['friend-request', 'group-enter']);

async function friendRequest(){
    try{
        let response = await frinedsApi.sendRequest(props.name);
        if(response.success)
            emits('friend-request',props.name);
    }
    catch(err){
        console.log(err);
    }
}

</script>

<template>
    <div class="social-entity-container">
        <p class="name-display">
            {{ name }} 
            <span class="crone" v-if="name === 'Osminogka'">👑</span>
        </p>
        <button @click="friendRequest" class="invite-friend-button custom-button"></button>
    </div>
</template>

<style scoped>

.social-entity-container {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    border: 2px solid #4437a3;
    border-radius: 8px;
    margin: 10px;
    width: 22em;
    padding: 12px;
    background: linear-gradient(90deg, #fc92d3, #dcaaf4);
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.15);
    transition: transform 0.2s ease-in-out;
}

.social-entity-container:hover {
    transform: scale(1.03);
}

.name-display {
    font-size: 1.1rem;
    font-weight: bold;
    color: #4437a3;
}

.invite-friend-button {
    background-image: url('../assets/svgs/addfriend.svg');
    background-color: transparent;
}

.custom-button {
    background-position: center;
    background-size: contain;
    background-repeat: no-repeat;
    border: none;
    width: 2.2em;
    height: 2.2em;
    cursor: pointer;
    transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
}

.custom-button:hover {
    transform: scale(1.1);
    opacity: 0.8;
}

@media (max-width: 600px) {
    .social-entity-container {
        width: 14em;
    }

    .custom-button {
        width: 1.8em;
        height: 1.8em;
    }
}

.crone{
    font-size: 1.2em;
}

</style>