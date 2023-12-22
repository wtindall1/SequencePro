using SequencePro.Contracts.Requests;
using Microsoft.EntityFrameworkCore;
using SequencePro.Application.Database;
using SequencePro.Contracts.Responses;
using FluentAssertions;
using SequencePro.Application.Database.Mapping;
using SequencePro.Integration.Tests.Fixtures;
using Autofac;
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
        var result = await _sut.Create(request);

        //Assert
        var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>();
        createdAtActionResult.Subject.StatusCode.Should().Be(201);

        var sequenceAnalysisResponse = createdAtActionResult.Subject.Value.Should().BeOfType<SequenceAnalysisResponse>();
        var entityFromDb = await _testDbContext.SequenceAnalyses
            .Where(x => x.Id == sequenceAnalysisResponse.Subject.Id)
            .SingleOrDefaultAsync();
        var expected = entityFromDb!.MapToObject();
        sequenceAnalysisResponse.Subject.Should().BeEquivalentTo(expected);
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
        var result = await _sut.GetAll(new GetAllSequenceAnalysisRequest());

        //Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>();
        objectResult.Subject.StatusCode.Should().Be(200);

        var response = objectResult.Subject.Value.Should().BeOfType<AllAnalysesResponse>();
        response.Subject.Items.Should().HaveSameCount(entities);
        entities.ForEach(x => response.Subject.Items.Should().ContainEquivalentOf(x.MapToObject()));
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyCollection_WhenNoRecordExist()
    {
        //Act
        var result = await _sut.GetAll(new GetAllSequenceAnalysisRequest());

        //Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>();
        objectResult.Subject.StatusCode.Should().Be(200);

        var response = objectResult.Subject.Value.Should().BeOfType<AllAnalysesResponse>();
        response.Subject.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_ReturnsRecord_MatchingById()
    {
        //Arrange
        var entity = SequenceAnalysisExample.Create().MapToEntity();
        _testDbContext.Add(entity);
        await _testDbContext.SaveChangesAsync();

        //Act
        var result = (OkObjectResult)await _sut.Get(entity.Id.ToString());

        //Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>();
        objectResult.Subject.StatusCode.Should().Be(200);

        var response = objectResult.Subject.Value.Should().BeOfType<SequenceAnalysisResponse>();
        response.Subject.Should().BeEquivalentTo(entity.MapToObject());
    }

    [Fact]
    public async Task Get_ReturnsRecord_MatchingByUniprotId()
    {
        //Arrange
        var entity = SequenceAnalysisExample.Create().MapToEntity();
        _testDbContext.Add(entity);
        await _testDbContext.SaveChangesAsync();

        //Act
        var result = await _sut.Get(entity.UniprotId.ToString());

        //Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>();
        objectResult.Subject.StatusCode.Should().Be(200);

        var response = objectResult.Subject.Value.Should().BeOfType<SequenceAnalysisResponse>();
        response.Subject.Should().BeEquivalentTo(entity.MapToObject());
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNoRecordMatchesId()
    {
        //Act
        var result = await _sut.Get(Guid.NewGuid().ToString());

        //Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNoRecordMatchesUniprotId()
    {        
        //Act
        var result = await _sut.Get("P12567");

        //Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_RemovesRecordWithMatchingId()
    {
        //Arrange
        var entity = SequenceAnalysisExample.Create().MapToEntity();
        _testDbContext.Add(entity);
        await _testDbContext.SaveChangesAsync();

        //Act
        var result = await _sut.Delete(entity.Id);

        //Assert
        bool exists;
        using (var scope = _container.BeginLifetimeScope())
        {
            var testDbContext = scope.Resolve<SequenceProContext>();
            exists = _testDbContext.SequenceAnalyses
                .Where(x => x.Id == entity.Id)
                .Any();
        }
         
        result.Should().BeOfType<OkResult>();
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNoMatchingRecordExists()
    {
        //Act
        var result = await _sut.Delete(Guid.NewGuid());

        //Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
