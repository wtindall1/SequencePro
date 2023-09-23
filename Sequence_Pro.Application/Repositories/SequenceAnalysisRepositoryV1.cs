﻿using Dapper;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace Sequence_Pro.Application.Repositories;
public class SequenceAnalysisRepositoryV1 : ISequenceAnalysisRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SequenceAnalysisRepositoryV1(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(SequenceAnalysis sequenceAnalysis, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

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
            """, queryParameters, cancellationToken: token));

        transaction.Commit();

        return result > 0;
    }

    public async Task<SequenceAnalysis?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.QuerySingleOrDefaultAsync(new CommandDefinition("""
            select * from Sequences where Id = @id
            """, new { id }, cancellationToken: token));

        if (result is null)
        {
            return null;
        }

        //deserialize jsonb composition from dynamic object
        string json = result.aminoacidcomposition.ToString();
        var aminoAcidComposition = JsonSerializer.Deserialize<Dictionary<char, double>>(json);

        var sequenceAnalysis = new SequenceAnalysis
        {
            Id = result.id,
            UniprotId = result.uniprotid,
            ProteinSequence = result.proteinsequence,
            SequenceLength = result.sequencelength,
            MolecularWeight = result.molecularweight,
            AminoAcidComposition = aminoAcidComposition!
        };

        transaction.Commit();

        return sequenceAnalysis;

    }
    public async Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.QueryFirstOrDefaultAsync(new CommandDefinition("""
            select * from Sequences where UniprotId = @uniprotId
            """, new { uniprotId }, cancellationToken: token));

        if (result is null)
        {
            return null;
        }

        //deserialize jsonb composition from dynamic object
        string json = result.aminoacidcomposition.ToString();
        var aminoAcidComposition = JsonSerializer.Deserialize<Dictionary<char, double>>(json);

        var sequenceAnalysis = new SequenceAnalysis
        {
            Id = result.id,
            UniprotId = result.uniprotid,
            ProteinSequence = result.proteinsequence,
            SequenceLength = result.sequencelength,
            MolecularWeight = result.molecularweight,
            AminoAcidComposition = aminoAcidComposition!
        };

        transaction.Commit();

        return sequenceAnalysis;
    }

    public async Task<IEnumerable<SequenceAnalysis>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.QueryAsync(new CommandDefinition("""
            select * from Sequences;
            """, cancellationToken: token));

        var allAnalyses = result.Select(x => new SequenceAnalysis
        {
            Id = x.id,
            UniprotId = x.uniprotid,
            ProteinSequence = x.proteinsequence,
            SequenceLength = x.sequencelength,
            MolecularWeight = x.molecularweight,
            AminoAcidComposition = JsonSerializer.Deserialize<Dictionary<char, double>>(x.aminoacidcomposition.ToString())
        });

        transaction.Commit();

        return allAnalyses;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete from Sequences where Id = @id
            """, new { id }, cancellationToken: token));

        transaction.Commit();

        return result > 0;
    }
}




