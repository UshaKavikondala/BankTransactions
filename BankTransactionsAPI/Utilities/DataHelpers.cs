using System.Configuration;

internal static class DataHelpers
{
    public static string BankBaseURL => ConfigurationManager.AppSettings["create"];
}