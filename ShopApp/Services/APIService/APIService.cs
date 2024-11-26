using AuthApi.Models.Requests;
using AuthApi.Models.Responses;
using ShopAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace ShopApp.Services
{
    public static class APIService
    {
        private static readonly HttpClient httpClient = new();
        private
        const string baseUrl = "https://localhost:7120/api/";
        private static string? jwtToken;

        public static async Task<APIResponse<T>> SendRequestAsync<T>(string endpoint, object? payload, HttpMethod method, bool useToken = true)
        {
            try
            {
                var url = $"{baseUrl}{endpoint}";
                HttpRequestMessage request = new HttpRequestMessage(method, url);

                if ((method == HttpMethod.Post || method == HttpMethod.Put) && payload != null)
                {
                    request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                }

                if (useToken && jwtToken != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                }

                var response = await httpClient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<APIResponse<T>>(responseString);
                    if (result == null)
                        return new APIResponse<T>(false, "Failed to parse response");

                    return result;
                }

                var errorResponse = JsonSerializer.Deserialize<APIResponse>(responseString);
                var errorMessage = errorResponse?.error ?? "Unknown Error";

                return new APIResponse<T>(false, errorMessage);
            }
            catch (Exception ex)
            {
                return new APIResponse<T>(false, ex.Message);
            }
        }

        public static async Task<APIResponse<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var result = await SendRequestAsync<LoginResponse>("auth/login", loginRequest, HttpMethod.Post, false);
            if (result.success && result.data != null)
                jwtToken = result.data.jwtToken;
            return result;
        }

        public static async Task<APIResponse<LoginResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            var result = await SendRequestAsync<LoginResponse>("auth/register", registerRequest, HttpMethod.Post, false);
            if (result.success && result.data != null)
                jwtToken = result.data.jwtToken;
            return result;
        }

        public static async Task<APIResponse<LoginResponse>> UserInfoAsync()
        {
            var result = await SendRequestAsync<LoginResponse>("auth/me", null, HttpMethod.Get);
            return result;
        }

        public static async Task<APIResponse<List<Product>>> GetProducts()
        {
            var result = await SendRequestAsync<List<Product>>("product/list", null, HttpMethod.Get);
            return result;
        }

        public static async Task<APIResponse<Product>> GetProductInfo(int productId)
        {
            var result = await SendRequestAsync<Product>("product/info?id=" + productId.ToString(), null, HttpMethod.Get);

            return result;
        }

        public static async Task<APIResponse<List<CartItemMin>>> GetCartItems()
        {
            var result = await SendRequestAsync<List<CartItemMin>>("cart/items", null, HttpMethod.Get);
            return result;
        }

        public static async Task<APIResponse<List<CartItemMin>>> RemoveCartItem(int productId)
        {
            var result = await SendRequestAsync<List<CartItemMin>>("cart/remove?productId=" + productId.ToString(), null, HttpMethod.Delete);
            return result;
        }

        public static async Task<APIResponse<List<CartItemMin>>> ModifyCartItem(CartItemMin updatedItem)
        {
            var result = await SendRequestAsync<List<CartItemMin>>("cart/modify", updatedItem, HttpMethod.Put);
            return result;
        }

        public static async Task<APIResponse<List<CartItemMin>>> AddCartItem(CartItemMin addItem)
        {
            var result = await SendRequestAsync<List<CartItemMin>>("cart/add", addItem, HttpMethod.Post);
            return result;
        }

        public static async Task<APIResponse<object>> CreateOrder()
        {
            var result = await SendRequestAsync<object>("order/create", null, HttpMethod.Post);
            return result;
        }

        public static async Task<APIResponse<List<OrderInfo>>> GetOrders()
        {
            var result = await SendRequestAsync<List<OrderInfo>>("order/list", null, HttpMethod.Get);
            return result;
        }
    }
}