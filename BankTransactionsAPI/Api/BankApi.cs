using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BankTransactionsAPI.Models;

namespace BankTransactionsAPI.Api
{
    public class BankApi
    {
        private Dictionary<int, CustomerDetails> accountDetails = new Dictionary<int, CustomerDetails>();
        private int accountNumber = 1000;

        public int CreateAccount(double balance, string accountName, string address)
        {
            if (balance > 0)
            {
                if (balance >= 100 && balance <= 10000)
                {
                    accountNumber++;
                    var customer = new CustomerDetails
                    {
                        AccountNumber = accountNumber,
                        AccountName = accountName,
                        Balance = balance,
                        Address = address
                    };
                    accountDetails.Add(accountNumber, customer);
                    return accountNumber;
                }
                else if (balance < 100)
                    throw new ArgumentException("Can not create account due to insufficiant minimum balance");
                else
                {
                    throw new ArgumentException("User cannot deposit more than $10,000 in a single transaction");
                }
            }else { throw new ArgumentException("Balance Amount should be greater than zero"); }
        }

        public void DepositAmount(int account, double amount)
        {
            if (accountDetails.ContainsKey(account))
            {

                if (amount > 0)
                {
                    if (amount <= 10000)
                    {
                        CustomerDetails customer = (CustomerDetails)accountDetails[account];
                        customer.Balance += amount;
                        Console.WriteLine("Amount Deposited into account! Balance is: " + customer.Balance + "\n");
                    }
                    else
                    {
                        throw new ArgumentException("A user cannot deposit more than $10,000 in a single transaction.");

                    }
                }
                else
                {
                    throw new ArgumentException("Amount should be greater than zero.");
                }
            }
            else
                throw new ArgumentException("Account does not exist:Please create account first");
        }

        public void Withdraw(int account, double amount)
        {
            if (accountDetails.ContainsKey(account))
            {

                if (amount > 0)
                {
                    double percentAmount = 0;
                    CustomerDetails customer = (CustomerDetails)accountDetails[account];
                    percentAmount = customer.Balance * (0.9);
                    if (customer.Balance < 100)
                    {
                        throw new ArgumentException("Can not WithDraw Amount: InSufficiant funds in Account, Need to maintain min $100 in Account");
                    }
                    else if (customer.Balance < amount)
                    {
                        throw new ArgumentException("Can not withdraw, amount is greater than the balance in account. Your account has " + customer.Balance + "$\n");
                    }
                    else if (amount <= percentAmount)
                    {
                        if ((customer.Balance - amount) < 100)
                        {
                            throw new ArgumentException("Can not withdraw money, The Balance amount will become less than 100$ after withdrawl.");
                        }
                        else
                        {
                            customer.Balance -= amount;
                            Console.WriteLine("Amount Successfully withdrawn from your account, balance is:" + customer.Balance + "\n");
                        }
                    }
                    else
                        throw new ArgumentException("cannot withdraw more than 90% of your total balance from an account in a single transaction, Maximum amout you can withdraw is: " + percentAmount + "\nNote: Minimum balance should be 100$ in account");
                }
                else
                    throw new ArgumentException("Please enter amount greater than zero.");
            }
            else
                throw new ArgumentException("Account does not exist:Please create account first");

        }
    
        public bool DeleteAccount(int account)
        {
            if (accountDetails.ContainsKey(account))
            {
                try
                {
                    accountDetails.Remove(account);
                }
                catch (Exception e)
                { Console.WriteLine(e.Message); }
                return true;
            }
            else
                return false;
        }
        public CustomerDetails? getAccountDetails(int account)
        {
            if (accountDetails.ContainsKey(account))
            {
                CustomerDetails customer = (CustomerDetails)accountDetails[account];
                Console.WriteLine("Account Number is:" + customer.AccountNumber + "Balance in your account is:" + customer.Balance);
                return customer;
            }
            else
            {
                Console.WriteLine("Account does not exist!\n");
                return null;
            }

        }
    }
}
