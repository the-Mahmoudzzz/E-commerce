export const BASE_URL = 'https://localhost:5001';

// الـ thresholds — شرط النجاح والفشل
export const thresholds = {
  http_req_duration: ['p(95)<500'],  // 95% من الـ requests أقل من 500ms
  http_req_failed:   ['rate<0.01'],  // أقل من 1% errors
};