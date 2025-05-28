using CandidatesAPI.Application.DTOs;
using CandidatesAPI.Application.Interfaces;
using CandidatesAPI.Entities;
using CandidatesAPI.Infrastructure.InfrastructureRepositiores;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Globalization;

namespace CandidatesAPI.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ILogger<ExchangeRateService> _logger;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        public ExchangeRateService(ILogger<ExchangeRateService> logger, IExchangeRateRepository exchangeRateRepository, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _exchangeRateRepository = exchangeRateRepository;
            _httpClient = httpClientFactory.CreateClient();
            _appSettings = appSettings.Value;
        }
        public async Task ImportExchangeRates(DateTime date, CancellationToken cancellationToken)
        {
            var exchangeRateList = await GetExchangeList(date, cancellationToken);
            List<ExchangeRate> exchangeRateListForInsert = new List<ExchangeRate>();
            if (exchangeRateList == null)
            {
                _logger.LogWarning("No exchange rates found for date: {Date}", date);
                return;
            }
            var currencies = await _exchangeRateRepository.GetAllCurrencies();
            foreach (var currency in currencies)
            {
                if (exchangeRateList.Result.Currencies.TryGetValue(currency.CurrencyCode.ToLower(), out var exchangeRate))
                {
                    decimal sre = Convert.ToDecimal(exchangeRate["sre"]);
                    exchangeRateListForInsert.Add(new ExchangeRate()
                    {
                        CurrencyFrom = currency.CurrencyCode,
                        CurrencyTo = "RSD",
                        ExchangeRate1 = sre,
                        ExchangeRateDate = DateOnly.FromDateTime(date),
                        Ts = DateTime.UtcNow
                    });
                }
            }
            await _exchangeRateRepository.InsertExchangeRates(exchangeRateListForInsert, cancellationToken);
        }
        private async Task<ExchangeRateApiResponse> GetExchangeList(DateTime date, CancellationToken cancellationToken)
        {
            var apiKey = _appSettings.ExchangeRateApiKey;

            var url = string.Format(_appSettings.ExchangeRateApiUrl, apiKey, date.ToString("dd.MM.yyyy", new CultureInfo("sr-Latn-RS")));

            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch exchange rates from the API. Status code: {StatusCode}", response.StatusCode);
                    throw new Exception("Failed to fetch exchange rates from the API.");
                }
                var result = await response.Content.ReadAsStringAsync();
                var exchangeRates = JsonConvert.DeserializeObject<ExchangeRateApiResponse>(result);
                return exchangeRates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching exchange rates for date: {Date}", date);
                throw new Exception($"An error occurred while fetching exchange rates: {ex.Message}");
            }
        }
    }
}
