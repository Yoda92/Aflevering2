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
            if (!File.Exists(@"Log.txt"))
            {
                File.Create(@"Log.txt");
                File.AppendAllText(@"Log.txt", "Door Locked with ID: " + ID + "\n");
            }
            else
            {
                File.AppendAllText(@"Log.txt", "Door Locked with ID: " + ID + "\n");
            }
            
        }

        public void LogDoorUnlocked(int ID)
        {
            if (!File.Exists(@"Log.txt"))
            {
                File.Create(@"Log.txt");
                File.AppendAllText(@"Log.txt", "Door Unlocked with ID: " + ID + "\n");
            }

            else
            {
                File.AppendAllText(@"Log.txt", "Door Unlocked with ID: " + ID + "\n");
            }
            
        }
    }
}