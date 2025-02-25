<script setup>
import { defineProps, defineEmits } from 'vue';

import { sendRequest } from '@/services/api/friends';

const props = defineProps({
    name: String,
});

const emits = defineEmits(['friend-request', 'group-enter']);

async function friendRequest(){
    let response = await sendRequest(props.name);
    if(response.success)
        emits('friend-request',props.name);
}

</script>

<template>
    <div class="social-entity-container">
        <p class="name-display">{{ name }}</p>
        <button v-if="type === 'friend'" @click="friendRequest" class="invite-friend-button custom-button"></button>
    </div>
</template>

<style scoped>

.social-entity-container{
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    border: 1px solid black;
    border-radius: 5px;
    margin: 10px;
    width: 20em;
    padding: 10px;
}

.name-display{
    align-self: flex-start;
}

.invite-friend-button{
    background-image: url('../assets/svgs/addfriend.svg');
    background-color: white;
}

.custom-button{
    background-position: center;
    background-size:contain;
    background-repeat: no-repeat;
    border: none;
    width: 2em;
    height: 2em;
    cursor: pointer;
}

@media (max-width: 600px){
    .social-entity-container{
        width: 10em;
    }
}
</style>