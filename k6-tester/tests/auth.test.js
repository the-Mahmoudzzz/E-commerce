// ============================================================
//  tests/auth.test.js — اختبار الـ Authentication endpoints
//  تشغيل: k6 run k6-tests/tests/auth.test.js
// ============================================================

import http from 'k6/http';
import { check, sleep } from 'k6';
import { BASE_URL, thresholds, TEST_CUSTOMER } from '../config.js';

export const options = {
  stages: [
    { duration: '20s', target: 10 },
    { duration: '40s', target: 20 },
    { duration: '20s', target: 0  },
  ],
  thresholds,
  insecureSkipTLSVerify: true,
};

const HEADERS = { 'Content-Type': 'application/json' };

export default function () {

  // ─── 1. Login صح ─────────────────────────────────────────
  const loginRes = http.post(
    `${BASE_URL}/api/account/login`,
    JSON.stringify({
      email:    TEST_CUSTOMER.email,
      password: TEST_CUSTOMER.password,
    }),
    { headers: HEADERS, insecureSkipTLSVerify: true }
  );

  const loginOk = check(loginRes, {
    'login: status 200':     (r) => r.status === 200,
    'login: has token':      (r) => {
      try {
        const b = JSON.parse(r.body);
        return !!(b.accessToken || (b.data && b.data.accessToken));
      } catch { return false; }
    },
    'login: response <500ms': (r) => r.timings.duration < 500,
  });

  if (!loginOk) {
    console.warn(`⚠️ Login check failed. Response: ${loginRes.body}`);
    return;
  }

  sleep(1);

  // ─── 2. Login بـ password غلط ────────────────────────────
  const wrongPassRes = http.post(
    `${BASE_URL}/api/account/login`,
    JSON.stringify({
      email:    TEST_CUSTOMER.email,
      password: 'WrongPassword999!',
    }),
    { headers: HEADERS, insecureSkipTLSVerify: true }
  );

  check(wrongPassRes, {
    'login wrong pass: status 400 or 401': (r) => r.status === 400 || r.status === 401,
  });

  sleep(1);

  // ─── 3. Forgot Password ──────────────────────────────────
  const forgotRes = http.post(
    `${BASE_URL}/api/account/forgot-password`,
    JSON.stringify({ email: TEST_CUSTOMER.email }),
    { headers: HEADERS, insecureSkipTLSVerify: true }
  );

  check(forgotRes, {
    'forgot-password: status 200 or 204': (r) => r.status === 200 || r.status === 204,
  });

  sleep(2);
}