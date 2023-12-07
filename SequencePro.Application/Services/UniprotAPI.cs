using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using SequencePro.Application.Interfaces;
using SequencePro.Application.Models;
using System.Net;
using Microsoft.Extensions.Logging;
using SequencePro.Application.Logging;

namespace SequencePro.Application.Services;

public class UniprotAPI : IUniprotAPI
{
    private const string _baseUrl = "https://www.uniprot.org/uniprotkb/";
    private readonly ILoggerAdapter _logger;

    public UniprotAPI(ILoggerAdapter loggerAdapter)
    {
        _logger = loggerAdapter;
    }

    public async Task<Sequence> GetSequenceDetails(string uniprotId, HttpClient client, CancellationToken token = default)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"{_baseUrl}{uniprotId}.json", token);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync(token);
            var json = JsonDocument.Parse(responseBody);

            var sequence = new Sequence
            {
                uniqueIdentifier = uniprotId,
                entryName = json.RootElement.GetProperty("uniProtkbId").ToString(),
                proteinName = json.RootElement.GetProperty("proteinDescription").GetProperty("recommendedName").GetProperty("fullName").GetProperty("value").ToString(),
                organismName = json.RootElement.GetProperty("organism").GetProperty("scientificName").ToString(),
                aminoAcidSequence = json.RootElement.GetProperty("sequence").GetProperty("value").ToString()
            };

            json.Dispose();
            _logger.LogInformation(
                "Sequence details for UniprotId {1} retrieved successfully.", uniprotId);
            return sequence;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw ex;
        }
    }
}