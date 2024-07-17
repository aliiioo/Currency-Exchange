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
                var data=await response.Content.ReadAsStringAsync();
                ExchangeRateResponse data1 = JsonSerializer.Deserialize<ExchangeRateResponse>(data, options);
                return new decimal();
                // if (data1)
                // {
                //     
                // }
                // else
                // {
                //     throw new KeyNotFoundException($"Currency '{targetCurrency}' not found in response.");
                // }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions
                Console.WriteLine($"Error fetching exchange rate: {ex.Message}");
                throw;
            }
        }
    }

    public class ExchangeRateResponse
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Rates Rates { get; set; }
    }

    public class Rates
    {
        public decimal USD { get; set; }
        // Add other currency properties here as needed
    }
}
