using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Core.LogFile L = new Core.LogFile();

            L.LogDoorLocked(16);
            L.LogDoorUnlocked(20);
        }
    }
}
