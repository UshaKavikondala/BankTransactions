Feature: BankTransactions

  Manages bank account transactions and balances
Background:
    Given Account Number is 1001

	
@tag1
Scenario: Create Account
	Given Account Initial Balance is $1000
    And Account name is "Somesh Rao"
    And Address is "123 Main St"
    When Create a new account
    Then Verify Account Creation

@tag2
Scenario: Create Account With Minimum Balance Less Than Minimum Required
	Given Account Initial Balance is $99
    And Account name is "Nitesh Kumar"
    And Address is "123 Main St"
    When Create a new account
    Then Verify Account Creation

@tag3
Scenario: Create Multiple Accounts
	Given Account Initial Balance is $<InitialBalance>
    And Account name is "<AccountName>"
    And Address is "<Address>"
    When Create a new account for Multiple Users
    Then Verify Account Number is Unique
    
    Examples:
    | InitialBalance | AccountName | Address          |
    | 1000.00        | Somesh Rao  | 123 Main St,Miami|
    | 1500.00        | Khushboo    | 456 Elm St,Noida |
    | 2000.00        | Nitesh Kumar| 789 ,Nagpur      |

@tag4
Scenario: Create Account With Balance Greater Than Transaction Limit
	Given Account Initial Balance is $10,001
    And Account name is "Nitesh Kumar"
    And Address is "123 Main St"
    When Create a new account
    Then Verify Account Creation

@tag5
Scenario: Create Account With Negative Balance
	Given Account Initial Balance is $-200
    And Account name is "Satish Kumar"
    And Address is "321 Main St"
    When Create a new account
    Then Verify Account Creation


@tag6
Scenario: Deposit Amount
    And Balance Entered is $100
    When "Deposit" Amount to Account
    Then Verify the Balance in the Account After "Deposit"
@tag7
Scenario: Deposit A Negative Amount
    And Balance Entered is $-100
    When "Deposit" Amount to Account
    Then Verify the Balance in the Account After "Deposit"
@tag8
Scenario: Deposit Amount Greater Than Transaction Limit
    And Balance Entered is $10,001
    When "Deposit" Amount to Account
    Then Verify the Balance in the Account After "Deposit"

@tag9
Scenario: Deposit Amount To NonExisting Account
	Given Account Number is 23001
    And Balance Entered is $200
    When "Deposit" Amount to Account
    Then Verify the Balance in the Account After "Deposit"


@tag10
Scenario: Withdraw Amount
    And Balance Entered is $100
    When "Withdraw" Amount to Account
    Then Verify the Balance in the Account After "Withdraw"

@tag11
Scenario: Withdraw NegativeAmount
    And Balance Entered is $-100
    When "Withdraw" Amount to Account
    Then Verify the Balance in the Account After "Withdraw"

@tag12
Scenario: Withdraw Amount Greater Than the Account Balance
    And Balance Entered is Greater Than the Account Balance
    When "Withdraw" Amount to Account
    Then Verify the Balance in the Account After "Withdraw"
@tag13
Scenario: Withdraw Amount From NonExisting Account
	Given Account Number is 23001
    And Balance Entered is $100
    When "Withdraw" Amount to Account
    Then Verify the Balance in the Account After "Withdraw"

@tag14
Scenario: Withdraw Amount With A Balance Less Than Minimum Required
    And Balance Entered is The Amount Less Than Minimum Required.
    When "Withdraw" Amount to Account
    Then Verify the Balance in the Account After "Withdraw"

@tag15
Scenario: Delete Account
	When Delete Account
    Then Verify the Account id Deleted
   
@tag16
Scenario: Delete Nonexisting Acoount 
	Given Account Number is 23001
    When Delete Account
    Then Verify the Account id Deleted

@tag17
Scenario: View the Required Account Details 
    When  Get Account Details
    Then  Verify account details 
    
@tag18
Scenario: View Account Details that doesn't exist 
    Given Account Number is 23001
    When  Get Account Details
    Then  Verify account details 





