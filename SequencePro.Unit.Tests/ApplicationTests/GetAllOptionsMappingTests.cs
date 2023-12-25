using SequencePro.API.Mapping;
using SequencePro.Application.Models;
using SequencePro.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Unit.Tests.ApplicationTests;
public class GetAllOptionsMappingTests
{
    private readonly GetAllSequenceAnalysisOptions _options = new GetAllSequenceAnalysisOptions();

    [Fact]
    public void SortByPlusSign_ShouldMapToAscendingOrder()
    {
        //Arrange
        var request = new GetAllSequenceAnalysisRequest
        {
            SortBy = "+SequenceLength"
        };

        //Act
        var options = request.MapToOptions();

        //Assert
        options.SortOrder = SortOrder.Ascending;
    }

    [Fact]
    public void SortByMinusSign_ShouldMapToDescendingOrder()
    {
        //Arrange
        var request = new GetAllSequenceAnalysisRequest
        {
            SortBy = "-SequenceLength"
        };

        //Act
        var options = request.MapToOptions();

        //Assert
        options.SortOrder = SortOrder.Descending;
    }

    [Fact]
    public void SortByWithoutSign_ShouldMapToDescendingOrder()
    {
        //Arrange
        var request = new GetAllSequenceAnalysisRequest
        {
            SortBy = "SequenceLength"
        };

        //Act
        var options = request.MapToOptions();

        //Assert
        options.SortOrder = SortOrder.Unsorted;
    }
}
