namespace Token.Api;

public interface ITokenGenerationService
{
    public string GenerateToken(TokenGenerationRequest request);
}
