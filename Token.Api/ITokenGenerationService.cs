namespace Token.Api;

public interface ITokenGenerationService
{
    public string GenerateToken(TokenGenerationRequest request,
        string? changeSecret = null,
        string? changeIssuer = null);
}
