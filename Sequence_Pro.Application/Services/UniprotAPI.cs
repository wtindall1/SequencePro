using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
using System.Net;

namespace Sequence_Pro.Application.Services;

public class UniprotAPI : IUniprotAPI
{
    private const string _baseUrl = "https://www.uniprot.org/uniprotkb/";

    public async Task<Sequence> GetSequenceDetails(string uniprotId, HttpClient client, CancellationToken token = default)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"{_baseUrl}{uniprotId}.json", token);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
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
            return sequence;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            throw e;
        }
    }
}