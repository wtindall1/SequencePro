using Microsoft.AspNetCore.Mvc;

namespace Token.Api.Controllers;

[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenGenerationService _tokenService;

    public TokenController(ITokenGenerationService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
    {
        var jwt = _tokenService.GenerateToken(request);
        return Ok(jwt);
    }
}
