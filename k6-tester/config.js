
export const BASE_URL = 'https://localhost:44323';
// شروط النجاح — لو أي شرط اتكسر الـ test هيفشل
export const thresholds = {
  http_req_duration: ['p(95)<800'],  // 95% من الـ requests تحت 800ms
  http_req_failed:   ['rate<0.05'],  // أقل من 5% errors
};
 
// بيانات تجريبية — غيّرها لبيانات موجودة عندك في الـ DB
export const TEST_CUSTOMER = {
  email:    'Customer@gmail.come.com',   // ← غيّر لـ email موجود
  password: 'String12*',        // ← غيّر للـ password الصح
};
 
export const TEST_SELLER = {
  email:    'Seller@gmail.come.com',     // ← غيّر لـ email موجود
  password: 'String12*',
};
 
export const TEST_ADMIN = {
  email:    'admin@test.com',      // ← غيّر لـ email موجود
  password: 'String12*',
};
 
// IDs موجودة في الـ DB — غيّرها بقيم حقيقية
export const TEST_PRODUCT_ID  = 9;      // ← id منتج موجود (شفناه في الـ response)
export const TEST_ADDRESS_ID  = 1;      // ← غيّر لـ address id حقيقي
export const TEST_ZONE_ID     = 1;      // ← غيّر لـ shipping zone id حقيقي
 
