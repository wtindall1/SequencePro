using Autofac;
using SequencePro.Application.IoC;
using SequencePro.Integration.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Token.Api;

namespace SequencePro.Integration.Tests.Fixtures.Auth;
public class AuthTestsFixture : BaseSequenceProApiServerFixture
{
    private ITokenGenerationService _tokenGenerationService = new TokenGenerationService();
    public AuthTestsFixture() : base()
    {
    }

    public string GenerateToken(
        string? changeSecret = null,
        string? changeIssuer = null,
        string? claim = null)
    {
        var request = new TokenGenerationRequest
        {
            UserId = Guid.NewGuid(),
            Email = "test@gmail.com"
        };

        if (claim != null)
        {
            request.CustomClaims.Add(claim, JsonDocument.Parse(JsonSerializer.Serialize("true")).RootElement);
        }

        return _tokenGenerationService.GenerateToken(
            request,
            changeSecret,
            changeIssuer);
    }
}


