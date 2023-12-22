using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Application.Models;
public class GetAllSequenceAnalysisOptions
{
    public string? UniprotId { get; set; }

    public string? SortField { get; set; }

    public SortOrder? SortOrder { get; set; }
}
