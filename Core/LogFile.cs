using System;
using System.IO;
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Threading.Tasks; 

namespace Core
{
    public class LogFile : ILogFile
    {
        public void LogDoorLocked(int ID)
        {
            File.AppendAllText(@"C:\Users\Public\TestFolder\Log.txt", "Door Locked with ID: " + ID + "\n");
        }

        public void LogDoorUnlocked(int ID)
        {
            File.AppendAllText(@"C:\Users\Public\TestFolder\Log.txt", "Door Locked with ID: " + ID + "\n");
        }
    }
}