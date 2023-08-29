using System;
using System.Collections.Generic;

namespace Sequence_Pro.Tests.Unit;

public static class DictionaryComparer
{
    public static bool Equals(Dictionary<char, double> dict1, Dictionary<char, double> dict2)
    {
        if (dict1.Count != dict2.Count)
        {
            return false;
        }

        foreach (var keyValuePair in dict1)
        {
            char key = keyValuePair.Key;
            if (!dict2.TryGetValue(key, out double value) || !(value == keyValuePair.Value))
            {
                return false;
            }
        }

        return true;
    }
}