<template>
  <!-- <div v-if="">

  </div> -->
  <div class="mx-40">
    <div v-if="loading" class="justify-center">
      <div
        class="spinner-border animate-spin inline-block w-40 h-40 border-4 rounded-full text-primary m-20"
        role="status"
      >
        <span class="visually-hidden">Loading...</span>
      </div>
      <div class="text-primary text-2xl">
        Pobieram informacje o kategorii z API...
      </div>
    </div>
    <div v-else class="w-fit">
      <div
        v-for="course in category.courses"
        v-bind:key="course.id"
        class="my-5 bg-primary card px-10 py-2 text-xl"
      >
        <router-link :to="`/categories/${course.id}`" class="card w-32 h-40">
          {{ course.name }}
        </router-link>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import axios from "axios-observable";

interface ResponseCourse {
  id: string;
  name: string;
}
interface ResponseCategory {
  id: string;
  name: string;
}
interface GetCategoryInfoModel {
  id: string;
  name: string;
  courses: ResponseCourse[];
  subCategories: ResponseCategory[];
}

export default defineComponent({
  name: "CategoryView",
  data() {
    return {
      category: {} as GetCategoryInfoModel,
      loading: true,
    };
  },
  computed: {
    categoryId() {
      return this.$route.params.id;
    },
  },
  mounted() {
    this.loading = true;
    axios
      .get<GetCategoryInfoModel>(
        `${process.env.BASE_URL}api/categories/ViewCategoryCourses/${this.categoryId}`
      )
      .subscribe({
        next: (res) => {
          this.category = res.data;
          this.loading = false;
        },
        error: (err) => console.error(err),
      });
  },
});
</script>
