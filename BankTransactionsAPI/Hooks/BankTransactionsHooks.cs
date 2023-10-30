using BankTransactionsAPI.Api;
using BankTransactionsAPI.Models;
using System.Security.Principal;
using TechTalk.SpecFlow;

namespace BankTransactionsAPI.Hooks
{
    [Binding]
    public sealed class BankTransactionsHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
       
        RestServices services = new RestServices();


        [BeforeScenario("@withdraw","@deposit","@delete")]
        public void BeforeScenarioWithTag()
        {
            if (services.AccountNumber == 0)
            {
                services.Balance = 200;
                services.AccountName = "Simha";
                services.Address = "Bangalore";
                services.CreateAccountAPI(services.Balance, services.AccountName, services.Address);

                services.Balance = 150;
                services.AccountName = "Chaitanys";
                services.Address = "Hyderabad";
                services.CreateAccountAPI(services.Balance, services.AccountName, services.Address);
            }
           
        }
    }
}