using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json.Nodes;
using System.Net.Http;
using System.Threading.Tasks;

namespace garage.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly string _paypalClientId;
        private readonly string _paypalClientSecret;
        private readonly string _paypalUrl;

        public CheckoutController(IConfiguration configuration, ILogger<CheckoutController> logger)
        {
            _paypalClientId = configuration["PayPalSettings:ClientId"];
            _paypalClientSecret = configuration["PayPalSettings:ClientSecret"];
            _paypalUrl = configuration["PayPalSettings:BaseUrl"];
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.PaypalClientId = _paypalClientId;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if (totalAmount == null)
            {
                _logger.LogError("Total amount is null");
                return new JsonResult(new { id = "" });
            }

            JsonObject createOrderRequest = new JsonObject
            {
                ["intent"] = "CAPTURE",
                ["purchase_units"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["amount"] = new JsonObject
                        {
                            ["currency_code"] = "MAD",
                            ["value"] = totalAmount
                        }
                    }
                }
            };

            string accessToken = await GetPaypalAccessToken();
            string url = $"{_paypalUrl}/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(createOrderRequest.ToString(), Encoding.UTF8, "application/json")
                };

                var httpResponse = await client.SendAsync(requestMessage);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("PayPal order creation response: {ResponseContent}", responseContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var jsonResponse = JsonNode.Parse(responseContent);

                    if (jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                        return new JsonResult(new { id = paypalOrderId });
                    }
                }
                else
                {
                    _logger.LogError("PayPal order creation failed: {StatusCode} - {ResponseContent}", httpResponse.StatusCode, responseContent);
                }
            }

            return new JsonResult(new { id = "" });
        }

        [HttpPost]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data?["orderId"]?.ToString();
            if (orderId == null)
            {
                _logger.LogError("Order ID is null");
                return new JsonResult(new { success = false });
            }
            string accessToken = await GetPaypalAccessToken();
            string url = $"{_paypalUrl}/v2/checkout/orders/{orderId}/capture";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                };
                var httpResponse = await client.SendAsync(requestMessage);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("PayPal order capture response: {ResponseContent}", responseContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return new JsonResult(new { success = true });
                }
                else
                {
                    _logger.LogError("PayPal order capture failed: {StatusCode} - {ResponseContent}", httpResponse.StatusCode, responseContent);
                }
            }
            return new JsonResult(new { success = false });
        }

        private async Task<string> GetPaypalAccessToken()
        {
            string url = $"{_paypalUrl}/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_paypalClientId}:{_paypalClientSecret}"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var httpResponse = await client.SendAsync(requestMessage);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("PayPal access token response: {ResponseContent}", responseContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var jsonResponse = JsonNode.Parse(responseContent);
                    return jsonResponse?["access_token"]?.ToString() ?? "";
                }
                else
                {
                    _logger.LogError("Failed to get PayPal access token: {StatusCode} - {ResponseContent}", httpResponse.StatusCode, responseContent);
                }
            }

            return "";
        }
    }
}
