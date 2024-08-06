using Application.Contracts;
using System.Text.Json;

namespace Application.API_Calls
{
    public class CurrencyService :IApiServices
    {
        private readonly HttpClient _client;

        public CurrencyService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.BaseAddress = new Uri("https://min-api.cryptocompare.com/data/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<decimal?> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            var apiKey = Environment.GetEnvironmentVariable("APIKey");
            var url = $"pricemulti?fsyms={fromCurrency}&tsyms={toCurrency}&api_key={apiKey}";


            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _client.SendAsync(request);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var dataReader = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(dataReader);
                if (data != null) return data.Values.FirstOrDefault().FirstOrDefault().Value;
                return null;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions
                Console.WriteLine($"Error fetching exchange rate: {ex.Message}");
                throw;
            }
        }
    }
}
