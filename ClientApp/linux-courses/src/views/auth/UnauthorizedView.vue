<template>
  <div class="grid h-[calc(100vh-120px)] place-items-center">
    <div>
      <h1 class="text-3xl">Nie posiadasz uprawnień do strony</h1>
      <button
        class="btn btn-primary btn-block btn-lg mt-4"
        @click="returnToPrev"
      >
        Wróć do poprzedniej strony
      </button>
      <br />
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
  data() {
    return {
      prevPage: "",
    };
  },
  methods: {
    returnToPrev() {
      this.$router.back();
    },
  },
});
</script>

<style scoped></style>
