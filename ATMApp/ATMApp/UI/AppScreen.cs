using ATMApp.Domain.Entities;

namespace ATMApp.UI
{
    public static class AppScreen
    {
        internal static string currency = "$";
        internal static void Welcome()
        {
            // Clearing the console
            Console.Clear();

            // Setting the title of the console
            Console.Title = "My ATM App";

            // Setting the color of the text
            Console.ForegroundColor = ConsoleColor.White;

            // Setting the welcome message
            Console.WriteLine("\n-------------Welcome to my ATM App------------- \n\n");
            // Asking the user to enter PIN number
            Console.WriteLine("Please insert your ATM Card");
            Console.WriteLine("Note: Actual ATM machine will accpet and validate a physical ATM card, " +
                "read the card number and validate it.");

            Utility.PressEnterToContinue();
        }
        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUser = new()
            {
                // Checking if it's 6 numbers or not
                CardNumber = Utility.ValidateCardNumber(Validator.Convert<long>("Enter your card number:")),

                CardPin = Convert.ToInt32(Utility.HashTheCardPIN("Enter your card pin:"))
            };
            return tempUser;
        }
        internal static void LoginProgress()
        {
            Console.Write("\n\nChecking your Card Number and PIN");
            Utility.PrintDotAnimation();
            Console.Clear();
        }
        public static void PrintLockMessage()
        {
            Utility.PrintMessage("\nYour account has been locked after 3 unsuccessful attempts." +
                " Please visit your nearest branch to unlock it", false);
            Environment.Exit(1);
        }
        public static void PrintWelcomeMessage(string name)
        {
            Utility.PrintMessage($"\nWelcome back, {name}");
        }
        public static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("----------My ATM App Menu----------\n");
            Console.WriteLine("1. Account Balance:");
            Console.WriteLine("2. Cash Deposit   :");
            Console.WriteLine("3. Withdrawl      :");
            Console.WriteLine("4. Transfer       :");
            Console.WriteLine("5. Transactions   :");
            Console.WriteLine("6. Logout         :");
        }
        internal static void LogoutProgress()
        {
            Console.Write("Thank you for using My ATM App");
            Utility.PrintDotAnimation();
            Console.Clear();
            Utility.PrintMessage("\nYou have sucessfully logged out. Please take your ATM Card", true);
        }

        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1.{0}50        5.{0}750",currency);
            Console.WriteLine(":2.{0}100       6.{0}1,000",currency);
            Console.WriteLine(":3.{0}200       7.{0}1,500",currency);
            Console.WriteLine(":4.{0}500       8.{0}2,000",currency);
            Console.WriteLine(":0.Other");
            Console.WriteLine("");

            int selectAmount= Validator.Convert<int>("Option:");
            switch (selectAmount) {
                case 1:
                    return 50;
                case 2:
                    return 100;
                case 3:
                    return 200;
                case 4:
                    return 500;
                case 5:
                    return 750;
                case 6:
                    return 1000;
                case 7:
                    return 1500;
                case 8:
                    return 2000;
                case 0:
                    return 0;
                default:
                    Utility.PrintMessage("Invalid Input. Try again");
                    return -1;
            }
        }
        internal static InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer
            {
                TransferAmount = Validator.Convert<int>("Transfer Amount:"),
                ReciepientBankAccountNumber = Validator.Convert<int>("Receipient Account Number:"),
                ReciepientBankAccountName = Utility.GetUserPrompt("Receipent Name:")
            };
            return internalTransfer;
        }

    }
}
