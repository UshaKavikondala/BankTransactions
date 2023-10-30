using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankTransactionsAPI.Models;

namespace BankTransactionsAPI.Api
{
    public class BankApi
    {
        private Dictionary<int, Account> accountDetails = new Dictionary<int, Account>();
        private int accountNumber = 1000;

        public int CreateAccount(double balance, string accountName, string address)
        {
            if (balance >= 100 && balance <= 10000)
            {
                accountNumber++;
                var customer = new Account
                {
                    AccountNumber = accountNumber,
                    AccountName = accountName,
                    Balance = balance,
                    Address = address
                };
                accountDetails[accountNumber] = customer;
                return accountNumber;
            }
            else
            {
                throw new ArgumentException("Invalid balance. Minimum balance must be $100, and maximum is $10,000.");
            }
        }

    }
}
