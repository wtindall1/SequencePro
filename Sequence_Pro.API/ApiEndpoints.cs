namespace Sequence_Pro.API;
public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class SequenceAnalysis
    {
        private const string Base = $"{ApiBase}/sequenceAnalysis";

        public const string Create = Base;
        public const string Get = $"{Base}/{{IdOrUniprotId}}";
    }

}

