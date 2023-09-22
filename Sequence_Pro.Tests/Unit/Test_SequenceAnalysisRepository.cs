using Dapper;
using Moq;
using Moq.Dapper;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Application.Repositories;
using Sequence_Pro.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sequence_Pro.Tests.Unit;
public class Test_SequenceAnalysisRepository
{
    private readonly SequenceAnalysisRepository _sut;
    private static SequenceAnalysis _sequenceAnalysis = SequenceAnalysisExample.Create();


    public Test_SequenceAnalysisRepository()
    {
        
    }

    [Fact]
    public async Task Test_CreateAsync_Returns_True_When_Record_Created()
    {
        
    }

    [Fact]
    public async Task Test_CreateAsync_Returns_False_When_No_Record_Created()
    {
        
    }

    [Fact]
    public async Task Test_GetByIdAsync_Returns_SequenceAnalysis_When_Record_Found()
    {

    }

    [Fact]
    public async Task Test_GetByIdAsync_Returns_Null_When_No_Record_Found()
    {

    }

    [Fact]
    public async Task Test_GetByUniprotIdAsync_Returns_SequenceAnalysis_When_Record_Found()
    {

    }

    [Fact]
    public async Task Test_GetByUniprotIdAsync_Returns_Null_When_No_Record_Found()
    {

    }

    [Fact]
    public async Task Test_GetAllAsync_Returns_IEnumerable_Of_SequenceAnalysis_When_Records_Found()
    {

    }

    [Fact]
    public async Task Test_GetAllAsync_Returns_Empty_IEnumerable_Of_SequenceAnalysis_When_No_Records_Found()
    {

    }

    [Fact]
    public async Task Test_DeleteByIdAsync_Returns_SequenceAnalysis_When_Record_Found()
    {

    }

    [Fact]
    public async Task Test_DeleteByIdAsync_Returns_Null_When_No_Record_Found()
    {

    }
}
