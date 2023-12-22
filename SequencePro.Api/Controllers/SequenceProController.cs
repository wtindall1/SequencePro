using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SequencePro.Api.Constants;
using SequencePro.API.Mapping;
using SequencePro.Application.Interfaces;
using SequencePro.Contracts.Requests;

namespace SequencePro.API.Controllers
{
    [ApiController]
    public class SequenceProController : ControllerBase
    {
        private readonly ISequenceAnalysisService _sequenceAnalysisService;
        private readonly IOutputCacheStore _outputCacheStore;

        public SequenceProController(
            ISequenceAnalysisService sequenceAnalysisService,
            IOutputCacheStore outputCacheStore)
        {
            _sequenceAnalysisService = sequenceAnalysisService;
            _outputCacheStore = outputCacheStore;
        }

        [Authorize(AuthConstants.TrustedUserPolicyName)]
        [HttpPost(ApiEndpoints.SequenceAnalysis.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSequenceAnalysisRequest request,
            CancellationToken token = default)
        {
            var sequenceAnalysis = await _sequenceAnalysisService.CreateAsync(request.UniprotId, token);
            await _outputCacheStore.EvictByTagAsync(CachingConstants.SequenceAnalysisTag, token);

            var sequenceAnalysisResponse = sequenceAnalysis.MapToResponse();
            return CreatedAtAction(nameof(Get), new { IdOrUniprotId = sequenceAnalysis.UniprotId }, sequenceAnalysisResponse);
        }

        [HttpGet(ApiEndpoints.SequenceAnalysis.Get)]
        [OutputCache(PolicyName = CachingConstants.Expire30PolicyName)]
        public async Task<IActionResult> Get([FromRoute] string IdOrUniprotId,
            CancellationToken token = default)
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
        [OutputCache(PolicyName = CachingConstants.Expire30PolicyName)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllSequenceAnalysisRequest request,
            CancellationToken token = default)
        {
            var allAnalyses = await _sequenceAnalysisService.GetAllAsync(token);

            var response = allAnalyses.MapToResponse();
            return Ok(response);
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.SequenceAnalysis.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id,
            CancellationToken token = default)
        {
            var deleted = await _sequenceAnalysisService.DeleteByIdAsync(Id, token);
            await _outputCacheStore.EvictByTagAsync(CachingConstants.SequenceAnalysisTag, token);

            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
