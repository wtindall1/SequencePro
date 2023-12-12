using FluentAssertions;
using SequencePro.API;
using SequencePro.Application.Database;
using SequencePro.Contracts.Requests;
using SequencePro.Integration.Tests.Fixtures.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SequencePro.Integration.Tests.ApiTests;
public class SequenceProControllerAuthTests : IClassFixture<AuthTestsFixture>
{
    private readonly AuthTestsFixture _serverFixture;
    private readonly HttpClient _httpClient;
    private readonly SequenceProContext _testDbContext;
    public SequenceProControllerAuthTests(AuthTestsFixture serverFixture)
    {
        _serverFixture = serverFixture;
        _httpClient = _serverFixture.CreateClient();
        _testDbContext = _serverFixture.CreateTestDbContext();
        
        _testDbContext.Database.EnsureDeleted();
        _testDbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task Create_ShouldReturn201_WhenAuthIsSuccessful()
    {
        //Arrange
        var url = $"/" + ApiEndpoints.SequenceAnalysis.Create;
        var request = new CreateSequenceAnalysisRequest
        {
            UniprotId = "P12345"
        };
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _serverFixture.GenerateToken(claim: "trusted_user"));

        //Act
        var result = await _httpClient.PostAsJsonAsync(url, request);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created, result.ReasonPhrase?.ToString());
    }
}
