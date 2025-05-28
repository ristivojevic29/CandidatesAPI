using CandidatesAPI.Application.DTOs;
using CandidatesAPI.Application.Interfaces;
using CandidatesAPI.Entities;
using CandidatesAPI.Infrastructure.InfrastructureRepositiores;

namespace CandidatesAPI.Application.Services
{
    public class ContractService : IContractService
    {
        private readonly ILogger<ContractService> _logger;
        private readonly IAmortPlanRepository _amortPlanRepository;
        private readonly IContractRepository _contractRepository;
        public ContractService(ILogger<ContractService> logger, IAmortPlanRepository amortPlanRepository, IContractRepository contractRepository)
        {
            _logger = logger;
            _amortPlanRepository = amortPlanRepository;
            _contractRepository = contractRepository;
        }

        public async Task<IEnumerable<ContractDto>> GetContractsByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            var contracts = await _contractRepository.GetContractsByCustomerId(customerId, cancellationToken);

            if (contracts is null || !contracts.Any())
            {
                _logger.LogWarning($"No contracts found for customer ID: {customerId}");
                return Enumerable.Empty<ContractDto>();
            }
            var contractDtos = contracts.Select(c => new ContractDto
            {
                ContractNumber = c.ContractNumber,
                Description = c.Description,
                TotalAmountPaid = TotalAmountPaid(c.AmortPlans),
                TotalDebt = TotalDebt(c.AmortPlans),
                ClaimDebt = ClaimDebt(c.AmortPlans),
                AmortPlans = c.AmortPlans.Select(ap => new AmortPlanDto
                {
                    DocumentId = ap.DocumentId,
                    ClaimDueDate = ap.ClaimDueDate.ToDateTime(TimeOnly.MinValue),
                    PaidAmount = ap.PaidAmount,
                    DebtPerPlan = ap.TotalAmount - ap.PaidAmount,
                    TotalAmount = ap.TotalAmount,
                    Currency = ap.CurrencyCode
                }),
            });
            return contractDtos;
        }
        private static decimal TotalAmountPaid(IEnumerable<AmortPlan> amortPlan)
        {
            var totalAmountPayed = amortPlan.Sum(x => x.PaidAmount);
            return totalAmountPayed;
        }
        private static decimal TotalDebt(IEnumerable<AmortPlan> amortPlan)
        {
            var totalAmount = amortPlan.Sum(x => x.TotalAmount);
            var totalAmountPayed = amortPlan.Sum(x => x.PaidAmount);
            return totalAmount - totalAmountPayed;
        }
        private static decimal ClaimDebt(IEnumerable<AmortPlan> amortPlan)
        {
            var amortPlanClaimDebt = amortPlan.Where(x => x.ClaimDueDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now);
            return amortPlanClaimDebt.Sum(x => x.TotalAmount);
        }
    }
}
