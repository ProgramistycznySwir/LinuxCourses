<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <HelloWorld msg="Welcome to Your Vue.js + TypeScript App" />
  </div>
  <div></div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import HelloWorld from "@/components/HelloWorld.vue"; // @ is an alias to /src
import axios from "axios-observable";

export default defineComponent({
  name: "HomeView",
  components: {
    HelloWorld,
  },
  data() {
    return {
      forecasts: null,
    };
  },
  mounted() {
    axios.get<string>("https://localhost:8085/api/WeatherForecast/")
      .subscribe({
        next: (res) => console.log(res),
        error: (err) => console.error(err),
      });
  },
});
</script>
