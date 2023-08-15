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

        private readonly ISequenceAnalysisService _sequenceAnalysisService;

        
        //inject analysis service
        public SequenceProController(ISequenceAnalysisService sequenceAnalysisService)
        {
            _sequenceAnalysisService = sequenceAnalysisService;
        }


        [HttpPost(ApiEndpoints.SequenceAnalysis.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSequenceAnalysisRequest request)
        {
            var sequenceAnalysis = await _sequenceAnalysisService.CreateAsync(request.UniprotId);
            var sequenceAnalysisResponse = sequenceAnalysis.MapToResponse();

            return CreatedAtAction(nameof(Get), new { IdOrUniprotId = sequenceAnalysis.UniprotId }, sequenceAnalysisResponse);
        }

        [HttpGet(ApiEndpoints.SequenceAnalysis.Get)]
        public async Task<IActionResult> Get([FromRoute] string IdOrUniprotId)
        {
            //check if search is guid & choose matching get function
            var sequenceAnalysis = Guid.TryParse(IdOrUniprotId, out var id)
                ? await _sequenceAnalysisService.GetByIdAsync(id)
                : await _sequenceAnalysisService.GetByUniprotIdAsync(IdOrUniprotId);
            
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
            var allAnalyses = await _sequenceAnalysisService.GetAllAsync();

            var response = allAnalyses.MapToResponse();
            return Ok(response);

        }

        [HttpDelete(ApiEndpoints.SequenceAnalysis.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            var deleted = await _sequenceAnalysisService.DeleteByIdAsync(Id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
