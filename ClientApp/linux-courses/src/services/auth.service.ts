import AuthResponse from "@/models/AuthResponse";
import FailedAuthResponse from "@/models/FailedAuthResponse";
import Register_Request from "@/models/Register_Request";
import UserLogin_Request from "@/models/UserLogin_Request";
import axios, { AxiosObservable } from "axios-observable";
import { tap } from "rxjs";

const API_URL = `${process.env.BASE_URL}api/Auth/`;

class AuthService {
  login(
    user: UserLogin_Request
  ): AxiosObservable<AuthResponse | FailedAuthResponse> {
    return axios
      .post<AuthResponse | FailedAuthResponse>(`${API_URL}Login`, user)
      .pipe(
        tap(console.info),
        tap({
          next: (res) => {
            const authRes = res.data as AuthResponse;
            console.log(authRes);
            if (authRes) localStorage.setItem("user", JSON.stringify(authRes));
          },
          error: (err) => {
            alert("Nie udało się zalogować");
          },
        })
      );
  }

  logout() {
    localStorage.removeItem("user");
  }

  register(user: Register_Request) {
    return axios
      .post(`${API_URL}Register`, user)
      .pipe(tap((res) => console.log("Registers", res)));
  }
}

export default new AuthService();
