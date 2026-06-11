using e_commerce.app.Dto.PayMentDTO;
using e_commerce.app.Services.IServices;
using e_commerce.core.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public class PaymobService : IPaymobService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public PaymobService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GetAuthToken()
    {
        var apiKey = _config["Paymob:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            throw new BusinessRuleException("Paymob API key is not configured.");

        var response = await _httpClient.PostAsJsonAsync(
            "https://accept.paymob.com/api/auth/tokens",
            new { api_key = apiKey });

        // ✅ فشل الـ request نفسه
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new BusinessRuleException($"Paymob authentication failed: {error}");
        }

        var data = await response.Content.ReadFromJsonAsync<JsonElement>();

        // ✅ الـ token مش موجود في الـ response
        if (!data.TryGetProperty("token", out var tokenProp) ||
             string.IsNullOrEmpty(tokenProp.GetString()))
            throw new BusinessRuleException("Paymob returned an invalid authentication response.");

        return tokenProp.GetString()!;
    }

    public async Task<int> CreateOrder(string token, decimal amount)
    {
        if (amount <= 0)
            throw new ValidationException("Amount", "Payment amount must be greater than zero.");

        var response = await _httpClient.PostAsJsonAsync(
            "https://accept.paymob.com/api/ecommerce/orders",
            new
            {
                auth_token = token,
                delivery_needed = "false",
                amount_cents = (int)(amount * 100),
                currency = "EGP",
                items = new object[] { }
            });

        // ✅ فشل إنشاء الأوردر في Paymob
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new BusinessRuleException($"Failed to create Paymob order: {error}");
        }

        var data = await response.Content.ReadFromJsonAsync<JsonElement>();

        if (!data.TryGetProperty("id", out var idProp))
            throw new BusinessRuleException("Paymob returned an invalid order response.");

        return idProp.GetInt32();
    }

    public async Task<string> GetPaymentKey(string token, int orderId, decimal amount)
    {
        var integrationId = _config["Paymob:IntegrationId"];
        if (string.IsNullOrEmpty(integrationId))
            throw new BusinessRuleException("Paymob Integration ID is not configured.");

        var response = await _httpClient.PostAsJsonAsync(
            "https://accept.paymob.com/api/acceptance/payment_keys",
            new
            {
                auth_token = token,
                amount_cents = (int)(amount * 100),
                expiration = 3600,
                order_id = orderId,
                currency = "EGP",
                integration_id = integrationId,
                billing_data = new
                {
                    email = "mahmoud.diab@example.com",
                    first_name = "Mahmoud",
                    last_name = "Diab",
                    phone_number = "+201000000000",
                    street = "Tahrir Street",
                    building = "10",
                    floor = "5",
                    apartment = "502",
                    city = "Cairo",
                    state = "Cairo",
                    country = "EG",
                    postal_code = "12345",
                    shipping_method = "PKG"
                }
            });

        // ✅ فشل الحصول على الـ payment key
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new BusinessRuleException($"Failed to generate Paymob payment key: {error}");
        }

        var data = await response.Content.ReadFromJsonAsync<JsonElement>();

        if (!data.TryGetProperty("token", out var tokenProp) ||
             string.IsNullOrEmpty(tokenProp.GetString()))
            throw new BusinessRuleException("Paymob returned an invalid payment key response.");

        return tokenProp.GetString()!;
    }
    public async Task< bool> ValidateHmac(PaymobCallbackDto callback, string receivedHmac)
    {
        var hmacSecret = _config["Paymob:PaymobHmacSecret"]; // من الـ appsettings
        var obj = callback.Obj;

        // 1. تجميع الحقول بالترتيب اللي Paymob طالباه (مهم جداً الترتيب ده)
        // لازم القيم تتحول لـ String. ولو boolean بتبقى "true" أو "false" حروف سمول
        var concatenatedString =
            obj.AmountCents.ToString() +
            obj.CreatedAt +
            obj.Currency +
            obj.ErrorOccured.ToString().ToLower() +
            obj.HasParentTransaction.ToString().ToLower() +
            obj.Id.ToString() +
            obj.IntegrationId.ToString() +
            obj.Is3dSecure.ToString().ToLower() +
            obj.IsAuth.ToString().ToLower() +
            obj.IsCapture.ToString().ToLower() +
            obj.IsRefunded.ToString().ToLower() +
            obj.IsStandalonePayment.ToString().ToLower() +
            obj.IsVoided.ToString().ToLower() +
            obj.Order.Id.ToString() +
            obj.Owner.ToString() +
            obj.Pending.ToString().ToLower() +
            obj.SourceData.Pan +
            obj.SourceData.SubType +
            obj.SourceData.Type +
            obj.Success.ToString().ToLower();

        // 2. عمل Hashing باستخدام HMACSHA512
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hmacSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));

        // 3. تحويل النتيجة لـ Hex String عشان نقارنها
        var calculatedHmac = BitConverter.ToString(hash).Replace("-", "").ToLower();

        // 4. مقارنة الـ HMAC المحسوب باللي مبعوت من Paymob
        return calculatedHmac == receivedHmac.ToLower();
    }

}