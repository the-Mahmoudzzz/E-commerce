import http from 'k6/http';
import { check, sleep } from 'k6';
import { BASE_URL, thresholds } from '../config.js';

// إعدادات الـ load
export const options = {
  stages: [
    { duration: '30s', target: 20  }, // ابدأ من 0 لـ 20 user
    { duration: '1m',  target: 50  }, // ارفع لـ 50 user
    { duration: '30s', target: 0   }, // نزّل تاني
  ],
  thresholds,
};

export default function () {
  // GET كل المنتجات
  const listRes = http.get(`${BASE_URL}/api/product`);
  check(listRes, {
    'products list: status 200':       (r) => r.status === 200,
    'products list: has data':         (r) => JSON.parse(r.body).data.length > 0,
    'products list: response < 500ms': (r) => r.timings.duration < 500,
  });

  sleep(1); // استنى ثانية زي الـ real user

  // GET منتج معين
  const productRes = http.get(`${BASE_URL}/api/product/PRODUCT-ID-HERE`);
  check(productRes, {
    'single product: status 200': (r) => r.status === 200,
  });

  sleep(1);
}