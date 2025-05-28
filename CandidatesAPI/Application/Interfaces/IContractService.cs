using CandidatesAPI.Application.DTOs;

namespace CandidatesAPI.Application.Interfaces
{
    public interface IContractService
    {
        public Task<IEnumerable<ContractDto>> GetContractsByCustomerId(int customerId, CancellationToken cancellationToken);
    }
}
