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
        private string path;

        public LogFile()
        { 
            path = @"Log.txt";
            var myFile = File.Create(path);
            myFile.Close();
        }

        public void LogDoorLocked(int ID)
        {
            File.AppendAllText(path, "Door Locked with ID: " + ID + "\n");
        }

        public void LogDoorUnlocked(int ID)
        {
            File.AppendAllText(path, "Door Unlocked with ID: " + ID + "\n");
        }
    }
}