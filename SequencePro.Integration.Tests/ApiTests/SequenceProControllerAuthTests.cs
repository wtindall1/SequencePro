﻿using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using SequencePro.API;
using SequencePro.Application.Database;
using SequencePro.Application.Database.Mapping;
using SequencePro.Contracts.Requests;
using SequencePro.Integration.Tests.Fixtures.Auth;
using SequencePro.Integration.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SequencePro.Integration.Tests.ApiTests;
public class SequenceProControllerAuthTests : IClassFixture<AuthTestsFixture>
{
    private readonly AuthTestsFixture _serverFixture;
    private readonly HttpClient _httpClient;
    private readonly SequenceProContext _testDbContext;
    public SequenceProControllerAuthTests(AuthTestsFixture serverFixture)
    {
        _serverFixture = serverFixture;
        _httpClient = _serverFixture.CreateClient();
        _testDbContext = _serverFixture.CreateTestDbContext();
        
        _testDbContext.Database.EnsureDeleted();
        _testDbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenAuthIsSuccessful()
    {
        //Arrange
        var url = ApiEndpoints.SequenceAnalysis.Create;
        var request = new CreateSequenceAnalysisRequest
        {
            UniprotId = "P12345"
        };
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _serverFixture.GenerateToken(claim: "trusted_user"));

        //Act
        var result = await _httpClient.PostAsJsonAsync(url, request);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created, result.ReasonPhrase?.ToString());
    }

    [Fact]
    public async Task Create_ShouldReturnUnauthorized_WhenAuthenticationFails()
    {
        //Arrange
        var url = ApiEndpoints.SequenceAnalysis.Create;
        var request = new CreateSequenceAnalysisRequest
        {
            UniprotId = "P12345"
        };
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _serverFixture.GenerateToken(
                claim: "trusted_user",
                changeSecret: "ThisIsNotTheSecret12345678987654321"));

        //Act
        var result = await _httpClient.PostAsJsonAsync(url, request);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ShouldReturnForbidden_WhenAuthorizationFails()
    {
        //Arrange
        var url = ApiEndpoints.SequenceAnalysis.Create;
        var request = new CreateSequenceAnalysisRequest
        {
            UniprotId = "P12345"
        };
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _serverFixture.GenerateToken());

        //Act
        var result = await _httpClient.PostAsJsonAsync(url, request);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenAuthIsSuccessful()
    {
        //Arrange
        var entity = SequenceAnalysisExample.Create().MapToEntity();
        _testDbContext.Add(entity);
        await _testDbContext.SaveChangesAsync();

        var url = ApiEndpoints.SequenceAnalysis.Delete;
        url = url.Replace("{Id}", entity.Id.ToString());

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _serverFixture.GenerateToken(claim: "admin_user"));

        //Act
        var result = await _httpClient.DeleteAsync(url);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_ShouldReturnUnauthorized_WhenAuthenticationFails()
    {
        //Arrange
        var url = ApiEndpoints.SequenceAnalysis.Delete;
        url = url.Replace("{Id}", Guid.NewGuid().ToString());

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _serverFixture.GenerateToken(
                claim: "admin_user",
                changeIssuer: "www.wrongissuer.co.uk"));

        //Act
        var result = await _httpClient.DeleteAsync(url);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Delete_ShouldReturnForbidden_WhenAuthorizationFails()
    {
        //Arrange
        var url = ApiEndpoints.SequenceAnalysis.Delete;
        url = url.Replace("{Id}", Guid.NewGuid().ToString());

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _serverFixture.GenerateToken(
                claim: "trusted_user"));

        //Act
        var result = await _httpClient.DeleteAsync(url);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }


}
