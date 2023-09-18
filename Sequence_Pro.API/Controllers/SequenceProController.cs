using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sequence_Pro.API.Auth;
using Sequence_Pro.API.Mapping;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Contracts.Requests;

namespace Sequence_Pro.API.Controllers
{
    [ApiController]
    public class SequenceProController : ControllerBase
    {

        private readonly ISequenceAnalysisService _sequenceAnalysisService;

        public SequenceProController(ISequenceAnalysisService sequenceAnalysisService)
        {
            _sequenceAnalysisService = sequenceAnalysisService;
        }

        [Authorize(AuthConstants.TrustedUserPolicyName)]
        [HttpPost(ApiEndpoints.SequenceAnalysis.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSequenceAnalysisRequest request,
            CancellationToken token)
        {
            var sequenceAnalysis = await _sequenceAnalysisService.CreateAsync(request.UniprotId, token);
            var sequenceAnalysisResponse = sequenceAnalysis.MapToResponse();

            return CreatedAtAction(nameof(Get), new { IdOrUniprotId = sequenceAnalysis.UniprotId }, sequenceAnalysisResponse);
        }

        [HttpGet(ApiEndpoints.SequenceAnalysis.Get)]
        public async Task<IActionResult> Get([FromRoute] string IdOrUniprotId,
            CancellationToken token)
        {
            //check if search is guid & choose matching get method
            var sequenceAnalysis = Guid.TryParse(IdOrUniprotId, out var id)
                ? await _sequenceAnalysisService.GetByIdAsync(id, token)
                : await _sequenceAnalysisService.GetByUniprotIdAsync(IdOrUniprotId, token);
            
            if (sequenceAnalysis is null)
            {
                return NotFound();
            }
            var response = sequenceAnalysis.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.SequenceAnalysis.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var allAnalyses = await _sequenceAnalysisService.GetAllAsync(token);

            var response = allAnalyses.MapToResponse();
            return Ok(response);

        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.SequenceAnalysis.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id,
            CancellationToken token)
        {
            var deleted = await _sequenceAnalysisService.DeleteByIdAsync(Id, token);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
