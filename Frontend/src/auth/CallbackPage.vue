<script setup>
import { register } from '@/auth/OAuthBackend';
import { saveToken } from '@/services/utils';

import { onMounted, ref } from "vue";
import { userManager } from "@/auth/oidc";
import { useRouter } from "vue-router";

const router = useRouter();
const errorMessage = ref('');

onMounted(async () => {
  try {
    // 🔥 THIS is the PKCE token exchange
    await userManager.signinRedirectCallback();

    // Optional: get tokens
    const user = await userManager.getUser();
    console.log("Access token:", user.access_token);

    var result = await register(user.access_token);
    if (result.success) {
            saveToken(result.message);
            router.push('/');
    } else {
        errorMessage.value = result.message;
    }

  } catch (e) {
    console.error("OIDC callback error", e);
  }
});
</script>

<template>
  <p>Signing you in…</p>
  <p class="error-message" v-if="errorMessage">{{ errorMessage }}</p>
</template>

<style scoped>
.error-message {
    color: #ff4d4d;
    font-weight: bold;
    margin-top: 0.8rem;
}
</style>