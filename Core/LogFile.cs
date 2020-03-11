using System;
using System.IO;

namespace Core
{
    public class LogFile : ILogFile
    {
        public static bool WriteLog(string fileName,string msg)  
        {  
            try  
            {  
                FileStream O = new FileStream(string.Format("{0}\\{1}", Path.GetTempPath(), fileName), FileMode.Append, FileAccess.Write);  
                StreamWriter S = new StreamWriter((Stream)O);  
                S.WriteLine(msg);  
                S.Close();  
                O.Close();  
                return true;  
            }  
            catch(Exception ex)  
            {  
                return false;  
            }  
        }  


        StreamWriter s = File.AppendTedt("log.txt");

        TextWriter w;

        public void LogDoorLocked(int ID)
        {
            w.WriteLine('Door Locked ID: ' + ID);
            w.WriteLine('Time: ');
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("-------------------------------");
        }

        public void LogDoorUnlocked(int ID)
        {
            w.WriteLine('Door Unlocked with ID: ' + ID);
            w.WriteLine('Time: ');
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("-------------------------------");
        }
    }
}