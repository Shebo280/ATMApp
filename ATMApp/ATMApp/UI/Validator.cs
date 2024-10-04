using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public static class Validator
    {
        public static T Convert<T>(string prompt)
        {
            bool valid = false;
            string userInput = string.Empty;

            while (!valid)
            {
               
                userInput = Utility.GetUserPrompt(prompt);
                try
                {
                    // Attempt to convert the string to the desired type
                    var convertedValue = TypeDescriptor.GetConverter(typeof(T));
                    if (convertedValue != null)
                    {

                        var value= (T)convertedValue.ConvertFromString(userInput);
   
                        return value;
                    }
                    else
                    {
                        return default;
                    }
                }
                catch
                {
                   Utility.PrintMessage("Invalid input. Try again",false);
                }
                Console.Clear();


            }
            return default;

        }
    }
}
