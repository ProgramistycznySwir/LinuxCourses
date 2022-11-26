<template>
  <div class="grid h-[calc(100vh-120px)] place-items-center">
    <div class="card w-96 bg-base-200 shadow-xl">
      <div class="card-body">
        <h2 class="card-title self-center">Zaloguj</h2>
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
              <span>Zaloguj się</span>
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
      this.$router.back();
    }
  },
  methods: {
    handleLogin(user: any): void {
      this.loading = true;

      from(this.$store.dispatch("auth/login", user)).subscribe({
        next: (res) => this.$router.back(),
        error: (err) => {
          this.loading = false;
          this.message =
            (err.response && err.response.data && err.response.data.message) ||
            err.message ||
            err.toString();
        },
      });
    },
  },
});
</script>

<style scoped>
div {
  margin: 10px;
}
</style>
