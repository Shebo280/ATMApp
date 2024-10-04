Here’s an updated version of the README that includes the available user accounts:

---

# 🌟 My ATM Console Application 🌟

Welcome to **My ATM App**! This console-based application simulates the basic functionalities of an ATM. It’s user-friendly, secure, and gives you access to your account right from the command line!

## 🏁 Features
- **Card Number & PIN Authentication**:  
  Secure login by entering your **card number** and **PIN**. Don't worry, we’ve got you covered with input validation:
  - **Max 6 digits** for the PIN.
  - No characters allowed – entering one will trigger an error.
  - Three incorrect attempts, and your account will be **locked**!

- **Successful Login**:  
  Enter the correct credentials, and you’ll be greeted!

- **Main Menu**:  
  Once logged in, you’ll see the options below:
  ```
  1. Account Balance
  2. Cash Deposit
  3. Withdrawal
  4. Transfer
  5. Transactions
  6. Logout
  ```

### Available User Accounts
You can use the following accounts to test the application:

```csharp
userAccountList = [
    new UserAccount{ Id=1, FullName= "Mohamed Shehab", CardNumber= 111111, CardPin= 112233, AccountNumber= 123456, AccountBalance= 48000.00m, Islocked= false },
    new UserAccount{ Id=2, FullName= "Ahmed Shaker", CardNumber= 222222, CardPin= 223344, AccountNumber= 123457, AccountBalance= 24500.00m, Islocked= false },
    new UserAccount{ Id=3, FullName= "Omar Mohsen", CardNumber= 333333, CardPin= 334455, AccountNumber= 123458, AccountBalance= 13000.00m, Islocked= false },
    new UserAccount{ Id=4, FullName= "Tamer Elkady", CardNumber= 444444, CardPin= 445566, AccountNumber= 123459, AccountBalance= 17250.00m, Islocked= false }
];
```

### 1️⃣ Account Balance
  Check your current account balance in real-time. Always know how much is in your account!

### 2️⃣ Cash Deposit
  Deposit cash into your account – but we accept **multiples of 50 and 100 only**! Your finances, managed with precision.

### 3️⃣ Withdrawal
  Need to withdraw money? Choose from pre-defined options or enter your own amount. We’ve got safeguards for:
  - **Negative amounts**: Sorry, no withdrawing negative amounts here!
  - **Overdraft protection**: You can’t withdraw more than what’s available in your account.

### 4️⃣ Transfer
  Transfer funds to another account by entering:
  - **Recipient’s account number**
  - **Recipient’s name** (which must match for the transfer to proceed).

### 5️⃣ Transaction History
  Keep track of your account activity with a detailed **transaction table** showing all deposits, withdrawals, and transfers.

### 6️⃣ Logout
  End your session and securely log out from the application. See you next time!

## 🚀 Getting Started
1. **Clone the repository**:
   ```bash
   git clone https://github.com/YourGitHubUsername/YourRepositoryName.git
   ```
2. **Navigate to the project directory**:
   ```bash
   cd /path/to/your/project
   ```
3. **Run the application** in your preferred environment:
   ```bash
   dotnet run
   ```

## 🎯 How It Works
1. **Login**: Enter your **card number** and **PIN**. After three failed attempts, the account locks.
2. **Menu**: Once authenticated, you’ll see the **ATM menu**.
3. **Choose an option**: Input a number to select a feature, and the system will guide you through the process.

## 🛠️ Technologies Used
- **C#**
- **.NET Core Console Application**
