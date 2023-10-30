using FluentAssertions.Common;
using System.Configuration;

namespace BankTransactionsAPI.Utilities
{
    public class Data
    {
        public static string BankBaseURL => ConfigurationManager.AppSettings["BankBaseURL"];

        public static string createResource => ConfigurationManager.AppSettings["create"];
        public static string depositResource => ConfigurationManager.AppSettings["deposit"];
        public static string withdrawResource => ConfigurationManager.AppSettings["withdraw"];
        public static string deleteResource => ConfigurationManager.AppSettings["delete"];
        public static string viewResource => ConfigurationManager.AppSettings["view"];



    }
}
