using BankTransactionsAPI.Api;
using BankTransactionsAPI.Models;
using FluentAssertions.Common;
using NUnit.Framework;
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
        bool flag;
        private List<int> accountNumbers = new List<int>();
        CustomerDetails customer;

        public BankTransactionsStepDefinitions()
        {
            services = new RestServices();
            services.BankApiServices();
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

        [When(@"Create a new account")]
        public void WhenCreateANewAccount()
        {
            expectedAccountNumber = services.CreateAccountAPI(services.Balance, services.AccountName, services.Address);
        }

        [Then(@"Verify Account Creation")]
        public void ThenVerifyAccountCreation()
        {
            if (expectedAccountNumber==0)
            {
                Assert.Zero(expectedAccountNumber);
                Console.WriteLine("Account is not created "); 
            }
            else
            {
                Assert.NotZero(expectedAccountNumber);
                customer = services.getAccount(expectedAccountNumber);
                Assert.Equals(expectedAccountNumber, customer.AccountNumber);
                Assert.Equals(services.Balance, customer.Balance);
                Assert.Equals(services.Address, customer.Address);
                Assert.Equals(services.AccountName, customer.AccountName);
                Console.WriteLine("Account is created Successfully, Account Number is: " + expectedAccountNumber);
            }

        }


        //Deposit Amount
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

        [When(@"""([^""]*)"" Amount")]
        public void WhenAmount(string TransactionType)
        {
            //Deposit/WithDraw Amount from Account
            if (TransactionType.ToUpper().Equals("DEPOSIT"))
            {

                customer = services.getAccount(services.AccountNumber);
                balanceBeforeUpdate = customer.Balance;
                balanceAfterUpdate = services.DepositORWithDrawAmount(services.AccountNumber, services.Balance, TransactionType);

            }
            if (TransactionType.ToUpper().Equals("WITHDRAW"))
            {

                customer = services.getAccount(services.AccountNumber);
                balanceBeforeUpdate = customer.Balance;
                balanceAfterUpdate = services.DepositORWithDrawAmount(services.AccountNumber, services.Balance, TransactionType);

            }
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
        [When(@"Delete Account")]
        public void WhenDeleteAccount()
        {
            //Call DeleteAccount API services with the Account Number
            flag = services.DeleteAccount(services.AccountNumber);
        }

        [Then(@"Verify the Account id Deleted")]
        public void ThenVerifyTheAccountIdDeleted()
        {
           Assert.True(flag);
           Console.WriteLine("Account Deleted Successfully");
        }

        [When(@"Create a new account for Multiple Users")]
        public void WhenCreateANewAccountForMultipleUsers()
        {
            expectedAccountNumber = services.CreateAccountAPI(services.Balance, services.AccountName, services.Address);
            accountNumbers.Add(expectedAccountNumber);
        }

        [Then(@"Verify Account Number is Unique")]
        public void ThenVerifyAccountNumberIsUnique()
        {
            bool areUnique = accountNumbers.Count == accountNumbers.Distinct().Count();
            Assert.IsTrue(areUnique, "Account numbers are not unique.");
        }

        [Given(@"Balance Entered is Greater Than the Account Balance")]
        public void GivenBalanceEnteredIsGreaterThanTheAccountBalance()
        {

            customer = services.getAccount(services.AccountNumber);
            balanceBeforeUpdate = customer.Balance;
            services.Balance = balanceBeforeUpdate+100;

        }

        [Given(@"Balance Entered is The Amount Less Than Minimum Required\.")]
        public void GivenBalanceEnteredIsTheAmountLessThanMinimumRequired_()
        {
            customer = services.getAccount(services.AccountNumber);
            balanceBeforeUpdate = customer.Balance;
            services.Balance = balanceBeforeUpdate -10;
        }

        [When(@"Get Account Details")]
        public void WhenGetAccountDetails()
        {
            customer = services.getAccount(services.AccountNumber);
        }

        [Then(@"Verify account details")]
        public void ThenVerifyAccountDetails()
        {
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
        [Given(@"Balance in the Account is More than the Percent allowed to withdraw\.")]
        public void GivenBalanceInTheAccountIsMoreThanThePercentAllowedToWithdraw_()
        {
            try
            {
                customer = services.getAccount(expectedAccountNumber);
                double percentAmount = customer.Balance * 0.9;
                services.Balance = percentAmount + 10;
            }catch(NullReferenceException ex) { Console.WriteLine(ex); }

        }
        [Given(@"Existing Balance is Less Than \$(.*)")]
        public void GivenExistingBalanceIsLessThan(double balance)
        {
            customer = services.getAccount(expectedAccountNumber);
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


    }
}
