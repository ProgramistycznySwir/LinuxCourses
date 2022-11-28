/* eslint-disable @typescript-eslint/ban-ts-comment */
import AuthResponse from "@/models/AuthResponse";
import Register_Request from "@/models/Register_Request";
import UserLogin_Request from "@/models/UserLogin_Request";
import AuthService from "../services/auth.service";

export default interface AuthState {
  status: { loggedIn: boolean };
  user: AuthResponse | null;
}

const user = JSON.parse(localStorage.getItem("user")!);
const initialState: AuthState = user
  ? { status: { loggedIn: true }, user }
  : { status: { loggedIn: false }, user: null };

export const auth = {
  namespaced: true,
  state: initialState,
  actions: {
    // @ts-ignore
    login({ commit }, user: UserLogin_Request) {
      AuthService.login(user).subscribe({
        next: (res) => {
          console.warn("Login success", res.data);
          commit("loginSuccess", res.data);
        },
        error: (err) => {
          console.warn("Login error", err);
          commit("loginFailure");
        },
      });
    },
    // @ts-ignore
    logout({ commit }) {
      AuthService.logout();
      commit("logout");
    },
    register(
      // @ts-ignore
      { commit },
      user: Register_Request
    ) {
      const result = AuthService.register(user);
      result.subscribe({
        next: (res) => {
          commit("registerSuccess");
        },
        error: (err) => {
          commit("registerFailure");
        },
      });
      return result;
    },
  },
  mutations: {
    loginSuccess(state: AuthState, user: AuthResponse) {
      state.status.loggedIn = true;
      state.user = user;
    },
    loginFailure(state: AuthState) {
      state.status.loggedIn = false;
      state.user = null;
    },
    logout(state: AuthState) {
      state.status.loggedIn = false;
      state.user = null;
    },
    registerSuccess(state: AuthState) {
      state.status.loggedIn = false;
    },
    registerFailure(state: AuthState) {
      state.status.loggedIn = false;
    },
  },
};
