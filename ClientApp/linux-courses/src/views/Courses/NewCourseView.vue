<template>
  <!-- <div v-if="">

  </div> -->
  <div class="mx-40">
    <h1 class="justify-start">Utwórz nowy kurs:</h1>
    <Form @submit="submit" :validation-schema="schema">
      <div class="form-control">
        <label class="label"> Nazwa kursu: </label>
        <Field
          name="name"
          type="text"
          placeholder="Nazwa kursu"
          class="input input-bordered"
        />
        <ErrorMessage name="name" class="text-error" />
      </div>
      <div class="form-control">
        <label class="label"> Opis kursu: </label>
        <Field
          as="textarea"
          name="description"
          placeholder="Opis kursu"
          class="textarea textarea-bordered"
        />
        <ErrorMessage name="description" class="text-error" />
      </div>
      <br />
      <div class="form-group">
        <button class="btn btn-primary" :disabled="loading">
          <span
            v-show="loading"
            class="spinner-border spinner-border-sm"
          ></span>
          <span>Dodaj kurs</span>
        </button>
      </div>
      <div class="form-group">
        <div v-if="message" class="alert alert-danger" role="alert">
          {{ message }}
        </div>
      </div>
    </Form>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { Form, Field, ErrorMessage } from "vee-validate";
import axios from "axios-observable";
import * as yup from "yup";
import JwtService from "@/services/jwt.service";
import AuthRoles from "@/models/AuthRoles";
import authHeader from "@/services/auth-header";

interface CreateCourseCommand {
  name: string;
  description: string;
  categoryId: string;
}

export default defineComponent({
  name: "NewCourseView",
  components: {
    Form,
    Field,
    ErrorMessage,
  },
  data() {
    const schema = yup.object().shape({
      name: yup
        .string()
        .required("Wymagana nazwa!")
        .min(3, "Nazwa musi być dłuższa niż 3 znaki")
        .max(64, "Nazwa musi nie przekraczać 64 znaków"),
      description: yup.string().max(256, "Musi nie przekraczać 256 znaków"),
    });

    return {
      loading: false,
      message: "",
      schema,
    };
  },
  computed: {
    categoryId() {
      console.log("ID", this.$route.params.categoryId)
      return this.$route.params.categoryId as string;
    },
    userRoles() {
      return JwtService.roles(this.$store.state.auth.user);
    },
    // TODO: Move consts to plugin.
    AUTH_ROLES() {
      return AuthRoles;
    },
  },
  mounted() {
    if (this.userRoles?.includes(this.AUTH_ROLES.CAN_CREATE_COURSES) != true)
      this.$router.push("/auth/unauthorized");
  },
  methods: {
    submit(course: any) {
      this.loading = true;
      course.categoryId = this.categoryId;
      console.log(course);
      axios
        .post<CreateCourseCommand>(
          `${process.env.BASE_URL}api/courses/AddCourse`,
          course,
          { headers: authHeader() }
        )
        .subscribe({
          next: (res) => {
            // TODO to the new course!
            this.$router.push("/");
            this.loading = false;
          },
          error: (err) => {
            this.loading = false;
            this.message =
              (err.response &&
                err.response.data &&
                err.response.data.message) ||
              err.message ||
              err.toString();
          },
        });
    },
  },
});
</script>
