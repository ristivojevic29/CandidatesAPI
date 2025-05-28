namespace CandidatesAPI.Application.Interfaces
{
    public interface IExchangeRateService
    {
        Task ImportExchangeRates(DateTime date, CancellationToken cancellationToken);
    }
}
