using Dapper;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Repositories;
public class SequenceAnalysisRepository : ISequenceAnalysisRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SequenceAnalysisRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(SequenceAnalysis sequenceAnalysis)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();

        var queryParameters = new
        {
            Id = sequenceAnalysis.Id,
            UniprotId = sequenceAnalysis.UniprotId,
            ProteinSequence = sequenceAnalysis.ProteinSequence,
            SequenceLength = sequenceAnalysis.SequenceLength,
            MolecularWeight = sequenceAnalysis.MolecularWeight,
            AminoAcidComposition = JsonSerializer.Serialize(sequenceAnalysis.AminoAcidComposition)
        };

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into Sequences (Id, UniprotId, ProteinSequence, SequenceLength, MolecularWeight, AminoAcidComposition)
            values (@Id, @UniprotId, @ProteinSequence, @SequenceLength, @MolecularWeight, @AminoAcidComposition::jsonb);
            """, queryParameters ));

        transaction.Commit();

        return result > 0;
        
    }

    public async Task<SequenceAnalysis?> GetByIdAsync(Guid id)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();

        var result = await connection.QuerySingleOrDefaultAsync("""
            select * from Sequences where Id = @id
            """, new { id });

        if (result is null)
        {
            return null;
        }

        //deserialize jsonb composition from dynamic object
        string json = result.AminoAcidComposition.ToString();
        var aminoAcidComposition = JsonSerializer.Deserialize<Dictionary<char, double>>(json);

        var sequenceAnalysis = new SequenceAnalysis
        {
            Id = result.Id,
            UniprotId = result.UniprotId,
            ProteinSequence = result.ProteinSequence,
            SequenceLength = result.SequenceLength,
            MolecularWeight = result.MolecularWeight,
            AminoAcidComposition = aminoAcidComposition!
        };

        transaction.Commit();

        return sequenceAnalysis;

    }

    public async Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();

        var result = await connection.QuerySingleOrDefaultAsync("""
            select * from Sequences where UniprotId = @uniprotId
            """, new { uniprotId });

        if (result is null)
        {
            return null;
        }

        //deserialize jsonb composition from dynamic object
        string json = result.AminoAcidComposition.ToString();
        var aminoAcidComposition = JsonSerializer.Deserialize<Dictionary<char, double>>(json);

        var sequenceAnalysis = new SequenceAnalysis
        {
            Id = result.Id,
            UniprotId = result.UniprotId,
            ProteinSequence = result.ProteinSequence,
            SequenceLength = result.SequenceLength,
            MolecularWeight = result.MolecularWeight,
            AminoAcidComposition = aminoAcidComposition!
        };

        transaction.Commit();

        return sequenceAnalysis;
    }

    public async Task<IEnumerable<SequenceAnalysis>> GetAllAsync()
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();

        var result = await connection.QueryAsync(new CommandDefinition("""
            select * from Sequences;
            """));

        var allAnalyses = result.Select(x => new SequenceAnalysis
        {
            Id = x.Id,
            UniprotId = x.UniprotId,
            ProteinSequence = x.ProteinSequence,
            SequenceLength = x.SequenceLength,
            MolecularWeight = x.MolecularWeight,
            AminoAcidComposition = JsonSerializer.Deserialize<Dictionary<char, double>>(x.AminoAcidComposition.ToString())
        });

        transaction.Commit();

        return allAnalyses;




    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete from Sequences where Id = @id
            """, new { id }));

        transaction.Commit();

        return result > 0;
    }

    

    


}





