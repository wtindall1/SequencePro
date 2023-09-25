using Microsoft.EntityFrameworkCore;
using Sequence_Pro.Application.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Database;
public class SequenceProContext : DbContext
{
    public DbSet<SequenceAnalysisEntity> SequenceAnalyses { get; set; }
    
    public SequenceProContext(DbContextOptions<SequenceProContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SequenceAnalysisEntity>()
            .Property(e => e.AminoAcidComposition)
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<Dictionary<char,double>>(v, new JsonSerializerOptions())!)
            .HasColumnType("json");
    }
}
