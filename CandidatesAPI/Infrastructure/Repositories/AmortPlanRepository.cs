using CandidatesAPI.Entities;
using CandidatesAPI.Infrastructure.InfrastructureRepositiores;

namespace CandidatesAPI.Infrastructure.Repositories
{
    public class AmortPlanRepository : IAmortPlanRepository
    {
        private readonly CandidatesContext _context;
        public AmortPlanRepository(CandidatesContext context)
        {
            _context = context;
        }
        public IEnumerable<AmortPlan> GetAmortPlansByContractId(int contractId)
        {
            return _context.AmortPlans.Where(ap => ap.ContractId == contractId)
                .ToList();
        }
    }
}
