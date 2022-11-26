import jwtDecode, { JwtPayload } from "jwt-decode";

export default interface JwtToken extends JwtPayload {
  role: string[];
}
