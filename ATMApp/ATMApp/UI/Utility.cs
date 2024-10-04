using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public static class Utility
    {
        private static long tranId=0;
        public static long GetTransactionId() { return ++tranId; }
        public static T ValidateCardNumber<T>(T  cardNumber)
        {
            while (cardNumber.ToString().Length != 6)
            {
                Utility.PrintMessage("Please Enter 6 digits.", false);
                cardNumber = Validator.Convert<T>("Enter your card number:");
            }
            return cardNumber;
        }
        public static string HashTheCardPIN(string prompt)
        {
            bool isPrompt = true;
            StringBuilder hashedPin = new();
            StringBuilder astrices = new();
            while (true)
            {
                if (isPrompt) Console.WriteLine(prompt);
                isPrompt = false;
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter) 
                {
                    // If the string doesn't equal 6
                    if (hashedPin.Length != 6)
                    {
                        PrintMessage("\n\nPlease, Enter 6 digit PIN", false);
                        isPrompt = true;
                        hashedPin.Clear();
                        astrices.Clear();
                        
                    }
                    // If the string equals 6 (it may be digits or characters) so we need to check
                    else {
                        // If it can parse it then the entered are digits otherwise the entered are characters print an error message
                        if (int.TryParse(hashedPin.ToString(), out _)) break;
                        
                        else
                        {
                            PrintMessage("\n\nInvalid Input, Please try again\n", false);
                            isPrompt = true;
                        }

                    };

                }
                else if (keyInfo.Key == ConsoleKey.Backspace && hashedPin.Length > 0)
                {
                    hashedPin.Remove(hashedPin.Length - 1,1);
                    astrices.Remove(astrices.Length - 1,1);
                    ClearCurrentLine();
                    Console.Write(astrices);
                } 
                else if(keyInfo.Key != ConsoleKey.Backspace)
                {
                    hashedPin.Append(keyInfo.KeyChar);
                    astrices.Append('*');
                    Console.Write('*');
                }
            }
            return hashedPin.ToString();
        }

        private static void ClearCurrentLine()
        {
            // Move the cursor to the beginning of the current line
            Console.SetCursorPosition(0, Console.CursorTop);
            // Clear the line by writing spaces
            Console.Write(new string(' ', Console.WindowWidth));
            // Move the cursor back to the start of the current line
            Console.SetCursorPosition(0, Console.CursorTop);
        }
        public static void PrintMessage(string message, bool success=true)
        {
            // Change the color of the text based on the success
            Console.ForegroundColor = success? ConsoleColor.Green : ConsoleColor.Red;
            
            Console.WriteLine(message);
            Console.ForegroundColor=ConsoleColor.White;
            PressEnterToContinue();

        }
        
        public static string GetUserPrompt(string prompt)
        {

            Console.WriteLine($"Enter {prompt}");

            return Console.ReadLine();
        }
        public static void PrintDotAnimation(int timer=10)
        {
            for (int i = 0; i < timer; i++)
            {
                Console.Write('.');
                Thread.Sleep(300);
            }
        }
        public static void PressEnterToContinue()
        {
            Console.WriteLine("\n\nPress Enter to continue....\n");
            Console.ReadKey();
        }
        public static string FormatAmount(decimal amount)
        {
            return amount.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
