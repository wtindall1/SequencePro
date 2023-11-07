using Xunit;
using SequencePro.Application.Services;
using SequencePro.Application.Models;
using System.Runtime.CompilerServices;
using SequencePro.Application.Interfaces;

namespace SequencePro.Integration.Tests.ApplicationTests;

public class UniprotAPI_IntegrationTests : IClassFixture<HttpClientFixture>
{

    private readonly HttpClient _httpClient;
    private readonly IUniprotAPI _uniprotAPI;

    public UniprotAPI_IntegrationTests(HttpClientFixture httpClientFixture)
    {
        _httpClient = httpClientFixture.httpClient;
        _uniprotAPI = new UniprotAPI();
    }

    [Fact]
    public async void Test_GetSequenceDetails_Returns_Sequence_Object()
    {
        //Arrange
        var uniprotId = "P12345";

        //Act
        var sequence = await _uniprotAPI.GetSequenceDetails(uniprotId, _httpClient);

        //Assert
        Assert.True(sequence.GetType() == typeof(Sequence), $"{sequence.GetType()} was returned");
    }

    [Fact]
    public void Test_GetSequenceDetails_Invalid_Identifier_Throws_HttpRequestException()
    {
        //Arrange
        var uniprotId = "G4612345";

        //Act
        Func<Task> asyncAction = async () => await _uniprotAPI.GetSequenceDetails(uniprotId, _httpClient);

        //Assert
        Assert.ThrowsAsync<HttpRequestException>(asyncAction);
    }
}

public class HttpClientFixture : IDisposable
{
    public HttpClient httpClient { get; }

    public HttpClientFixture()
    {
        httpClient = new HttpClient();
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }
}