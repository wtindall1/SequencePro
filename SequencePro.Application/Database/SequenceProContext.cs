using Microsoft.EntityFrameworkCore;
using SequencePro.Application.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SequencePro.Application.Database;
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
            .HasMany(e => e.AminoAcidCompositions)
            .WithOne()
            .HasForeignKey("SequenceAnalysisId");
    }
}
