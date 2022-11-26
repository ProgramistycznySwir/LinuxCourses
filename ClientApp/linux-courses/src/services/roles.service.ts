import AuthResponse from "@/models/AuthResponse";
import FailedAuthResponse from "@/models/FailedAuthResponse";
import Register_Request from "@/models/Register_Request";
import UserLogin_Request from "@/models/UserLogin_Request";
import axios, { AxiosObservable } from "axios-observable";
import { Observable, tap } from "rxjs";

const API_URL = `${process.env.BASE_URL}api/Auth/`;

class RolesService {
  login(
    user: UserLogin_Request
  ): AxiosObservable<AuthResponse | FailedAuthResponse> {
    const result = axios
      .post<AuthResponse | FailedAuthResponse>(`${API_URL}Login`, user)
      .pipe(tap(console.info));
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
}

export default new RolesService();
