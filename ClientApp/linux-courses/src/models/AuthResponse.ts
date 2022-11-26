import jwtDecode, { JwtPayload } from "jwt-decode";

export default interface AuthResponse {
  userName: string;
  token: string;
}
