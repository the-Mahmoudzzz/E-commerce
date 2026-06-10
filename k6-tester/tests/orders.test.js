// ============================================================
//  tests/orders.test.js — اختبار الـ Order endpoints
//  تشغيل: k6 run k6-tests/tests/orders.test.js
// ============================================================

import http from 'k6/http';
import { check, sleep, group } from 'k6';
import { BASE_URL, thresholds, TEST_CUSTOMER, TEST_SELLER, TEST_ADMIN,
         TEST_PRODUCT_ID, TEST_ADDRESS_ID, TEST_ZONE_ID } from '../config.js';
import { getAuthHeaders } from '../helpers/auth.js';

export const options = {
  stages: [
    { duration: '20s', target: 5  },
    { duration: '40s', target: 10 },
    { duration: '20s', target: 0  },
  ],
  thresholds,
  insecureSkipTLSVerify: true,
};

export default function () {

  // ─── Login ────────────────────────────────────────────────
  const customerHeaders = getAuthHeaders(TEST_CUSTOMER.email, TEST_CUSTOMER.password);
  if (!customerHeaders) {
    console.error('❌ Cannot get customer token, skipping order test');
    return;
  }

  // ─── 1. إضافة منتج للـ Cart ───────────────────────────────
  group('Cart Operations', () => {
    const addRes = http.post(
      `${BASE_URL}/api/shoppingcart/add-item?productId=${TEST_PRODUCT_ID}&quantity=1`,
      null,
      { headers: customerHeaders, insecureSkipTLSVerify: true }
    );

    check(addRes, {
      'add to cart: status 200':       (r) => r.status === 200,
      'add to cart: response <500ms':  (r) => r.timings.duration < 500,
    });

    sleep(1);

    // GET الـ Cart
    const cartRes = http.get(
      `${BASE_URL}/api/shoppingcart/basket`,
      { headers: customerHeaders, insecureSkipTLSVerify: true }
    );

    check(cartRes, {
      'get cart: status 200': (r) => r.status === 200,
    });

    sleep(1);
  });

  // ─── 2. إنشاء Order ──────────────────────────────────────
  group('Place Order', () => {
    const orderRes = http.post(
      `${BASE_URL}/api/order`,
      JSON.stringify({
        addressId:      TEST_ADDRESS_ID,
        shippingZoneId: TEST_ZONE_ID,
      }),
      { headers: customerHeaders, insecureSkipTLSVerify: true }
    );

    check(orderRes, {
      'create order: status 200 or 201': (r) => r.status === 200 || r.status === 201,
      'create order: response <1000ms':  (r) => r.timings.duration < 1000,
    });

    sleep(2);
  });

  // ─── 3. GET أوردرات الـ Customer ─────────────────────────
  group('Customer Orders', () => {
    const myOrdersRes = http.get(
      `${BASE_URL}/api/order/customer`,
      { headers: customerHeaders, insecureSkipTLSVerify: true }
    );

    check(myOrdersRes, {
      'my orders: status 200':       (r) => r.status === 200,
      'my orders: response <800ms':  (r) => r.timings.duration < 800,
    });

    sleep(1);
  });

  // ─── 4. GET أوردرات الـ Seller ───────────────────────────
  group('Seller Orders', () => {
    const sellerHeaders = getAuthHeaders(TEST_SELLER.email, TEST_SELLER.password);
    if (!sellerHeaders) return;

    const sellerOrdersRes = http.get(
      `${BASE_URL}/api/order/seller`,
      { headers: sellerHeaders, insecureSkipTLSVerify: true }
    );

    check(sellerOrdersRes, {
      'seller orders: status 200': (r) => r.status === 200,
    });

    sleep(1);
  });

  sleep(2);
}