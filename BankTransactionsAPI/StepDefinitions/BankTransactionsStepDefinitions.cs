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
            //Identify the existing balance and set the balance to be more than the existing amount.
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
            //Identify the existing balance in the account given and calculate the balance in such a way that when deducted than the Balance amount will be less than 100$
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
            //Identify the existing balance and calculate percent amount and set the balance to more than the percent amount
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
            //Identify the existing balance and if that is less than 100$ then set the balance deducted as 10.
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
            //Calling CreateAccount Service API
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
            //Create Multiple Accounts and then add the returned Account Number to a List.
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
            // Verify Account Details exist in the system after creation.
            customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            response = services.getAccount(customer.AccountNumber);
            if ((int)response.StatusCode == 200)
            {
                Console.WriteLine("Account got created successfully and available in System");
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            Assert.NotZero(customer.AccountNumber);
            Assert.AreEqual(services.Balance, customer.Balance);
            Assert.AreEqual(services.Address, customer.Address);
            Assert.AreEqual(services.AccountName, customer.AccountName);
            Console.WriteLine("Account is created Successfully, Account Number is: " + expectedAccountNumber);
        }

        [Then(@"Verify the Balance in the Account After ""([^""]*)""")]
        public void ThenVerifyTheBalanceInTheAccountAfter(string TransactionType)
        {
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

        [Then(@"Verify the Account id Deleted")]
        public void ThenVerifyTheAccountIdDeleted()
        {
            //If system is failed to retrive the account means it is already deleted in the system
            response = services.getAccount(customer.AccountNumber);
            if ((int)response.StatusCode == 400)
            Console.WriteLine("Account Deleted Successfully");
        }

        [Then(@"Verify Account Number is Unique")]
        public void ThenVerifyAccountNumberIsUnique()
        {
            //Verifying Account Numbers list has unique values thru Distict and count methods.
            bool areUnique = accountNumbers.Count == accountNumbers.Distinct().Count();
            Assert.IsTrue(areUnique, "Account numbers are not unique.");
            Console.WriteLine("Account Numbers are Unique");
        }

        [Then(@"Verify account details")]
        public void ThenVerifyAccountDetails()
        {
            //Verify Account Number that we are pssing matches to the received Customer Account Number
            customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            if (services.AccountNumber == customer.AccountNumber)
             {
                //Verify all the Details of that Account Number are retrived.
                Console.WriteLine("Account Found");
                Assert.Equals(services.AccountNumber, customer.AccountNumber);
                Assert.NotNull(customer.Balance);
                Assert.IsNotEmpty(customer.AccountName);
                Assert.IsNotEmpty(customer.Address);
             }
             else
             {
                //Failed to Find Account Number
                 Console.WriteLine("Failed to Find the account.");
                 Assert.AreNotEqual(services.AccountNumber, customer.AccountNumber);
             }
        }
           

        [Then(@"Verify the Account")]
        public void ThenVerifyTheAccount()
        {
            //Verify Account exist by verifying the response
            response = services.getAccount(customer.AccountNumber);
            if ((int)response.StatusCode == 400)
                Console.WriteLine("Account Number does not exist");
        }

        [Then(@"Verify the response code is (.*)")]
        public void ThenVerifyTheResponseCodeIs(int responseCode)
        {
            //Verify Response Code
            Assert.Equals(responseCode,(int)response.StatusCode);
        }
        

    }
}
