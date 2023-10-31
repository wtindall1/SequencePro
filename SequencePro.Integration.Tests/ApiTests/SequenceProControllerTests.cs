using Microsoft.AspNetCore.Mvc.Testing;
using SequencePro.Api;
using SequencePro.Contracts.Requests;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using SequencePro.API;
using Microsoft.EntityFrameworkCore;
using SequencePro.Application.Database;
using System.Net.Http.Json;
using SequencePro.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using SequencePro.Application.Database.Mapping;
using SequencePro.Integration.Tests.Fixtures;
using Autofac;
using Testcontainers.PostgreSql;
using SequencePro.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SequencePro.Integration.Tests.ApiTests;
public class SequenceProControllerTests : IClassFixture<SequenceProApiTestFixture>
{
    private readonly IContainer _container;
    private readonly SequenceProContext _testDbContext;
    private readonly SequenceProController _sut;

    public SequenceProControllerTests(SequenceProApiTestFixture fixture)
    {
        _container = fixture.Container!;
        _testDbContext = _container.Resolve<SequenceProContext>();
        _testDbContext.Database.EnsureCreated();
        _sut = _container.Resolve<SequenceProController>();
    }

    [Fact]
    public async Task CreateAsync_SavesRecordToDb_WhenUniprotIdIsValid()
    {
        //Arrange
        var request = new CreateSequenceAnalysisRequest
        {
            UniprotId = "P12345"
        };

        //Act
        var result = (CreatedAtActionResult)await _sut.Create(request);
        var sequenceAnalysisResponse = (SequenceAnalysisResponse)result.Value!;

        //Assert
        var entity = await _testDbContext.SequenceAnalyses
            .Where(x => x.Id == sequenceAnalysisResponse.Id)
            .SingleOrDefaultAsync();
        var expected = entity!.MapToObject();

        result.StatusCode.Should().Be(201);
        sequenceAnalysisResponse.Should().BeEquivalentTo(expected);
    }
}
