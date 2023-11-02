using BankTransactionsAPI.Models;
using RestSharp;
using BankTransactionsAPI.Utilities;
using System.Configuration;
using Newtonsoft.Json;

namespace BankTransactionsAPI.Api
{
    public class RestServices
    {
        public RestClient Client { get; private set; } 

        public RestRequest request { get; private set; }
        public int AccountNumber = 0;
        public double Balance = 0;
        public string AccountName = string.Empty;
        public string Address = string.Empty;
        public RestServices() { 
            Client = new RestClient();
            request = new RestRequest();
        }

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
            // Define Request Client
            Client = new RestClient(baseUrl);
        }
        public RestResponse CreateAccountAPI(double balance, string accountName, string address)
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
            
            //return response
            return response;
        }
        public RestResponse DepositORWithDrawAmount(int accountNumber, double balance,string transactionType)
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

            //process the request
            var response = Client.Execute(request);
           
            //return response
            return response;    
        }
        
        public RestResponse getAccount(int accountNumber)
        {
            // Defining the request
            var request = new RestRequest(Data.viewResource + $"/{accountNumber}", Method.Get);

            //process the request
            var response = Client.Execute(request);

            //Return response
            return response;
        }
        
        public RestResponse DeleteAccount(int accountNumber)
        {
            // Defining the request
            var request = new RestRequest(Data.deleteResource + $"/{accountNumber}", Method.Delete);

            //process the request
            var response = Client.Execute(request);

            //return response
            return response;
        }
    }
}
