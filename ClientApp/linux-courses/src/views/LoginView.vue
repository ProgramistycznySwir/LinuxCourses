<template>
  <div class="grid h-[calc(100vh-90px)] place-items-center">
    <div class="card w-96 bg-base-100 shadow-xl">
      <div class="card-body">
        <h2 class="card-title self-center">Login</h2>
        <Form @submit="handleLogin" :validation-schema="schema">
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
              <span>Login</span>
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
export default defineComponent({
  name: "LoginView",
  components: {
    Form,
    Field,
    ErrorMessage,
  },
  data() {
    const schema = yup.object().shape({
      username: yup.string().required("Username is required!"),
      password: yup.string().required("Password is required!"),
    });

    return {
      loading: false,
      message: "",
      schema,
    };
  },
  computed: {
    loggedIn(): boolean {
      return this.$store.state.auth.status.loggedIn;
    },
  },
  created() {
    if (this.loggedIn) {
      this.$router.push("/profile");
    }
  },
  methods: {
    handleLogin(user: any): void {
      this.loading = true;

      this.$store.dispatch("auth/login", user).then(
        () => {
          this.$router.push("/profile");
        },
        (error: any) => {
          this.loading = false;
          this.message =
            (error.response &&
              error.response.data &&
              error.response.data.message) ||
            error.message ||
            error.toString();
        }
      );
    },
  },
});
</script>

<style scoped>
div {
  margin: 10px;
}
</style>
