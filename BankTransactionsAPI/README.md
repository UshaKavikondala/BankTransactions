# Bank Transactions Project

## Description
A SpecFlow and RestShart project for managing bank account transactions and balances.

## Prerequisites
- Visual Studio
- SpecFlow
- RestSharp

### Built With
The Framework built with below main packages
* [SpecFlow](https://specflow.org/)
* [RestSharp](http://restsharp.org)
* [FluentAssertions](https://fluentAssertions.com)
* [NUnit](https://nunit.org)

## Installation
1. Clone the repository.
2. Open the project in Visual Studio.
3. Restore NuGet packages.
4. Build and run the project.

## Usage
1. Set up your test scenarios in SpecFlow feature files.
2. Run the scenarios to test bank transactions.

## Features
- Create accounts
- Deposit and withdraw Amount from Account
- View account details
- Delete accounts

## Tests:
- Create Account with Valid values
- Create Account With Minimum Balance Less Than Minimum Required
- Create Multiple Accounts
- Create Account With Balance Greater Than Transaction Limit
- Create Account With Negative Balance
- Deposite Amount to Account with Valid Values
- Deposit A Negative Amount
- Deposit Amount Greater Than Transaction Limit
- Deposit Amount To NonExisting Account
- Withdraw Amount to Account with Valid Values
- Withdraw NegativeAmount
- Withdraw Amount Greater Than the Account Balance
- Withdraw Amount more than 90% of the Account Balance
- Withdraw Amount From NonExisting Account
- Withdraw Amount With A Balance Less Than Minimum Required
- Withdraw Amount If Balance is Less then $100
- Delete Account
- Delete Nonexisting Acoount 
- Delete Multiple Accounts
- View the Required Account Details 
- View Multiple Accounts
- View Account Details that doesn't exist 


## Usage
To run tests use Test Explorer


## License
This project is licensed under the MIT License.

## Contact Information
For questions or support, contact kavikondala.usha20@gmail.com.


