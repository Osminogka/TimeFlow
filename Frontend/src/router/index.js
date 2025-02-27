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
    {
        path: "/addfriend",
        name: "AddFriend",
        component: () => import("../components/AddFriend.vue"),
        meta:{
            title: "Add Friend",
            requireAuth: true
        }
    },
    {
        path: "/profile",
        name: "Profile",
        component: () => import("../components/ProfilePage.vue"),
        meta:{
            title: "Profile",
            requireAuth: true
        }
    },
    {
        path: "/friend",
        name: "Friend",
        component: () => import("../components/FriendProfile.vue"),
        meta:{
            title: "Friend",
            requireAuth: true
        }
    }
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