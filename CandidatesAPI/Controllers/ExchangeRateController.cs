using CandidatesAPI.Application.DTOs;
using CandidatesAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CandidatesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ILogger<ExchangeRateController> _logger;
        private readonly IExchangeRateService _exchangeRateService;
        public ExchangeRateController(ILogger<ExchangeRateController> logger, IExchangeRateService exchangeRateService)
        {
            _logger = logger;
            _exchangeRateService = exchangeRateService;
        }
        [HttpPost]
        [Route("ImportExchangeRates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportExchangeRates(ImportExchangeRateDto importExchangeRateDto, CancellationToken cancellationToken)
        {
            try
            {
                if (importExchangeRateDto.Date.HasValue)
                {
                    await _exchangeRateService.ImportExchangeRates((DateTime)importExchangeRateDto.Date, cancellationToken);
                    return Ok($"Exchange rates imported for date: {importExchangeRateDto.Date:dd.MM.yyyy}.");
                }
                else if (importExchangeRateDto.DateFrom.HasValue && importExchangeRateDto.DateTo.HasValue)
                {
                    if (importExchangeRateDto.DateFrom > importExchangeRateDto.DateTo)
                    {
                        return BadRequest("DateFrom cannot be later than DateTo.");
                    }
                    for (var date = importExchangeRateDto.DateFrom; date <= importExchangeRateDto.DateTo; date = date.Value.AddDays(1))
                    {
                        await _exchangeRateService.ImportExchangeRates((DateTime)date, cancellationToken);
                    }
                    return Ok($"Exchange rates imported from {importExchangeRateDto.DateFrom:dd.MM.yyyy} to {importExchangeRateDto.DateTo:dd.MM.yyyy}.");
                }
                else
                {
                    return BadRequest("Invalid date range provided.");
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, $"Operation was cancelled while importing exchange rates for date: {importExchangeRateDto.Date}");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "The request was cancelled due to a timeout.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while importing exchange rates for date: {importExchangeRateDto.Date}");
                return BadRequest($"An error occurred while importing exchange rates: {ex.Message}");
            }
        }
    }
}
