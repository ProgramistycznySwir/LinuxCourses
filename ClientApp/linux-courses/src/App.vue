<template>
  <div>
    <nav class="navbar bg-primary text-primary-content justify-between">
      <div>
        <router-link class="btn btn-ghost normal-case text-xl" to="/">
          Panel główny
        </router-link>
        <router-link class="btn btn-ghost normal-case text-xl" to="/about">
          About
        </router-link>
      </div>
      <div v-if="currentUser" class="bold-case text-xl">
        {{ currentUser?.username }}
        <button
          class="btn btn-ghost normal-case text-xl"
          @click.prevent="logout"
        >
          Wyloguj
        </button>
      </div>
      <div v-else>
        <router-link class="btn btn-ghost normal-case text-xl" to="/auth/login">
          Zaloguj
        </router-link>
        <router-link
          class="btn btn-ghost normal-case text-xl"
          to="/auth/register"
        >
          Zarejestruj
        </router-link>
      </div>
    </nav>
    <router-view />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "App",
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  methods: {
    logout(): void {
      this.$store.dispatch("auth/logout");
      this.$router.push("/auth/login");
    },
  },
});
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;

  a {
    &.router-link-exact-active {
      color: theme("colors.secondary-content");
      background-color: theme("colors.secondary");
    }
  }
}
</style>
