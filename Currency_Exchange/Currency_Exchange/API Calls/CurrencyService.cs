using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.API_Calls
{
    public class CurrencyService
    {
        private readonly HttpClient _client;

        public CurrencyService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.BaseAddress = new Uri("https://min-api.cryptocompare.com/data/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
        }

        public async Task<decimal> GetExchangeRateAsync(string baseCurrency, string targetCurrency)
        {
            string api_key = "d30eacd56561ae4df8681c5bfcdf6b23ae033501824d39b82a5f59d51d0c41d4";
            string url = $"pricemulti?fsyms={baseCurrency}&tsyms={targetCurrency}&api_key={api_key}";

           
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _client.SendAsync(request);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var dataReder=await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(dataReder);
                return data.Values.FirstOrDefault().FirstOrDefault().Value;
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
