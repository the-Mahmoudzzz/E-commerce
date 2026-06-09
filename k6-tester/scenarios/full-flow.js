import http from 'k6/http';
import { check, sleep, group } from 'k6';
import { BASE_URL, thresholds } from '../config.js';
import { getAuthToken } from '../helpers/auth.js';

export const options = {
  // سيناريوهات مختلفة في نفس الوقت
  scenarios: {
    // مستخدمين بيتصفحوا بس
    browsing_users: {
      executor:        'constant-vus',
      vus:             30,
      duration:        '2m',
      exec:            'browseProducts',
    },
    // مستخدمين بيعملوا orders
    buying_users: {
      executor:        'ramping-vus',
      startVUs:        0,
      stages: [
        { duration: '30s', target: 10 },
        { duration: '1m',  target: 20 },
        { duration: '30s', target: 0  },
      ],
      exec: 'placeOrder',
    },
  },
  thresholds,
};

// ============= SCENARIO 1: التصفح =============
export function browseProducts() {
  group('Browse Products', () => {
    const res = http.get(`${BASE_URL}/api/product`);
    check(res, { 'browse: status 200': (r) => r.status === 200 });
    sleep(2);
  });
}

// ============= SCENARIO 2: الشراء الكامل =============
export function placeOrder() {
  const headers = { 'Content-Type': 'application/json' };
  let authHeaders;

  // الـ Step 1: Login
  group('Login', () => {
    const token = getAuthToken('customer@test.com', 'Password123!');
    if (!token) return;
    authHeaders = {
      ...headers,
      Authorization: `Bearer ${token}`,
    };
  });

  if (!authHeaders) return;

  // الـ Step 2: إضافة منتج للـ Cart
  group('Add to Cart', () => {
    const res = http.post(
      `${BASE_URL}/api/shoppingcart/add-item?productId=PROD-ID&quantity=1`,
      null,
      { headers: authHeaders }
    );
    check(res, { 'add to cart: status 200': (r) => r.status === 200 });
    sleep(1);
  });

  // الـ Step 3: عمل Order
  group('Place Order', () => {
    const payload = JSON.stringify({
      addressId:      'ADDRESS-ID',
      shippingZoneId: 'ZONE-ID',
    });
    const res = http.post(
      `${BASE_URL}/api/order`,
      payload,
      { headers: authHeaders }
    );
    check(res, {
      'order created: status 201':   (r) => r.status === 201,
      'order has id':                (r) => JSON.parse(r.body).data.orderId !== undefined,
    });
    sleep(2);
  });
}