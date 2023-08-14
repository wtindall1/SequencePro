using Xunit;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Models;
using System.Runtime.CompilerServices;

namespace Sequence_Pro.Tests;

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



public class Test_UniprotAPI : IClassFixture<HttpClientFixture>
{

    private readonly HttpClient _httpClient;

    public Test_UniprotAPI(HttpClientFixture httpClientFixture)
    {
        _httpClient = httpClientFixture.httpClient;
    }


    [Fact]
    public async void Test_GetSequenceDetails_Returns_Sequence_Object()
    {
        var uniprotAPI = new UniprotAPI();
        var sequence = await uniprotAPI.GetSequenceDetails("P12345", _httpClient);

        Assert.True(sequence.GetType() == typeof(Sequence), $"{sequence.GetType()} was returned");
    }

    [Fact]
    public void Test_GetSequenceDetails_Invalid_Identifier_Throws_HttpRequestException()
    {
        var uniprotAPI = new UniprotAPI();

        //create delegate that should throw exception (invalid uniprot accession id)
        Func<Task> asyncAction = async () => await uniprotAPI.GetSequenceDetails("G4612345", _httpClient);

        Assert.ThrowsAsync<HttpRequestException>(asyncAction);


    }
}