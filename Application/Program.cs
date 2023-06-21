using System;
using System.Net.Http;
using Application.Services;


public class Program
{
	public static async Task Main()
	{
		await UniprotAPI.GetSequenceDetails("P12345", new HttpClient());
	}
}
