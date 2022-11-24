// vuex.d.ts
import * as vuex from "vuex";
import AuthState from "./auth.module";

declare module "@vue/runtime-core" {
  // declare your own store states
  interface State {
    auth: AuthState;
  }

  // provide typings for `this.$store`
  interface ComponentCustomProperties {
    $store: vuex.Store<State>;
  }
}
