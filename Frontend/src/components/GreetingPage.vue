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
    margin-top: 1em;
}

header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem 2rem;
    background: rgba(68, 55, 163, 0.8);
    backdrop-filter: blur(10px);
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    position: sticky;
    top: 0;
    width: auto;
    z-index: 100;
}

.header-text {
    cursor: pointer;
    font-size: 2rem;
    font-weight: bold;
    background: linear-gradient(90deg, #ff007f, #ff5e62);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    text-shadow: 2px 2px 6px rgba(255, 0, 127, 0.4);
    transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
}

.header-text:hover {
    transform: scale(1.08);
    opacity: 0.9;
}

.button-redirect {
    cursor: pointer;
    border: none;
    padding: 0.6rem 1rem;
    border-radius: 8px;
    background: linear-gradient(90deg, #ff007f, #ff5e62);
    font-weight: bold;
    color: white;
    text-decoration: none;
    transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
    display: inline-block;
    text-align: center;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.button-redirect:hover {
    transform: scale(1.08);
    opacity: 0.9;
    box-shadow: 0 6px 12px rgba(255, 0, 127, 0.4);
}

.button-redirect:visited {
    color: black;
}

.block {
    padding: auto;
    width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
}

@media (max-width: 768px) {
    header {
        padding: 1.3rem;
    }

    .button-redirect {
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 0.5em;
        height: 1.5em;
        text-align: center;
    }

}
</style>