using BankTransactionsAPI.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankTransactionsAPI.Utilities;
using System.ComponentModel.Design;
using NUnit.Framework;
using System.Configuration;
using BankTransactionsAPI;
using Newtonsoft.Json;
using System.Net;

namespace BankTransactionsAPI.Api
{
    public class RestServices
    {
        public RestClient Client { get; private set; } 
        public RestRequest request { get; private set; } 
        public int AccountNumber;
        public string AccountName = string.Empty;
        public double Balance =0;
        public string Address=string.Empty;
        CustomerDetails customer;


        public void BankApiServices()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            string baseUrl = configuration.AppSettings.Settings["BankBaseURL"].Value;

            Console.WriteLine($"Base URL from config: {baseUrl}");

            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("Base URL is not defined in the configuration.");
            }

            Client = new RestClient(baseUrl);

        }
        public int CreateAccountAPI(double balance, string accountName, string address)
        {
            // Defining the request body with customer data
            var customerDTO = new CustomerDetails
            {
                AccountName = accountName,
                Balance = balance,
                Address = address
            };
            // Serializing the DTO to JSON
            string jsonBody = JsonConvert.SerializeObject(customerDTO);

            //Sending the JSON as the request body
            var request = new RestRequest(Data.createResource, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            //process the request
            var response = Client.Execute(request);
            
            //Deserialize the response
            if (!string.IsNullOrEmpty(response.Content))
            {
                customer = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
            }
            else
                throw new Exception("Json Response is Empty");

            if (response.IsSuccessful)
            {
                Assert.AreEqual(200, (int)response.StatusCode);
                Console.WriteLine($"New account created with account number: {customer.AccountNumber}");
                return customer.AccountNumber;
            }
            else
            {
                // Handling the error response
                Console.WriteLine("Failed to create an account.");
                return 0;
            }

        }
        public double DepositORWithDrawAmount(int accountNumber, double balance,string transactionType)
        {
            // Defining the request body with customer data
            var customerDTO = new CustomerDetails
            {
                AccountNumber = accountNumber,
                Balance = balance
            };
            // Serializing the DTO to JSON
            string jsonBody = JsonConvert.SerializeObject(customerDTO);

            
            //Sending the JSON as the request body
            if (transactionType.ToUpper().Equals("DEPOSIT"))
            {
                request = new RestRequest(Data.depositResource + $"/{accountNumber}", Method.Put);
            }
            if (transactionType.ToUpper().Equals("WITHDRAW"))
            {
                request = new RestRequest(Data.withdrawResource + $"/{accountNumber}", Method.Put);
            }
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            var response = Client.Execute(request);
            //Deserialize the response
            var createdAccount = JsonConvert.DeserializeObject<CustomerDetails>(response.Content);

            if (response.IsSuccessful)
            {
                // Handle the success response
                Assert.AreEqual(200, (int)response.StatusCode);
                Console.WriteLine("Balance is updated successfully.");
                return createdAccount.Balance;
            }
            else
            {
                // Handle the error response
                Console.WriteLine("Failed to update the Balance.");
                Assert.Fail("Failed to update the Balance.");
                return 0;
            }

        }
        
        public CustomerDetails? getAccount(int accountNumber)
        {
            var request = new RestRequest(Data.viewResource + $"/{accountNumber}", Method.Get);
            var response = Client.Execute(request);

            if (response.IsSuccessful)
            {
                // Parse and handle the response as needed
                CustomerDetails customer = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerDetails>(response.Content);
                Assert.AreEqual(200, (int)response.StatusCode);
                Console.WriteLine($"Account Number: {customer.AccountNumber}");
                Console.WriteLine($"Account Name: {customer.AccountName}");
                Console.WriteLine($"Balance: {customer.Balance}");
                Console.WriteLine($"Address: {customer.Address}");
                return customer;
            }
            else
            {
                // Handle the error response
                Console.WriteLine("Failed to retrieve account details.");
                return null;
            }

        }
        public bool DeleteAccount(int accountNumber) {
            var request = new RestRequest(Data.deleteResource + $"/{accountNumber}", Method.Delete);

            var response = Client.Execute(request);

            if (response.IsSuccessful)
            {
                // Handle the success response
                Assert.AreEqual(200, (int)response.StatusCode);
                Console.WriteLine("Account deleted successfully.");
                return true;
            }
            else
            {
                // Handle the error response
                Console.WriteLine("Failed to delete the account or Account does not exist");
                return false;
            }

        }
    }
}
