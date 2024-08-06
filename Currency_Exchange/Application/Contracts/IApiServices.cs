namespace Application.Contracts
{
    public interface IApiServices
    {
        public Task<decimal?> GetExchangeRateAsync(string fromCurrency, string toCurrency);
    }
}
