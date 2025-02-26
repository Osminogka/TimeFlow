import { ref } from 'vue';

export const user = ref({
  name: '',
  email: '',
}); 

const TOKEN_KEY = 'jwt';

export const saveToken = (token) => {
  localStorage.setItem(TOKEN_KEY, token);
};

export const getToken = () => {
  return localStorage.getItem(TOKEN_KEY);
};


export const isAuthenticated = () => {
  getCurrentUser();
  return user.value.name !== '';
};

export function getCurrentUser() {
  const token = localStorage.getItem(TOKEN_KEY);
  if (!token) {
      return null;
  }

  const parts = token.split('.');
  if (parts.length !== 3) {
      return null;
  }

  const decoded = atob(parts[1]);
  const payload = JSON.parse(decoded);

  console.log(payload);

  user.value.name = payload.unique_name;
  user.value.email = payload.email;
  return payload;
}

export const clearToken = () => {
  localStorage.removeItem(TOKEN_KEY);
};

export default {
  user,
  saveToken,
  getToken,
  isAuthenticated,
  clearToken,
  getCurrentUser,
};