using BankTransactionsAPI.Models;
using System.Security.Principal;
using TechTalk.SpecFlow;

namespace BankTransactionsAPI.Hooks
{
    [Binding]
    public sealed class BankTransactionsHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        private List<Account> mockAccounts = new List<Account>();


        [BeforeScenario("@tag1")]
        public void BeforeScenarioWithTag()
        {
            // Example of filtering hooks using tags. (in this case, this 'before scenario' hook will execute if the feature/scenario contains the tag '@tag1')
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=hooks#tag-scoping

            //TODO: implement logic that has to run before executing each scenario
            mockAccounts.Add(new Account { AccountNumber = 1001, AccountName = "John Doe", Balance = 1000.00, Address = "123 Main St" });
            mockAccounts.Add(new Account { AccountNumber = 1002, AccountName = "Jane Smith", Balance = 1500.00, Address = "456 Elm St" });

        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario()
        {
            // Example of ordering the execution of hooks
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=order#hook-execution-order

            //TODO: implement logic that has to run before executing each scenario
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}