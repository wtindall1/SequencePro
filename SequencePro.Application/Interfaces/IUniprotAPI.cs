using System;
using System.Net.Http;
using System.Threading.Tasks;
using SequencePro.Application.Models;

namespace SequencePro.Application.Interfaces;

public interface IUniprotAPI
{ 
    public Task<Sequence> GetSequenceDetails(string uniprotId, HttpClient httpclient, CancellationToken token = default);
}