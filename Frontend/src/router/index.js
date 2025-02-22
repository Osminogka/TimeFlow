import { createRouter, createWebHistory } from "vue-router";

import authApi from "@/services/utils";

const routes = [
    {
        path: "/login",
        name: "Login",
        component: () => import("../components/LoginPage.vue"),
        meta:{
            title: "Login",
            requireAuth: false
        }
    },
    {
        path: "/register",
        name: "Register",
        component: () => import("../components/RegisterPage.vue"),
        meta:{
            title: "Register",
            requireAuth: false
        }
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach((to) => {
    const loggedIn = authApi.getCurrentUser();
    
    if(to.meta.requireAuth && !loggedIn){
        return {name: "Login", query: {redirect: to.fullPath}};
    }
});

router.beforeEach((to, from, next) => {
    document.title = to.meta.title || 'TimeFlow';
    next();
});

export default router;