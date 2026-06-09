import http from 'k6/http';
import { BASE_URL } from '../config.js';

export function getAuthToken(email, password) {
  const res = http.post(
    `${BASE_URL}/api/account/login`,
    JSON.stringify({ email, password }),
    { headers: { 'Content-Type': 'application/json' } }
  );

  // لو فشل الـ login، وقف الـ test
  if (res.status !== 200) {
    console.error(`Login failed: ${res.status} — ${res.body}`);
    return null;
  }

  return JSON.parse(res.body).data.accessToken;
}