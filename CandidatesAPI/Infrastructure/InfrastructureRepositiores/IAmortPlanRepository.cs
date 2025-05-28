using CandidatesAPI.Entities;

namespace CandidatesAPI.Infrastructure.InfrastructureRepositiores
{
    public interface IAmortPlanRepository
    {
        public IEnumerable<AmortPlan> GetAmortPlansByContractId(int contractId);
    }
}
