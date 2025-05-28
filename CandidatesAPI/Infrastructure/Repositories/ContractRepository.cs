using CandidatesAPI.Entities;
using CandidatesAPI.Infrastructure.InfrastructureRepositiores;
using Microsoft.EntityFrameworkCore;

namespace CandidatesAPI.Infrastructure.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly CandidatesContext _context;
        public ContractRepository(CandidatesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contract>> GetContractsByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Contracts
                .Include(ap => ap.AmortPlans)
                .Where(c => c.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
