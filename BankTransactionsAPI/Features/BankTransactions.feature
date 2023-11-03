Feature: BankTransactions

  Manages bank account transactions and balances
Background:
    Given Account Number is 1001

	
@Smoke @Create
Scenario: Create Account with Valid Data
	Given Account Initial Balance is $1000
    And   Account name is "Somesh Rao"
    And   Address is "123 Main St"
    When  Create a new account
    Then  Verify the response code is 200
    And   Verify Account Creation

@Smoke @Create
Scenario: Create Account With Minimum Balance Less Than Minimum Required
	Given Account Initial Balance is $99
    And   Account name is "Nitesh Kumar"
    And   Address is "123 Main St"
    When  Create a new account
    Then  Verify the response code is 400
    And   Verify Account Creation

@Smoke @Create
Scenario Outline: Create Multiple Accounts
	Given Account Initial Balance is $<InitialBalance>
    And   Account name is "<AccountName>"
    And   Address is "<Address>"
    When  Create a new account for Multiple Users
    Then  Verify the response code is 200
    And   Verify Account Number is Unique
    
    Examples:
    | InitialBalance | AccountName | Address          |
    | 1000.00        | Somesh Rao  | 123 Main St,Miami|
    | 1500.00        | Khushboo    | 456 Elm St,Noida |
    | 2000.00        | Nitesh Kumar| 789 ,Nagpur      |

@Smoke @Create
Scenario: Create Account With Balance Greater Than Transaction Limit
	Given Account Initial Balance is $10,001
    And   Account name is "Nitesh Kumar"
    And   Address is "123 Main St"
    When  Create a new account
    Then  Verify the response code is 400
    And   Verify Account Creation

@Smoke @Create
Scenario: Create Account With Negative Balance
	Given Account Initial Balance is $-200
    And   Account name is "Satish Kumar"
    And   Address is "321 Main St"
    When  Create a new account
    Then  Verify the response code is 400
    And   Verify Account Creation

@Smoke @Deposit
Scenario: Deposit Amount - By sending Valid Amounts
    Given Balance Entered is $<InitialBalance>
    When  "Deposit" Amount
    Then  Verify the response code is 200
    And   Verify the Balance in the Account After "Deposit"
       Examples:
    | InitialBalance |
    | 100            |  # Positive scenario - Sending Valid Minimum Amount
    | 10000          |  # Positive scenario - Sending Valid Maximum Transaction Limit Amount

@Smoke @Deposit
Scenario Outline: Deposit Amount - By sending Invalid Amounts
    Given Balance Entered is $<InitialBalance>
    When  "Deposit" Amount
    Then  Verify the response code is 400
    And   Verify the Balance in the Account After "Deposit"

    Examples:
    | InitialBalance |
    | -100           |  # Negative scenario - Sending Negative Amount
    | 50             |  # Negative scenario - Sending Balance Less than Minimum Required Amount
    | 10001          |  # Deposit exceeds limit scenario

@Smoke @Deposit
Scenario: Deposit Amount To NonExisting Account
	Given Account Number is 23001
    And   Balance Entered is $200
    When  "Deposit" Amount
    Then  Verify the response code is 400
    And   Verify the Balance in the Account After "Deposit"

@Smoke @Withdraw
Scenario: Withdraw Amount with Valid Data
    Given Balance Entered is $100
    When  "Withdraw" Amount
    Then  Verify the response code is 200
    And   Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw NegativeAmount
    Given Balance Entered is $-100
    When  "Withdraw" Amount
    Then  Verify the response code is 400
    And   Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount Greater Than the Account Balance
    Given Balance Entered is Greater Than the Account Balance
    When  "Withdraw" Amount
    Then  Verify the response code is 400
    And   Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount more than 90% of the Account Balance
    Given Balance in the Account is More than the Percent allowed to withdraw.
    When  "Withdraw" Amount
    Then  Verify the response code is 400
    And   Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount From NonExisting Account
	Given Account Number is 23001
    And   Balance Entered is $100
    When  "Withdraw" Amount
    Then  Verify the response code is 400
    And   Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount With A Balance Less Than Minimum Required
    Given Balance Entered is The Amount Less Than Minimum Required.
    When  "Withdraw" Amount
    Then  Verify the response code is 400
    And   Verify the Balance in the Account After "Withdraw"

@Smoke @Withdraw
Scenario: Withdraw Amount If Balance is Less then $100
    Given Existing Balance is Less Than $100 
    When  "Withdraw" Amount
    Then  Verify the response code is 400
    And   Verify the Balance in the Account After "Withdraw"

@Smoke @Delete
Scenario: Delete Account with Valid Account Number
	When  Delete Account
    Then  Verify the response code is 200
    And   Verify the Account id Deleted
   
@Smoke @Delete
Scenario: Delete Nonexisting Account 
	Given Account Number is 23001
    When  Delete Account
    Then  Verify the response code is 400
    And   Verify the Account id Deleted

@Smoke @Delete
Scenario Outline: Delete Multiple Accounts
	Given Account Number is $<AccountNumber>
    When  Delete Account
    Then  Verify the response code is 200
    And   Verify the Account 

    Examples:
    | AccountNumber |
    | 1001          |
    | 1002          |
    | 1003          |

@Smoke @View
Scenario: View the Required Account Details 
    When  Get Account Details
    Then  Verify the response code is 200
    And   Verify account details 
    
@Smoke @View
Scenario Outline: View Multiple Accounts
    Given Account Number is $<AccountNumber>
    When  Get Account Details
    Then  Verify the response code is 200
    And   Verify account details
   
   Examples:
    | AccountNumber |
    | 1001          |
    | 1002          |
    | 1003          |

@Smoke @View
Scenario: View Account Details that doesn't exist 
    Given Account Number is 23001
    When  Get Account Details
    Then  Verify the response code is 400
    And   Verify account details 





