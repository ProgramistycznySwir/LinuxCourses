<template>
  <!-- <div v-if="">

  </div> -->
  <div>
    
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import axios from "axios-observable";

export default defineComponent({
  name: "MyCoursesView",
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
