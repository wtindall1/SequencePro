using Microsoft.AspNetCore.Mvc.Testing;
using SequencePro.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Integration.Tests.ApiTests;
public class SequenceProControllerTests : IClassFixture<WebApplicationFactory<ISequenceProApiMarker>>
{
    private readonly HttpClient _httpClient;
    public SequenceProControllerTests(WebApplicationFactory<ISequenceProApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }
}
