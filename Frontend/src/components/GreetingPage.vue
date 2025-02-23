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
    padding: 0.3rem;
    border-radius: 5px;
    background: linear-gradient(90deg, #ff007f, #ff5e62);
    font-weight: bold;
    text-decoration: none;
    transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
    display: inline-block;
    text-align: center;
}

.button-redirect:hover {
    transform: scale(1.05);
    opacity: 0.9;
}

.button-redirect:visited {
    color: black;
}

.header-text {
    cursor: pointer;
    font-size: 2rem;
    font-weight: bold;
    background: linear-gradient(90deg, #ff007f, #ff5e62);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
}

.header-text:hover {
    transform: scale(1.05);
    opacity: 0.9;
}

.block {
    padding: auto;
    width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin: 0;
    padding: 0;
}
</style>