using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Contracts.Responses;
public class ValidationFailureResponse
{
    public required IEnumerable<ValidationResponse> Errors { get; set; }
}

public class ValidationResponse
{
    public required string Name { get; init; }

    public required string Message { get; init; }
}
