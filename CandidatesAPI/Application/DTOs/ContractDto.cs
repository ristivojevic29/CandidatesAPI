namespace CandidatesAPI.Application.DTOs
{
    public class ContractDto
    {
        public string ContractNumber { get; set; }
        public string Description { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal ClaimDebt { get; set; }
        public IEnumerable<AmortPlanDto> AmortPlans { get; set; }
    }
}
