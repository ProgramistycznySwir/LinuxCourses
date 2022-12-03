<template>
  <!-- <div v-if="">

  </div> -->
  <div class="mx-40 my-10">
    <div class="card bg-base-200 w-full">
      <h1 class="text-3xl justify-start">Kategorie kursów:</h1>

      <div v-if="courseCategories_loading" class="justify-center">
        <div
          class="spinner-border animate-spin inline-block w-40 h-40 border-4 rounded-full text-primary m-20"
          role="status"
        >
          <span class="visually-hidden">Loading...</span>
        </div>
        <div class="text-primary text-2xl">Pobieram listę kursów z API...</div>
      </div>
      <div v-else class="w-fit">
        <div
          v-for="category in courseCategories"
          v-bind:key="category.id"
          class="my-5 bg-base-200 card px-10 py-2 text-xl"
        >
          <router-link
            :to="`/categories/${category.id}`"
            class="link-primary font-bold"
          >
            {{ category.name }} - Liczba kursów: {{ category.courseCount }}
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import axios from "axios-observable";

interface GetCoursesTreeModel {
  rootCategories: CourseCategory[];
}

interface CourseCategory {
  id: string;
  name: string;
  subCategories: CourseCategory[];
  courseCount: number;
}

export default defineComponent({
  name: "AllCategoriesView",
  data() {
    return {
      courseCategories: [] as CourseCategory[],
      courseCategories_loading: true,
    };
  },
  mounted() {
    axios
      .get<GetCoursesTreeModel>(
        `${process.env.BASE_URL}api/categories/GetCoursesTree`
      )
      .subscribe({
        next: (res) => {
          console.debug("WRYYYYYYYY", res.data);
          this.$data.courseCategories = res.data.rootCategories;
          this.$data.courseCategories_loading = false;
        },
        error: (err) => console.error(err),
      });
  },
});
</script>
