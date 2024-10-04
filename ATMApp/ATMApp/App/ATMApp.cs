using ATMApp.Domain.Entities;
using ATMApp.Domain.Enums;
using ATMApp.Domain.Interfaces;
using ATMApp.UI;
using ConsoleTables;
using System.Linq;
namespace ATMApp.App
{

    public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    {

        private List<UserAccount> userAccountList;
        private UserAccount currentUser;
        private List<Transaction> listOfTransactions;
        

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumberAndPassword();
            AppScreen.PrintWelcomeMessage(currentUser.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuOptions(); 
            }
        }


        public void CheckUserCardNumberAndPassword()
        {
            bool isCorrectlogin = false;
            UserAccount inputAccount = AppScreen.UserLoginForm();
            while (!isCorrectlogin)
            {
                AppScreen.LoginProgress();
                foreach (UserAccount userAccount in userAccountList)
                {
                    if (userAccount.CardNumber.Equals(inputAccount.CardNumber)) {
                        userAccount.TotalLogin++;
                        if (userAccount.CardPin.Equals(inputAccount.CardPin)) {
                            // If the account is locked or there 3 failed attempets then print locked message

                            if (userAccount.Islocked || userAccount.TotalLogin>3)
                            {
                                AppScreen.PrintLockMessage();   
                            }
                            else
                            {
                                // We found a correct user
                                isCorrectlogin = true;
                                currentUser=userAccount;
                            }
                        }
                   
                    }
             
                }
                // If we didn't find any account that matches card number and PIN, show error message and ask to enter again
                if (!isCorrectlogin)
                {
                    Utility.PrintMessage("\nInvalid Card number or PIN", false);
                    inputAccount = AppScreen.UserLoginForm();
                }
            }
         

        }

       

        public void Initialize()
        {
            userAccountList =
            [
                new UserAccount{ Id=1, FullName= "Mohamed Shehab", CardNumber= 111111, CardPin= 112233, AccountNumber= 123456, AccountBalance= 48000.00m, Islocked= false },
                new UserAccount{ Id=2, FullName= "Ahmed Shaker", CardNumber= 222222, CardPin= 223344, AccountNumber= 123457, AccountBalance= 24500.00m, Islocked= false},
                new UserAccount{Id=3, FullName= "Omar Mohsen", CardNumber= 333333, CardPin= 334455, AccountNumber= 123458, AccountBalance= 13000.00m, Islocked= false},
                new UserAccount{Id=4, FullName= "Tamer Elkady", CardNumber= 444444, CardPin= 445566, AccountNumber= 123459, AccountBalance= 17250.00m, Islocked= false}
            ];
            listOfTransactions = [];

        }
        private void ProcessMenuOptions()
        {
            doSwitchagain: switch (Validator.Convert<int>("Enter an option:"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrwal:
                    MakeWithDrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = AppScreen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransactions:
                    ViewTransactions();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid option.", false);
                    goto doSwitchagain;
            }

        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your balance is {Utility.FormatAmount(currentUser.AccountBalance)}", true);
        }

        public void PlaceDeposit()
        {
            Console.WriteLine($"\n\nOnly multiples of {AppScreen.currency}50 or {AppScreen.currency}100 allowed.\n\n");
            enterAmount:  int amount = Validator.Convert<int>("Enter the amount:");

            // Counting and Previewing
            Console.Write("Counting and Previewing");
            Utility.PrintDotAnimation();
            // If the amount is not positive
            if(amount <0)
            {
                Utility.PrintMessage("Invalid amount, The amount must be greater than 0.");
                goto enterAmount;
            }
            // If the amount isn't divisble by 50 or 100
            else if (amount % 50 != 0)
            {
                Utility.PrintMessage($"\nOnly multiples of {AppScreen.currency}50 or {AppScreen.currency}100 allowed.", false);
                goto enterAmount;
            }
            // If the user didn't confirm the deopsit
            else if (!PreviewBankDeposit(amount)) 
            {
                Utility.PrintMessage("The process has been cancelled", false);
                goto enterAmount;

            }
            // The deposit has been done successfully
            else
            {
                currentUser.AccountBalance += amount;
                Console.WriteLine("Loading");
                Utility.PrintDotAnimation();
                Utility.PrintMessage($"\nYou have deposited {Utility.FormatAmount(amount)} successfully", true);
                InsertTransaction(currentUser.Id, TransactionType.Deposit, amount, "");
            }


        }

        public void MakeWithDrawal()
        {
            int transactionAmount = 0;
            int selectedAmount = AppScreen.SelectAmount();
            if (selectedAmount == -1)
            {
                _ = AppScreen.SelectAmount();
            }
            else if (selectedAmount != 0)
            {
                transactionAmount = selectedAmount;
            }
            else
            {
                transactionAmount = Validator.Convert<int>("Enter the amount");
            }
            if (transactionAmount <= 0)
                Utility.PrintMessage("Amounts needs to be greater than zero. Please try again.",false);
            else if (transactionAmount % 50 !=0)
                Utility.PrintMessage($"Amounts need to be multiples of {AppScreen.currency}50 or {AppScreen.currency}100.", false);
            else if (transactionAmount >= currentUser.AccountBalance)
                Utility.PrintMessage("Withdrawl failed. Your balance is too low to withdraw.",false);
            else
            {
                Utility.PrintMessage($"You have sucessfully withdrawn {Utility.FormatAmount(transactionAmount)}. Please take your card and recipt.");
                currentUser.AccountBalance -= transactionAmount;
                InsertTransaction(currentUser.Id, TransactionType.Withdrawl, -transactionAmount, "");   
            }

        }
        private static bool PreviewBankDeposit(int amount)
        {
            int numberOfHundreds = amount/100;
            int numberOfFifties= (amount%100)/50;
            Console.WriteLine("\n\n\nSummary\n------");
            
            Console.WriteLine($"{AppScreen.currency}100 * ${numberOfHundreds} = ${numberOfHundreds*100} ");
            Console.WriteLine($"{AppScreen.currency}50 * ${numberOfFifties} = ${numberOfFifties * 50} ");
            Console.WriteLine($"Total Amount: {Utility.FormatAmount(amount)}");

            int confirm=Validator.Convert<int>("\nPress 1 to confirm");
            return confirm == 1;
        }

        public void InsertTransaction(long userBankID, TransactionType transactionType, decimal transactionAmount, string description)
        {
            // Creating transaction object
            var transaction = new Transaction
            {
                TransactionID = Utility.GetTransactionId(),
                UserBankAccountID = userBankID,
                TransactionDate=DateTime.Now,
                TransactionType=transactionType,
                TransactionAmount=transactionAmount,
                Description = description
            };
            // Adding transaction object to the list
            listOfTransactions.Add(transaction);

        }

        public void ViewTransactions()
        {
            var filterdTransactionList = listOfTransactions.Where(t => t.UserBankAccountID == currentUser.Id).ToList();
            // Check if there is any transaction
            if(filterdTransactionList.Count==0)
            {
                Utility.PrintMessage("You don't have any transcations yet.", false);

            }
            // Installing Console Table from depenedencies
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type",
                    "Amount"+AppScreen.currency, "Descriptions");
                foreach(var tran in filterdTransactionList)
                {
                    table.AddRow(tran.TransactionID, tran.TransactionDate, tran.TransactionType,
                        tran.TransactionAmount, tran.Description);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {filterdTransactionList.Count} transaction(s)", true);




            }
        }

   
        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            // Validating the transfter amount
            if (internalTransfer.TransferAmount<=0)
                Utility.PrintMessage("Amounts needs to be greater than zero. Please try again.", false);
            if (internalTransfer.TransferAmount >= currentUser.AccountBalance)
                Utility.PrintMessage($"Transfer Failed. You don't have enough balance" +
                    $" to transfer {internalTransfer.TransferAmount}");

            // Checking if receiver's account number is valid
            var selectedBankAccountReceiver = (from userAcc in userAccountList
                                               where userAcc.AccountNumber == internalTransfer.ReciepientBankAccountNumber
                                               select userAcc).FirstOrDefault();

            // Checking reciever's bank number
            if (selectedBankAccountReceiver == null)
            {
                Utility.PrintMessage("Transfer Failed. Recipient's bank account number is invalid.", false);
                return;
            }
            // Checking receiver's name

            if (selectedBankAccountReceiver.FullName!=internalTransfer.ReciepientBankAccountName)
            {
                Utility.PrintMessage("Transfer Failed. Recipient's bank account name does not match.", false);
                return;
            }

            // Add transaction record to the sender
            InsertTransaction(currentUser.Id, TransactionType.Transfer, internalTransfer.TransferAmount
                , $"Transfered to {selectedBankAccountReceiver.AccountNumber} ({selectedBankAccountReceiver.FullName})");
            // Subtract amount sent from sender
            currentUser.AccountBalance -= internalTransfer.TransferAmount;

            // Add trasnaction record to the recipient
            InsertTransaction(selectedBankAccountReceiver.Id, TransactionType.Transfer, internalTransfer.TransferAmount
                , $"Transfered from {currentUser.AccountNumber} ({currentUser.FullName})");
            // Add amount sent to the recipient
            selectedBankAccountReceiver.AccountBalance += internalTransfer.TransferAmount;
            // Print success message
            Utility.PrintMessage("You have successfully transfered " +
                $"{Utility.FormatAmount(internalTransfer.TransferAmount)} to " +
                $"{selectedBankAccountReceiver.FullName}", true);
        }
    }
}




