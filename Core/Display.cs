using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Display : IDisplay
    {
        public void DisplayUserInstructions(string s)
        {
            string tempString = "User Instruction: {0}";
            string formattedString = string.Format(tempString, s);
            System.Console.WriteLine(formattedString);
        }
        public void DisplayChargingMessage(string s)
        {
            string tempString = "Charging Message: {0}";
            string formattedString = string.Format(tempString, s);
            System.Console.WriteLine(formattedString);
        }
    }
}
