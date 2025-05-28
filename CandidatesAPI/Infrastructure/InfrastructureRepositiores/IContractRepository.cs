using CandidatesAPI.Entities;

namespace CandidatesAPI.Infrastructure.InfrastructureRepositiores
{
    public interface IContractRepository
    {
        public Task<IEnumerable<Contract>> GetContractsByCustomerId(int customerId, CancellationToken cancellationToken);
    }
}
