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
using SequencePro.Integration.Tests.TestObjects;
using SequencePro.Application.Database.Models;

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
        _testDbContext.Database.EnsureDeleted();
        _testDbContext.Database.EnsureCreated();

        _sut = _container.Resolve<SequenceProController>();
    }

    [Fact]
    public async Task Create_SavesRecordToDb_WhenUniprotIdIsValid()
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

    [Fact]
    public async Task GetAll_ReturnsRecordsFromDatabase()
    {
        //Arrange
        var entities = new List<SequenceAnalysisEntity>();
        for (int i = 0; i < 10; i++)
        {
            entities.Add(SequenceAnalysisExample.Create().MapToEntity());
        }
        _testDbContext.AddRange(entities);
        await _testDbContext.SaveChangesAsync();

        //Act
        var result = (OkObjectResult)await _sut.GetAll();
        var response = (AllAnalysesResponse)result.Value!;

        //Assert
        result.StatusCode.Should().Be(200);
        response.Items.Should().HaveSameCount(entities);
        entities.ForEach(x => response.Items.Should().ContainEquivalentOf(x.MapToObject()));
    }
}
