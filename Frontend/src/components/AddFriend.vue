<script setup>
import { watch, ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import usersAPi from '@/services/api/users';

import SocialEntity from '@/view/SocialEntity.vue';
import LoadingAnimation from '@/view/LoadingAnimation.vue';

const route = useRoute();
const router = useRouter();

const currentPage = ref(0);

const loading = ref(false);
const users = ref([]);
const error = ref(null);

const username = ref('');

async function loadUsers(){
  loading.value = true;
  try{
      let response = await usersAPi.getUsers(currentPage.value);
      console.log(response);
      if(response.success)
      {
          users.value = response.enum;
          error.value = "";
      }
      else
      {
          users.value = [];
          error.value = "Server error!";
      }
      loading.value = false;
  }
  catch(err) {
      error.value = err;
      loading.value = false;
  }
}

async function getCertainUser(){
  users.value = [];
  loading.value = true;
  try{
      let tempUser = await usersAPi.getUserByName(username.value)
      if(tempUser.success && tempUser.enum.length > 0)
          users.value.push(tempUser.enum[0]);
      loading.value = false;
  }
  catch(err){
      error.value = err;
      loading.value = false;
  }
}

onMounted(() =>{
  currentPage.value = parseInt(route.query.page) || 0;
  loadUsers();
});

watch(() => route.query.page, (page) => {
  currentPage.value = page;
  loadUsers();
});

function nextPage(){
  currentPage.value++;
  router.push({ query: { page: currentPage.value } });
}

function prevPage(){
  if(currentPage.value < 1) return;
  currentPage.value--;
  router.push({ query: { page: currentPage.value } });
}

function searchFriend(){
  if(username.value == '') 
      loadUsers();
  else
      getCertainUser();
}

function inviteIsSent(name){
  users.value = users.value.filter(user => user !== name);
}
</script>

<template>
  <div class="container">
    <div class="search-box">
      <input v-model="username" type="text" placeholder="Search users..." class="search-input" />
      <button class="search-btn" @click.prevent="searchFriend">
        <i class="fa fa-search"></i>
      </button>
    </div>

    <div class="friend-list">
      <loading-animation v-if="loading" />
      <div v-else-if="error" class="error">{{ error.message }}</div>
      <div class="entity-list" v-if="users.length > 0">
        <social-entity 
          v-for="(user, index) in users" 
          :key="index" 
          :name="user" 
          @friend-request="inviteIsSent" 
        />
      </div>
      <div v-else-if="!loading" class="no-results">
        <p>No users found</p>
      </div>
    </div>

    <div class="pagination" v-if="users.length > 3 || currentPage > 0">
      <button v-if="currentPage > 0" class="page-btn prev" @click="prevPage"></button>
      <button v-if="users.length > 3" class="page-btn next" @click="nextPage"></button>
    </div>
  </div>
</template>
  
<style scoped>
.container {
  width: 90%;
  max-width: 600px;
  margin: auto;
  padding: 20px;
  text-align: center;
  background: #f373b9;
  border-radius: 15px;
  box-shadow: 0 4px 15px #f373b9;
  color: white;
  margin-top: 1em;
}

.search-box {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  padding: 10px;
  background: #f848a9;
  margin: 1em;
  border-radius: 8px;
}

.search-input {
  flex: 1;
  padding: 10px;
  border: none;
  border-radius: 5px;
  font-size: 16px;
  outline: none;
  background: #f10688;
  color: white;
}

.search-btn {
  padding: 10px;
  border: none;
  border-radius: 5px;
  background: linear-gradient(90deg, #ff007f, #ff5e62);
  background-image: url('../assets/svgs/search.svg');
  color: white;
  cursor: pointer;
  transition: transform 0.2s;
}

.search-btn:hover {
  transform: scale(1.05);
}

.friend-list {
  margin-top: 20px;
  display: flex;
  flex-direction: column;
  justify-items: center;
}

.entity-list{
  display: flex;
  flex-direction: column;
  justify-items: center;
  align-items: center;
}

.no-results {
  color: #ff5e62;
}

.pagination {
  display: flex;
  align-items: center;
  width: 100%;
  gap: 10px;
  margin-top: 20px;
}

.page-btn {
  width: 40px;
  height: 40px;
  border: none;
  border-radius: 50%;
  background: linear-gradient(90deg, #ff007f, #ff5e62);
  cursor: pointer;
  transition: transform 0.2s;
  background-position: center;
  background-repeat: no-repeat;
}

.page-btn.prev {
  background-image: url('../assets/svgs/leftArrow.svg');
  justify-self: start;
  margin: auto;
}

.page-btn.next {
  background-image: url('../assets/svgs/rightArrow.svg');
  justify-self: end;
  margin: auto;
}

.page-btn:hover {
  transform: scale(1.1);
}

@media (max-width: 756px) {
  .container {
    padding: 1px;
    margin-top: 1em;
  }
}
</style>
