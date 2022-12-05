<template>
  <div class="grid h-[calc(100vh-120px)] place-items-center">
    <div>
      <h1 class="text-3xl">Nie znaleziono strony {{ prevRoute }}</h1>
      <router-link class="btn btn-primary btn-block btn-lg mt-4" to="/">
        Wróć do strony głównej
      </router-link>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "UnauthorizedView",
  methods: {},
  computed: {
    prevRoute() {
      return this.$route.redirectedFrom;
    },
  },
  // beforeRouteEnter(to, from, next) {
  //   next((vm) => {
  //     vm.$data.prevPage = from;
  //   });
  // },
});
</script>

<style scoped></style>
