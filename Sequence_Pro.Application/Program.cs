using System;
using System.Net.Http;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Models;
using System.Text.Json;

namespace Sequence_Pro.Application;

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
