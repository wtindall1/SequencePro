using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sequence_Pro.Application.Models;

namespace Sequence_Pro.Application.Interfaces;

public interface IUniprotAPI
{ 

    public Task<Sequence> GetSequenceDetails(string uniprotId, HttpClient httpclient, CancellationToken token = default);

}