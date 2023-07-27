using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sequence_Pro.API.Mapping;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Contracts.Requests;

namespace Sequence_Pro.API.Controllers
{
    [ApiController]
    public class SequenceProController : ControllerBase
    {

        private readonly IUniprotAPI _uniprotAPI;
        private readonly ISequenceAnalyser _sequenceAnalyser;
        private readonly HttpClient _httpClient;
        
        //inject uniprot API caller and sequence anlyser
        public SequenceProController(IUniprotAPI uniprotAPI, 
            ISequenceAnalyser sequenceAnalyser, 
            HttpClient httpClient)
        {
            _uniprotAPI = uniprotAPI;
            _sequenceAnalyser = sequenceAnalyser;
            _httpClient = httpClient;
        }

        [HttpPost(ApiEndpoints.SequenceAnalysis.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSequenceAnalysisRequest request)
        {
            string uniprotId = request.UniprotId;
            var sequence = await _uniprotAPI.GetSequenceDetails(uniprotId, _httpClient);
            var sequenceAnalysis = _sequenceAnalyser.Analyse(sequence);
            var sequenceAnalysisResponse = sequenceAnalysis.MapToResponse();
            return CreatedAtAction(nameof(Get), new { uniprotId = sequenceAnalysis.UniprotId }, sequenceAnalysisResponse);
        }

        [HttpGet(ApiEndpoints.SequenceAnalysis.Get)]
        public async Task<IActionResult> Get([FromRoute] string uniprotId)
        {
            return Ok();
        }

    }
}
