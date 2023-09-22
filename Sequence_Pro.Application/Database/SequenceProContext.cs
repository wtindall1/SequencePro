using Microsoft.EntityFrameworkCore;
using Sequence_Pro.Application.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Database;
public class SequenceProContext : DbContext
{
    public DbSet<SequenceAnalysis> SequenceAnalyses { get; set; }
    
    public SequenceProContext(DbContextOptions<SequenceProContext> options) 
        : base(options)
    {
    }
}
