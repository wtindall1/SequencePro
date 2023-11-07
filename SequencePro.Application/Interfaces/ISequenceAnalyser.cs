using System;
using SequencePro.Application.Models;

namespace SequencePro.Application.Interfaces;

public interface ISequenceAnalyser
{
	public SequenceAnalysis Analyse(Sequence sequence);
}
