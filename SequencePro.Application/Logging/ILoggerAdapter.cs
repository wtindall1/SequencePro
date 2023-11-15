using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Application.Logging;
public interface ILoggerAdapter
{
    public void LogInformation(string? message, params object?[] args);

    public void LogError(Exception? exception, string? message, params object?[] args);
}
