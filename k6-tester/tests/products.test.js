// ============================================================
//  tests/products.test.js — اختبار الـ Product endpoints
//  تشغيل: k6 run k6-tests/tests/products.test.js
// ============================================================

import http from 'k6/http';
import { check, sleep } from 'k6';
import { BASE_URL, thresholds, TEST_PRODUCT_ID, TEST_SELLER, TEST_ADMIN } from '../config.js';
import { getAuthHeaders } from '../helpers/auth.js';

export const options = {
  stages: [
    { duration: '30s', target: 20 },
    { duration: '1m',  target: 50 },
    { duration: '30s', target: 0  },
  ],
  thresholds,
  insecureSkipTLSVerify: true,
};

export default function () {

  // ─── 1. GET كل المنتجات (Public) ─────────────────────────
  const listRes = http.get(
    `${BASE_URL}/api/product`,
    { insecureSkipTLSVerify: true }
  );

  check(listRes, {
    'products list: status 200':        (r) => r.status === 200,
    'products list: is array':          (r) => {
      try { return Array.isArray(JSON.parse(r.body)); }
      catch { return false; }
    },
    'products list: not empty':         (r) => {
      try { return JSON.parse(r.body).length > 0; }
      catch { return false; }
    },
    'products list: has id field':      (r) => {
      try { return JSON.parse(r.body)[0].id !== undefined; }
      catch { return false; }
    },
    'products list: response <800ms':   (r) => r.timings.duration < 800,
  });

  sleep(1);

  // ─── 2. GET منتج بـ ID معين (Public) ─────────────────────
  const singleRes = http.get(
    `${BASE_URL}/api/product/${TEST_PRODUCT_ID}`,
    { insecureSkipTLSVerify: true }
  );

  check(singleRes, {
    'single product: status 200':     (r) => r.status === 200,
    'single product: correct id':     (r) => {
      try {
        const b = JSON.parse(r.body);
        // بيرجع object مباشرة أو { data: {...} }
        const prod = b.id ? b : b.data;
        return prod && prod.id === TEST_PRODUCT_ID;
      } catch { return false; }
    },
    'single product: has price':      (r) => {
      try {
        const b = JSON.parse(r.body);
        const prod = b.id ? b : b.data;
        return prod && prod.price > 0;
      } catch { return false; }
    },
    'single product: response <500ms': (r) => r.timings.duration < 500,
  });

  sleep(1);

  // ─── 3. GET منتج بـ ID مش موجود ──────────────────────────
  const notFoundRes = http.get(
    `${BASE_URL}/api/product/99999`,
    { insecureSkipTLSVerify: true }
  );

  check(notFoundRes, {
    'product not found: status 404': (r) => r.status === 404,
  });

  sleep(1);

  // ─── 4. POST إنشاء منتج (Seller) ─────────────────────────
  const sellerHeaders = getAuthHeaders(TEST_SELLER.email, TEST_SELLER.password);

  if (sellerHeaders) {
    const createRes = http.post(
      `${BASE_URL}/api/product`,
      JSON.stringify({
        name:        `Test Product ${Date.now()}`,
        description: 'k6 load test product',
        price:       99.99,
        quantity:    10,
        categoryId:  1,   // ← غيّر لـ category id موجود
        imageUrl:    'https://example.com/image.png',
      }),
      { headers: sellerHeaders, insecureSkipTLSVerify: true }
    );

    check(createRes, {
      'create product: status 200 or 201': (r) => r.status === 200 || r.status === 201,
      'create product: response <1000ms':  (r) => r.timings.duration < 1000,
    });

    sleep(1);
  }

  // ─── 5. Check Stock (Admin/Seller) ───────────────────────
  const adminHeaders = getAuthHeaders(TEST_ADMIN.email, TEST_ADMIN.password);

  if (adminHeaders) {
    const stockRes = http.get(
      `${BASE_URL}/api/product/${TEST_PRODUCT_ID}/stock`,
      { headers: adminHeaders, insecureSkipTLSVerify: true }
    );

    check(stockRes, {
      'stock check: status 200': (r) => r.status === 200,
    });

    sleep(1);

    // ─── 6. Low Stock Alert ───────────────────────────────
    const lowStockRes = http.get(
      `${BASE_URL}/api/product/low-stock?threshold=5`,
      { headers: adminHeaders, insecureSkipTLSVerify: true }
    );

    check(lowStockRes, {
      'low stock: status 200': (r) => r.status === 200,
    });
  }

  sleep(2);
}