// ============================================================
//  helpers/auth.js — دالة بتعمل login وترجع الـ token
// ============================================================

import http from 'k6/http';
import { BASE_URL } from '../config.js';

/**
 * بتعمل login وترجع accessToken
 * لو فشلت بترجع null
 */
export function getAuthToken(email, password) {
  const res = http.post(
    `${BASE_URL}/api/account/login`,
    JSON.stringify({ email, password }),
    {
      headers: { 'Content-Type': 'application/json' },
      insecureSkipTLSVerify: true,
    }
  );

  if (res.status !== 200) {
    console.error(`❌ Login failed [${res.status}] for ${email}: ${res.body}`);
    return null;
  }

  try {
    const body = JSON.parse(res.body);
    const token = body.accessToken;

    if (!token) {
      console.error(`❌ No token found in response: ${res.body}`);
      return null;
    }

    return token;
  } catch (e) {
    console.error(`❌ Failed to parse login response: ${e}`);
    return null;
  }
}

/**
 * بترجع الـ Authorization header جاهز للاستخدام
 */
export function getAuthHeaders(email, password) {
  const token = getAuthToken(email, password);
  if (!token) return null;

  return {
    'Content-Type':  'application/json',
    'Authorization': `Bearer ${token}`,
  };
}