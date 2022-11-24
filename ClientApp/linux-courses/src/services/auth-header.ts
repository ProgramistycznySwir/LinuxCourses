export default function authHeader(): { Authorization: string } | null {
  const user = JSON.parse(localStorage.getItem("user")!);

  if (user && user.token) {
    return { Authorization: "Bearer " + user.token };
  } else {
    return null;
  }
}
