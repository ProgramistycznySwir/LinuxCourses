<template>
  <div class="home">
    <AllCoursesView />
    <!-- <HelloWorld msg="Welcome to Your Vue.js + TypeScript App" /> -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import HelloWorld from "@/components/HelloWorld.vue"; // @ is an alias to /src
import axios from "axios-observable";
import AllCoursesView from "./Courses/AllCategoriesView.vue";

export default defineComponent({
  name: "HomeView",
  components: {
    // HelloWorld,
    AllCoursesView,
  },
  data() {
    return {
      forecasts: null,
    };
  },
  mounted() {},
});
</script>
