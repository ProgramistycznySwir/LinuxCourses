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
      const result = AuthService.login(user);
      result.subscribe({
        next: (res) => {
          commit("loginSuccess", user);
        },
        error: (err) => {
          commit("loginFailure");
        },
      });
      return result;
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
    loginSuccess(state: any, user: AuthResponse) {
      state.status.loggedIn = true;
      state.user = user;
    },
    loginFailure(state: any) {
      state.status.loggedIn = false;
      state.user = null;
    },
    logout(state: any) {
      state.status.loggedIn = false;
      state.user = null;
    },
    registerSuccess(state: any) {
      state.status.loggedIn = false;
    },
    registerFailure(state: any) {
      state.status.loggedIn = false;
    },
  },
};
