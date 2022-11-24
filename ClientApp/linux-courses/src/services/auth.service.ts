import AuthResponse from "@/models/AuthResponse";
import FailedAuthResponse from "@/models/FailedAuthResponse";
import axios, { AxiosObservable } from "axios-observable";
import { Observable } from "rxjs";

const API_URL = "http://localhost:8085/api/Auth/";

class AuthService {
  login(user: {
    username: string;
    password: string;
  }): AxiosObservable<AuthResponse | FailedAuthResponse> {
    const result = axios.post<AuthResponse | FailedAuthResponse>(API_URL, {
      username: user.username,
      password: user.password,
    });
    result.subscribe({
      next: (res) => {
        const authRes = res.data as AuthResponse;
        if (authRes)
          localStorage.setItem("user", JSON.stringify(authRes.token));

        return res.data;
      },
      error: (err) => {
        alert("Nie udało się zalogować");
      },
    });
    return result;
  }

  logout() {
    localStorage.removeItem("user");
  }

  register(user: { username: string; email: string; password: string }) {
    return axios.post(API_URL + "signup", {
      username: user.username,
      email: user.email,
      password: user.password,
    });
  }
}

export default new AuthService();
