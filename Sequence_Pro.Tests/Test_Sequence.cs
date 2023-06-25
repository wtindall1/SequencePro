using Xunit;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Models;

namespace Sequence_Pro.Tests;

//MAKE A TEST FIXTURE FOR THE HTTPCLIENT

public class Test_UniprotAPI
{
    [Fact]
    public async void Test_GetSequenceDetails_Returns_Sequence_Object ()
    {
        var uniprotAPI = new UniprotAPI();
        var sequence = await uniprotAPI.GetSequenceDetails("P12345", new HttpClient());

        Assert.True(sequence.GetType() == typeof(Sequence), $"{sequence.GetType()} was returned");
    }
}