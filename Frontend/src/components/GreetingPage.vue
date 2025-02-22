<script setup>
import InfoPage from './InfoPage.vue';

import router from '@/router';
import { useRoute } from 'vue-router';

const route = useRoute();

function goToGreetings() {
  router.push("/");
}

</script>

<template>
    <header>
        <h1 class="header-text" @click="goToGreetings">TimeFlow</h1>
        <div>
            <router-link class="button-redirect" :to="{ name: 'Register' }">Get Started!</router-link>
        </div>
    </header>
    <main>
        <Transition name="slide-left-right">
            <router-view class="block">
                <component :is="route.path === '/' ? InfoPage : route.matched[0]?.components.default" />
            </router-view>
        </Transition>
    </main>
</template>

<style scoped>
@import '@/assets/css/animations.css';

main {
    display: flex;
    justify-content: center;
    align-items: center;
}

header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
    background-color: #4437a3;
}

.button-redirect {
    cursor: pointer;
    border: 1px solid black;
    padding: 0.2rem;
    border-radius: 5px;
    background-color: #ffdd6b;
    color: black;
    text-wrap: bold;
    text-decoration: none;
}

.button-redirect:visited {
    color: black;
}

.header-text{
    cursor: pointer;
    color: #fe79cc;
}

.block {
    padding: auto;
    width: 90%;
    display: flex;
    flex-direction: column;
}

.info-block {
    width: 80%;
    padding: 1rem;
    margin: 1rem;
    border: 1px solid #4437a3;
    border-radius: 5px;
    background-color: #dcaaf4;
}
</style>