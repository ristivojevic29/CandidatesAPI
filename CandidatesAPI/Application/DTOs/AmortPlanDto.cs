namespace CandidatesAPI.Application.DTOs
{
    public class AmortPlanDto
    {
        public string DocumentId { get; set; }
        public DateTime ClaimDueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DebtPerPlan { get; set; }
        public string Currency { get; set; }
    }
}
