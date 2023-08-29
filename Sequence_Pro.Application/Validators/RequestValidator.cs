using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Validators;
public class RequestValidator : AbstractValidator<string>
{
    public RequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithName("UniprotId")
            .WithMessage("UniprotId must not be empty");

        RuleFor(x => x)
            .Must(value => value == null || value.Length == 6 || value.Length == 10)
            .WithName("UniprotId")
            .WithMessage("UniprotID / Accession number should be 6 or 10 alphanumeric characters.");

    }
}
