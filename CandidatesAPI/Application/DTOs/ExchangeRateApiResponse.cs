using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CandidatesAPI.Application.DTOs
{
    public class ExchangeRateApiResponse
    {
        public ExchangeRateResult Result { get; set; }
    }
    public class ExchangeRateResult
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> Currencies { get; set; }
    }
    public class Rate
    {
        public string Sre { get; set; }
    }
}