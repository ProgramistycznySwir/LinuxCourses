<template>
  <!-- <div v-if="">

  </div> -->
  <div class="mx-40">
    <div class="w-full bg-primary">
      RED
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import axios from "axios-observable";

export default defineComponent({
  name: "AllCoursesView",
  data() {
    return {
      forecasts: null,
    };
  },
  mounted() {
    axios.get<string>(`${process.env.BASE_URL}api/WeatherForecast/`).subscribe({
      next: (res) => console.log(res),
      error: (err) => console.error(err),
    });
  },
});
</script>
