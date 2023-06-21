using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace Application.Services;

public static class UniprotAPI
{
    static string _baseUrl = "https://www.uniprot.org/uniprotkb/";


    public static async Task GetSequenceDetails(string uniprotId, HttpClient client)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"{_baseUrl}{uniprotId}.json");
            response.EnsureSuccessStatusCode(); //throws exception if not success

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseBody);

            //extract the json properties and pass to sequence object
            var sequence = new Sequence(uniprotId,
                                        json.RootElement.GetProperty("uniProtkbId").ToString(),
                                        json.RootElement.GetProperty("proteinDescription").GetProperty("recommendedName").GetProperty("fullName").GetProperty("value").ToString(),
                                        json.RootElement.GetProperty("organism").GetProperty("scientificName").ToString(),
                                        json.RootElement.GetProperty("sequence").GetProperty("value").ToString());

            json.Dispose();

            return sequence;
            

        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}