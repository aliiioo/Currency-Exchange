using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.API_Calls
{
    public class CurrencyService
    {
        private readonly HttpClient _client;

        public CurrencyService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.BaseAddress = new Uri("https://api.exchangeratesapi.io/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<decimal> GetExchangeRateAsync(string baseCurrency, string targetCurrency)
        {
            string url = $"latest?base={baseCurrency}&symbols={targetCurrency}";

            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Ensure success status code

                var data = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>(); // استفاده از ReadFromJsonAsync برای خواندن JSON

                decimal exchangeRate = 0;
                bool found = false;

                
                //foreach (var rate in data.Rates)
                //{
                //    if (rate.Key == targetCurrency)
                //    {
                //        exchangeRate = rate.Value;
                //        found = true;
                //        break;
                //    }
                //}

                if (found)
                {
                    return exchangeRate;
                }
                else
                {
                    throw new KeyNotFoundException($"Currency '{targetCurrency}' not found in response.");
                }
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
