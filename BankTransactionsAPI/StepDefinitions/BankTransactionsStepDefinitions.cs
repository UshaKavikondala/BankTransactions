using BankTransactionsAPI.Api;
using BankTransactionsAPI.Models;
using FluentAssertions.Common;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using TechTalk.SpecFlow;

namespace BankTransactionsAPI.StepDefinitions
{
    [Binding]
    public class BankTransactionsStepDefinitions
    {
        RestServices services;
        int expectedAccountNumber = 0;
        double balanceBeforeUpdate = 0;
        double balanceAfterUpdate = 0;
        private List<int> accountNumbers = new List<int>();
        private CustomerDetails customer;
        private RestResponse response;

        public BankTransactionsStepDefinitions()
        {
            services = new RestServices();
            services.BankApiServices();
            customer = new CustomerDetails();
            response = new RestResponse();
        }

        [Given(@"Account Initial Balance is \$(.*)")]
        public void GivenAccountInitialBalanceIs(int balance)
        {
            services.Balance = balance;
        }

        [Given(@"Account name is ""([^""]*)""")]
        public void GivenAccountNameIs(string accountName)
        {
            services.AccountName = accountName;
        }

        [Given(@"Address is ""([^""]*)""")]
        public void GivenAddressIs(string address)
        {
            services.Address = address;
        }

        [Given(@"Account Number is (.*)")]
        public void GivenAccountNumberIs(int accountNumber)
        {
            services.AccountNumber = accountNumber;
        }

        [Given(@"Balance Entered is \$(.*)")]
        public void GivenBalanceEnteredIs(double balance)
        {
            services.Balance = balance;
        }

        [Given(@"Balance Entered is Greater Than the Account Balance")]
        public void GivenBalanceEnteredIsGreaterThanTheAccountBalance()
        {
            response = services.getAccount(services.AccountNumber);
            if ((int)response.StatusCode == 200)
            {
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            balanceBeforeUpdate = customer.Balance;
            services.Balance = balanceBeforeUpdate + 100;
        }

        [Given(@"Balance Entered is The Amount Less Than Minimum Required\.")]
        public void GivenBalanceEnteredIsTheAmountLessThanMinimumRequired_()
        {
            response = services.getAccount(services.AccountNumber);
            if ((int)response.StatusCode == 200)
            {
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            balanceBeforeUpdate = customer.Balance;
            services.Balance = balanceBeforeUpdate - 10;
        }

        [Given(@"Balance in the Account is More than the Percent allowed to withdraw\.")]
        public void GivenBalanceInTheAccountIsMoreThanThePercentAllowedToWithdraw_()
        {
            response = services.getAccount(expectedAccountNumber);
            if ((int)response.StatusCode == 200)
            {
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            double percentAmount = customer.Balance * 0.9;
            services.Balance = percentAmount + 10;
        }

        [Given(@"Existing Balance is Less Than \$(.*)")]
        public void GivenExistingBalanceIsLessThan(double balance)
        {
            response = services.getAccount(expectedAccountNumber);
            if ((int)response.StatusCode == 200)
            {
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            if (customer.Balance < 100)
            {
                services.Balance = balance - 90;
            }
            else
            {
                balanceBeforeUpdate = customer.Balance;
                services.Balance = balanceBeforeUpdate - 10;
            }
        }

        [When(@"Create a new account")]
        public void WhenCreateANewAccount()
        {
            response = services.CreateAccountAPI(services.Balance, services.AccountName, services.Address);
        }

        [When(@"""([^""]*)"" Amount")]
        public void WhenAmount(string TransactionType)
        {
            //Deposit/WithDraw Amount from Account
            response = services.getAccount(services.AccountNumber);
            if ((int)response.StatusCode == 200)
            {
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            balanceBeforeUpdate = customer.Balance;
            response = services.DepositORWithDrawAmount(services.AccountNumber, services.Balance, TransactionType);
            if ((int)response.StatusCode == 200)
            {
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            balanceAfterUpdate = customer.Balance;
        }

        [When(@"Delete Account")]
        public void WhenDeleteAccount()
        {
            //Calling DeleteAccount API services with the Account Number
            response = services.DeleteAccount(services.AccountNumber);
        }

        [When(@"Create a new account for Multiple Users")]
        public void WhenCreateANewAccountForMultipleUsers()
        {
            response = services.CreateAccountAPI(services.Balance, services.AccountName, services.Address);
            if ((int)response.StatusCode == 200)
            {
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            expectedAccountNumber = customer.AccountNumber;
            accountNumbers.Add(expectedAccountNumber);
        }

        [When(@"Get Account Details")]
        public void WhenGetAccountDetails()
        {
            //calling getAccount API services to retrieve Account Details
            response = services.getAccount(services.AccountNumber);
        }

        [Then(@"Verify Account Creation")]
        public void ThenVerifyAccountCreation()
        {
            if (response.IsSuccessful)
            {
                Assert.AreEqual(200, (int)response.StatusCode);
                response = services.CreateAccountAPI(services.Balance, services.AccountName, services.Address);
                if ((int)response.StatusCode == 200)
                {
                    customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
                }
                Assert.NotZero(customer.AccountNumber);
                Assert.AreEqual(services.Balance, customer.Balance);
                Assert.AreEqual(services.Address, customer.Address);
                Assert.AreEqual(services.AccountName, customer.AccountName);
                Console.WriteLine("Account is created Successfully, Account Number is: " + expectedAccountNumber);
            }
            else
            {
                Assert.AreEqual(400, (int)response.StatusCode);
                Console.WriteLine("Account is not created ");
            }
        }

        [Then(@"Verify the Balance in the Account After ""([^""]*)""")]
        public void ThenVerifyTheBalanceInTheAccountAfter(string TransactionType)
        {
            if (response.IsSuccessful)
            {
                Assert.AreEqual(200, (int)response.StatusCode);
                response = services.CreateAccountAPI(services.Balance, services.AccountName, services.Address);
                if ((int)response.StatusCode == 200)
                {
                    customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
                }
                //Verify Balance for both Deposit and Withdraw Transactions
                if (TransactionType.ToUpper().Equals("DEPOSITE"))
                {
                    Assert.AreEqual(balanceBeforeUpdate + services.Balance, balanceAfterUpdate, "Amount Deposited Successfully");
                }
                if (TransactionType.ToUpper().Equals("WITHDRAW"))
                {
                    Assert.AreEqual(balanceBeforeUpdate - services.Balance, balanceAfterUpdate, "Amount is Successfully Withdrawl from the account");
                }
            }
            else
                throw new Exception("Account Number does not exist or Not able to retrieve account details");
        }

        [Then(@"Verify the Account id Deleted")]
        public void ThenVerifyTheAccountIdDeleted()
        {
            if (response.IsSuccessful)
            {
                Assert.AreEqual(200, (int)response.StatusCode);
                Console.WriteLine("Account Deleted Successfully");
            }
            else
            {
                Assert.AreEqual(400, (int)response.StatusCode);
                Console.WriteLine("Account Does not exist or failed to delete the account");
            }
        }

        [Then(@"Verify Account Number is Unique")]
        public void ThenVerifyAccountNumberIsUnique()
        {
            Assert.AreEqual(200, (int)response.StatusCode);
            bool areUnique = accountNumbers.Count == accountNumbers.Distinct().Count();
            Assert.IsTrue(areUnique, "Account numbers are not unique.");
            Console.WriteLine("Account Numbers are Unique");
        }

        [Then(@"Verify account details")]
        public void ThenVerifyAccountDetails()
        {
            if (response.IsSuccessful)
            {
                Assert.AreEqual(200, (int)response.StatusCode);

                if (services.AccountNumber == customer.AccountNumber)
                {
                    Console.WriteLine("Account Found");
                    Assert.Equals(services.AccountNumber, customer.AccountNumber);
                    Assert.NotNull(customer.Balance);
                    Assert.IsNotEmpty(customer.AccountName);
                    Assert.IsNotEmpty(customer.Address);
                }
                else
                {
                    Console.WriteLine("Failed to Find the account.");
                    Assert.AreNotEqual(services.AccountNumber, customer.AccountNumber);

                }
            }
            else
            {
                Assert.AreEqual(400, (int)response.StatusCode);
                Console.WriteLine("Failed to Retrive the Accounts");
            }
        }

        [Then(@"Verify the Account")]
        public void ThenVerifyTheAccount()
        {
            if (!response.IsSuccessful)
            {
                Assert.AreEqual(400, (int)response.StatusCode);
                Console.WriteLine("Failed to Delete the Account/ Account Number does not exist");
            }
        }

        [Then(@"Verify the response code is \$(.*)")]
        public void ThenVerifyTheResponseCodeIs(int responseCode)
        {
            Assert.Equals(responseCode,(int)response.StatusCode);
        }

    }
}
