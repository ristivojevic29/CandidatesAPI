using CandidatesAPI.Entities;

namespace CandidatesAPI.Infrastructure.InfrastructureRepositiores
{
    public interface IExchangeRateRepository
    {
        Task InsertExchangeRates(IEnumerable<ExchangeRate> exchangeRate, CancellationToken cancellationToken);
        Task<IEnumerable<Currency>> GetAllCurrencies();

    }
}
