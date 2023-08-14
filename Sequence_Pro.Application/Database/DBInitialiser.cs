﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Sequence_Pro.Application.Database;
public class DBInitialiser
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DBInitialiser(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitialiseAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        //create sequences table
        await connection.ExecuteAsync("""
            create table if not exists Sequences (
            Id UUID primary key
            UniprotId string not null
            ProteinSequence string not null
            SequenceLength integer not null
            MolecularWeight double precision not null
            AminoAcidComposition jsonb not null;
            """);

        //create index for uniprotid
        await connection.ExecuteAsync("""
            create unique index concurrently if not exists Sequences_UniprotId_idx
            on Sequences
            using btree(UniprotId);
            """);


    }
}
