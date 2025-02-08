namespace Ambev.DeveloperEvaluation.PostgreSQL.Configurations;

internal static class PostgreSqlConstants
{
    internal static class Types
    {
        internal const string DateTime = "TIMESTAMP WITHOUT TIME ZONE";
        internal const string Guid = "UUID";
    }

    internal static class DefaultValues
    {
        internal const string Guid = "GEN_RANDOM_UUID()";
    }
}