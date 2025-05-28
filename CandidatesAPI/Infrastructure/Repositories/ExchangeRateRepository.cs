using CandidatesAPI.Entities;
using CandidatesAPI.Infrastructure.InfrastructureRepositiores;
using Microsoft.EntityFrameworkCore;

namespace CandidatesAPI.Infrastructure.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly CandidatesContext _context;
        public ExchangeRateRepository(CandidatesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task InsertExchangeRates(IEnumerable<ExchangeRate> exchangeRate, CancellationToken cancellationToken)
        {
            await _context.ExchangeRates.AddRangeAsync(exchangeRate, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
