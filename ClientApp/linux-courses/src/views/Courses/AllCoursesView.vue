<template>
  <!-- <div v-if="">

  </div> -->
  <div class="mx-40">
    <div class="w-full bg-primary">
      <div>
        <router-link to=""> Bez kategorii. </router-link>
        <div>Ilość kursów: {{}}</div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import axios from "axios-observable";

interface CourseCategory {
  Id: string;
  name: string;
  subCategories: CourseCategory[];
  CourseCount: number;
}

export default defineComponent({
  name: "AllCoursesView",
  data() {
    return {
      courseCategories: [] as CourseCategory[],
    };
  },
  mounted() {
    axios
      .get<CourseCategory[]>(
        `${process.env.BASE_URL}api/categories/GetCoursesTree`
      )
      .subscribe({
        next: (res) => (this.$data.courseCategories = res.data),
        error: (err) => console.error(err),
      });
  },
});
</script>
