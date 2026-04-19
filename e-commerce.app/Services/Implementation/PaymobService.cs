using e_commerce.app.Services.IServices;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
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
        var response = await _httpClient.PostAsJsonAsync(
            "https://accept.paymob.com/api/auth/tokens",
            new { api_key = _config["Paymob:ApiKey"] });

      
        var data = await response.Content.ReadFromJsonAsync<JsonElement>();

        return data.GetProperty("token").GetString();
    }

    public async Task<int> CreateOrder(string token, decimal amount)
    {
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

        var data = await response.Content.ReadFromJsonAsync<JsonElement>();
        return data.GetProperty("id").GetInt32();
    }

    public async Task<string> GetPaymentKey(string token, int orderId, decimal amount)
    {
        var integrationId = _config["Paymob:IntegrationId"];

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

   
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            
            throw new Exception($"Paymob Error: {errorContent}");
        }

        var data = await response.Content.ReadFromJsonAsync<JsonElement>();
        return data.GetProperty("token").GetString();
    }
}