using System;

namespace Application.Services;

public class Sequence
{

    public string _uniqueIdentifier { get; }
    public string _entryName { get; }
    public string _proteinName { get; }
    public string _organismName { get; }
    public string _aminoAcidSequence { get; }


    public Sequence(string uniqueIdentifier,
                    string entryName,
                    string proteinName,
                    string organismName,
                    string aminoAcidSequence)
	{
        _uniqueIdentifier = uniqueIdentifier;
        _entryName = entryName;
        _proteinName = proteinName;
        _organismName = organismName;
        _aminoAcidSequence = aminoAcidSequence;
	}
}


