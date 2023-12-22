using FluentValidation;
using SequencePro.Application.Models;
using SequencePro.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Application.Validators;
public class GetAllSequenceAnalysisOptionsValidator : AbstractValidator<GetAllSequenceAnalysisOptions>
{
    private static readonly string[] AllowedSortFields =
    {
        "UniprotId", "SequenceLength", "MolecularWeight"
    };

    public GetAllSequenceAnalysisOptionsValidator()
    {
        RuleFor(x => x.UniprotId)
            .Must(value => value == null || value.Length == 6 || value.Length == 10)
            .WithName("UniprotId")
            .WithMessage("UniprotID / Accession number should be 6 or 10 alphanumeric characters.");

        RuleFor(x => x.SortField)
            .Must(value => value == null || AllowedSortFields.Contains(value))
            .WithMessage("Sort field is not valid.");
    }
}
