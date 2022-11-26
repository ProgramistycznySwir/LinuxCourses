import AuthResponse from "@/models/AuthResponse";
import JwtToken from "@/models/JwtToken";
import jwtDecode from "jwt-decode";

class JwtService {
  roles(user: AuthResponse): string[] {
    return jwtDecode<JwtToken>(user.token)?.role;
  }
}

export default new JwtService();
