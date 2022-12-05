import AuthResponse from "@/models/AuthResponse";
import JwtToken from "@/models/JwtToken";
import jwtDecode from "jwt-decode";

class JwtService {
  roles(user: AuthResponse | null): string[] | null {
    const authRes = user as AuthResponse;
    if (authRes)
      return jwtDecode<JwtToken>(authRes.token)?.role;
    return null;
  }
}

export default new JwtService();
