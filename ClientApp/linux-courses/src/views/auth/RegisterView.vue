<template>
  <div class="grid h-[calc(100vh-120px)] place-items-center">
    <div class="card w-96 bg-base-200 shadow-xl">
      <div class="card-body">
        <h2 class="card-title self-center">Zarejestruj</h2>
        <Form @submit="handleRegister" :validation-schema="schema">
          <div class="form-control w-full max-w-xs">
            <Field
              name="username"
              type="text"
              placeholder="Nazwa użytkownika"
              class="input input-bordered w-full max-w-xs"
            />
            <ErrorMessage name="username" class="text-error" />
          </div>
          <div class="form-control w-full max-w-xs">
            <Field
              name="email"
              type="email"
              placeholder="Email"
              class="input input-bordered w-full max-w-xs"
            />
            <ErrorMessage name="email" class="text-error" />
          </div>
          <div class="form-control w-full max-w-xs">
            <Field
              name="password"
              type="password"
              placeholder="Hasło"
              class="input input-bordered w-full max-w-xs"
            />
            <ErrorMessage name="password" class="text-error" />
          </div>
          <div class="form-group">
            <button class="btn btn-primary btn-block" :disabled="loading">
              <span
                v-show="loading"
                class="spinner-border spinner-border-sm"
              ></span>
              <span>Zarejestruj się</span>
            </button>
          </div>
          <div class="form-group">
            <div v-if="message" class="alert alert-danger" role="alert">
              {{ message }}
            </div>
          </div>
        </Form>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { Form, Field, ErrorMessage } from "vee-validate";
import * as yup from "yup";
import { from } from "rxjs/internal/observable/from";

export default defineComponent({
  name: "RegisterView",
  components: {
    Form,
    Field,
    ErrorMessage,
  },
  data() {
    const schema = yup.object().shape({
      username: yup
        .string()
        .required("Username is required!")
        .min(3, "Must be at least 3 characters!")
        .max(20, "Must be maximum 20 characters!"),
      email: yup
        .string()
        .required("Email is required!")
        .email("Email is invalid!")
        .max(50, "Must be maximum 50 characters!"),
      password: yup
        .string()
        .required("Password is required!")
        .min(6, "Must be at least 6 characters!")
        .max(40, "Must be maximum 40 characters!"),
    });

    return {
      successful: false,
      loading: false,
      message: "",
      schema,
    };
  },
  computed: {
    loggedIn() {
      return this.$store.state.auth.status.loggedIn;
    },
  },
  updated() {
    if (this.loggedIn) {
      this.$router.push("/");
    }
  },
  methods: {
    handleRegister(user: any) {
      this.message = "";
      this.successful = false;
      this.loading = true;

      from(this.$store.dispatch("auth/register", user)).subscribe({
        next: (res) => {
          this.$router.push("login");
          alert("Rejestracja przebiegła pomyślnie.");
          this.message = res.message;
          this.successful = true;
          this.loading = false;
        },
        error: (err) => {
          this.message =
            (err.response && err.response.data && err.response.data.message) ||
            err.message ||
            err.toString();
          this.successful = false;
          this.loading = false;
        },
      });
    },
  },
});
</script>

<style scoped></style>
