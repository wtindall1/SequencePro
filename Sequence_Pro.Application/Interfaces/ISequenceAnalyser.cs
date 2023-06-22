using System;
using Sequence_Pro.Application.Models;

namespace Sequence_Pro.Application.Interfaces;

public interface ISequenceAnalyser
{
	public SequenceAnalysis Analyse(Sequence sequence);
}
