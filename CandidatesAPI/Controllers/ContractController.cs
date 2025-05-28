using CandidatesAPI.Application.DTOs;
using CandidatesAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CandidatesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly ILogger<ContractController> _logger;
        private readonly IContractService _contractService;
        public ContractController(ILogger<ContractController> logger, IContractService contractService)
        {
            _logger = logger;
            _contractService = contractService;
        }
        [HttpGet]
        [Route("GetContractsByCustomerId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ContractDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetContractsByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var contracts = await _contractService.GetContractsByCustomerId(customerId, cancellationToken);
                if (!contracts.Any())
                {
                    return NotFound($"No contracts found for customer ID: {customerId}");
                }
                return Ok(contracts);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation was cancelled while retrieving contracts");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "The request was cancelled due to a timeout.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving contracts for customer ID: {customerId}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving contracts: {ex.Message}");
            }
        }

    }
}
