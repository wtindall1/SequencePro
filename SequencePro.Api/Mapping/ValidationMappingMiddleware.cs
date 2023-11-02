using FluentValidation;
using SequencePro.Contracts.Responses;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;

namespace SequencePro.API.Mapping;

public class ValidationMappingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMappingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;

            var validationResponse = new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(x => new ValidationResponse
                {
                    Name = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };

            await context.Response.WriteAsJsonAsync(validationResponse);
        }

        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                context.Response.StatusCode = 400;
            }
            else if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                context.Response.StatusCode = 404;
            }
            else
            {
                context.Response.StatusCode = 500;
                return;
            }

            var validationResponse = new ValidationResponse
            {
                Name = ex.StatusCode?.ToString() ?? "",
                Message = ex.Message
            };

            var errorResponse = new ValidationFailureResponse
            {
                Errors = new[] { validationResponse }
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
