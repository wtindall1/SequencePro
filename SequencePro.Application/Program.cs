using System;
using System.Net.Http;
using SequencePro.Application.Services;
using SequencePro.Application.Models;
using System.Text.Json;

namespace SequencePro.Application;

public class Program
{
	public static async Task Main()
	{
		var uniprot = new UniprotAPI();
		var sequence = await uniprot.GetSequenceDetails("P12563", new HttpClient());
		var sequenceAnalyser = new SequenceAnalyser();

		SequenceAnalysis analysis = sequenceAnalyser.Analyse(sequence);

		return;
	}
}
