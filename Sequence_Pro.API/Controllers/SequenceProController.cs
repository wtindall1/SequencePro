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
        private readonly ISequenceAnalysisRepository _repository;
        private readonly HttpClient _httpClient;
        
        //inject uniprot API caller, sequence analyser, repository
        public SequenceProController(IUniprotAPI uniprotAPI, 
            ISequenceAnalyser sequenceAnalyser,
            ISequenceAnalysisRepository sequenceAnalysisRepository,
            HttpClient httpClient)
        {
            _uniprotAPI = uniprotAPI;
            _sequenceAnalyser = sequenceAnalyser;
            _repository = sequenceAnalysisRepository;
            _httpClient = httpClient;
        }

        [HttpPost(ApiEndpoints.SequenceAnalysis.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSequenceAnalysisRequest request)
        {
            //create and analyse sequence
            var sequence = await _uniprotAPI.GetSequenceDetails(request.UniprotId, _httpClient);
            var sequenceAnalysis = _sequenceAnalyser.Analyse(sequence);

            await _repository.CreateAsync(sequenceAnalysis);
            var sequenceAnalysisResponse = sequenceAnalysis.MapToResponse();
            return CreatedAtAction(nameof(Get), new { IdOrUniprotId = sequenceAnalysis.UniprotId }, sequenceAnalysisResponse);
        }

        [HttpGet(ApiEndpoints.SequenceAnalysis.Get)]
        public async Task<IActionResult> Get([FromRoute] string IdOrUniprotId)
        {
            //check if search is guid & choose matching get function
            var sequenceAnalysis = Guid.TryParse(IdOrUniprotId, out var id)
                ? await _repository.GetByIdAsync(id)
                : await _repository.GetByUniprotIdAsync(IdOrUniprotId);
            
            if (sequenceAnalysis is null)
            {
                return NotFound();
            }
            var response = sequenceAnalysis.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.SequenceAnalysis.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var allAnalyses = await _repository.GetAllAsync();

            var response = allAnalyses.MapToResponse();
            return Ok(response);

        }

        [HttpDelete(ApiEndpoints.SequenceAnalysis.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            var deleted = await _repository.DeleteByIdAsync(Id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
