using Dapper;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Tests.TestDatabase;
public class TestDbManager
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TestDbManager(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitialiseAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
            create table if not exists Sequences (
            Id UUID primary key,
            UniprotId TEXT not null,
            ProteinSequence TEXT not null,
            SequenceLength integer not null,
            MolecularWeight double precision not null,
            AminoAcidComposition jsonb not null);
            """);

        //create 5 records
        for (int i = 0; i < 5; i++)
        {
            var queryParameters = SequenceAnalysisExample.CreateSequenceAnalysisRecord();

            await connection.ExecuteAsync(new CommandDefinition("""
            insert into Sequences (Id, UniprotId, ProteinSequence, SequenceLength, MolecularWeight, AminoAcidComposition)
            values (@Id, @UniprotId, @ProteinSequence, @SequenceLength, @MolecularWeight, @AminoAcidComposition::jsonb);
            """, queryParameters));
        }
    }

    public async Task ClearTestDbAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(new CommandDefinition("""
            delete from Sequences;
            """));
    }

    public async Task<Guid> PostTestRecord()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var queryParameters = SequenceAnalysisExample.CreateSequenceAnalysisRecord();

        await connection.ExecuteAsync(new CommandDefinition("""
            insert into Sequences (Id, UniprotId, ProteinSequence, SequenceLength, MolecularWeight, AminoAcidComposition)
            values (@Id, @UniprotId, @ProteinSequence, @SequenceLength, @MolecularWeight, @AminoAcidComposition::jsonb);
            """, queryParameters));

        return queryParameters.Id;

        
    }
}
