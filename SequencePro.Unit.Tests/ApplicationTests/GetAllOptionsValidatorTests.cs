using FluentAssertions;
using FluentValidation;
using SequencePro.Application.Models;
using SequencePro.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Unit.Tests.ApplicationTests;
public class GetAllOptionsValidatorTests
{
    private IValidator<GetAllSequenceAnalysisOptions> _sut =
        new GetAllSequenceAnalysisOptionsValidator();

    [Theory]
    [InlineData("P1234583927")]
    [InlineData("P145")]
    public void InvalidUniprotIds_ShouldThrow(string uniprotId)
    {
        //Arrange
        var options = new GetAllSequenceAnalysisOptions
        {
            FilterByUniprotId = uniprotId
        };

        //Act
        var action = () => _sut.ValidateAndThrow(options);

        //Assert
        action.Should().Throw<ValidationException>()
            .WithMessage("*UniprotID / Accession number should be 6 or 10 alphanumeric characters.*");
    }

    [Theory]
    [InlineData("P12345")]
    [InlineData("A0A023GPI8")]
    public void ValidUniprotIds_ShouldNotThrow(string uniprotId)
    {
        //Arrange
        var options = new GetAllSequenceAnalysisOptions
        {
            FilterByUniprotId = uniprotId
        };

        //Act
        var action = () => _sut.ValidateAndThrow(options);

        //Assert
        action.Should().NotThrow();
    }

    [Theory]
    [InlineData("AminoAcidComposition")]
    [InlineData("ProteinSequence")]
    [InlineData("MolecularMass")]
    public void InvalidSortByFields_ShouldThrow(string sortField)
    {
        //Arrange
        var options = new GetAllSequenceAnalysisOptions
        {
            SortField = sortField
        };

        //Act
        var action = () => _sut.ValidateAndThrow(options);

        //Assert
        action.Should().Throw<ValidationException>();
    }

    [Theory]
    [InlineData("UniprotId")]
    [InlineData("SequenceLength")]
    [InlineData("MolecularWeight")]
    public void AcceptedSortByFields_ShouldNotThrow(string sortField)
    {
        //Arrange
        var options = new GetAllSequenceAnalysisOptions
        {
            SortField = sortField
        };

        //Act
        var action = () => _sut.ValidateAndThrow(options);

        //Assert
        action.Should().NotThrow();
    }





}
