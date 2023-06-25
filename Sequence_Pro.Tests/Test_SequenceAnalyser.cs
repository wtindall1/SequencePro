using System;
using Xunit;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Models;
using System.Runtime.CompilerServices;


namespace Sequence_Pro.Tests;


public class Test_SequenceAnalyser : IClassFixture<HttpClientFixture>
{
    private readonly HttpClient _httpClient;

    public Test_SequenceAnalyser(HttpClientFixture httpClientFixture)
    {
        _httpClient = httpClientFixture.httpClient;
    }

    [Fact]
    public async void Test_SequenceAnalyser_Analyse_Molecular_Weight()
    {
        var uniprotAPI = new UniprotAPI();
        var sequence = await uniprotAPI.GetSequenceDetails("P12563", _httpClient);

        var sequenceAnalyser = new SequenceAnalyser();
        var sequenceAnalysis = sequenceAnalyser.Analyse(sequence);

        Assert.True(Math.Round(sequenceAnalysis.molecularWeight, 2) == 64394.58, $"Molecular weight was {sequenceAnalysis.molecularWeight} vs expected: 64394.58");

    }
}