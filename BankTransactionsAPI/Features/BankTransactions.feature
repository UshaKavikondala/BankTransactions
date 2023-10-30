Feature: BankTransactions

  Manages bank account transactions and balances
Background:
    Given Account Number is 1001

	
@Smoke @Crate
Scenario: Create Account
	Given Account Initial Balance is $1000
    And Account name is "Somesh Rao"
    And Address is "123 Main St"
    When Create a new account
    Then Verify Account Creation

@Smoke @Create
Scenario: Create Account With Minimum Balance Less Than Minimum Required
	Given Account Initial Balance is $99
    And Account name is "Nitesh Kumar"
    And Address is "123 Main St"
    When Create a new account
    Then Verify Account Creation

@Smoke @Create
Scenario Outline: Create Multiple Accounts
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

@Smoke @Create
Scenario: Create Account With Balance Greater Than Transaction Limit
	Given Account Initial Balance is $10,001
    And Account name is "Nitesh Kumar"
    And Address is "123 Main St"
    When Create a new account
    Then Verify Account Creation

@Smoke @Create
Scenario: Create Account With Negative Balance
	Given Account Initial Balance is $-200
    And Account name is "Satish Kumar"
    And Address is "321 Main St"
    When Create a new account
    Then Verify Account Creation

@Smoke @Deposit
Scenario: Deposit Amount
    And Balance Entered is $100
    When "Deposit" Amount
    Then Verify the Balance in the Account After "Deposit"

@Smoke @Deposit
Scenario: Deposit A Negative Amount
    And Balance Entered is $-100
    When "Deposit" Amount
    Then Verify the Balance in the Account After "Deposit"

@Smoke @Deposit
Scenario: Deposit Amount Greater Than Transaction Limit
    And Balance Entered is $10,001
    When "Deposit" Amount
    Then Verify the Balance in the Account After "Deposit"

@Smoke @Deposit
Scenario: Deposit Amount To NonExisting Account
	Given Account Number is 23001
    And Balance Entered is $200
    When "Deposit" Amount
    Then Verify the Balance in the Account After "Deposit"

@Smoke @Withdraw
Scenario: Withdraw Amount
    And Balance Entered is $100
    When "Withdraw" Amount
    Then Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw NegativeAmount
    And Balance Entered is $-100
    When "Withdraw" Amount
    Then Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount Greater Than the Account Balance
    And Balance Entered is Greater Than the Account Balance
    When "Withdraw" Amount
    Then Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount more than 90% of the Account Balance
    And Balance in the Account is More than the Percent allowed to withdraw.
    When "Withdraw" Amount
    Then Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount From NonExisting Account
	Given Account Number is 23001
    And Balance Entered is $100
    When "Withdraw" Amount
    Then Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount With A Balance Less Than Minimum Required
    And Balance Entered is The Amount Less Than Minimum Required.
    When "Withdraw" Amount
    Then Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount If Balance is Less then $100
    And Existing Balance is Less Than $100 
    When "Withdraw" Amount
    Then Verify the Balance in the Account After "Withdraw"

@Smoke @Delete
Scenario: Delete Account
	When Delete Account
    Then Verify the Account id Deleted
   
@Smoke @Delete
Scenario: Delete Nonexisting Acoount 
	Given Account Number is 23001
    When Delete Account
    Then Verify the Account id Deleted

@Smoke @Delete
Scenario Outline: Delete Multiple Accounts
	Given Account Number is $<AccountNumber>
    When Delete Account
    Then Verify the Account id Deleted

    Examples:
    | AccountNumber |
    | 1001          |
    | 1002          |
    | 1003          |


@Smoke @View
Scenario: View the Required Account Details 
    When  Get Account Details
    Then  Verify account details 
    

@Smoke @View
Scenario Outline: View Multiple Accounts
    Given Account Number is $<AccountNumber>
    When  Get Account Details
    Then  Verify account details
   
   Examples:
    | AccountNumber |
    | 1001          |
    | 1002          |
    | 1003          |

@Smoke @View
Scenario: View Account Details that doesn't exist 
    Given Account Number is 23001
    When  Get Account Details
    Then  Verify account details 





