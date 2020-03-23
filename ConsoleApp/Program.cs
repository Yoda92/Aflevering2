using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Core;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Display _display = new Display();
            UsbChargerSimulator _usbChargerSimulator = new UsbChargerSimulator();
            ChargeControl _chargeControl = new ChargeControl(_display, _usbChargerSimulator);
            RFIDReaderSimulator _RFIDReaderSimulator = new RFIDReaderSimulator();
            DoorSimulator _doorSimulator = new DoorSimulator();
            LogFile _logfile = new LogFile();
            StationControl _stationControl = new StationControl(_display, _doorSimulator, _logfile, _RFIDReaderSimulator, _usbChargerSimulator);

            bool finish = false;
            System.Console.WriteLine("Indtast E = Exit\nO = Open\nC = Close\nR = RFID Read\nT = Tilslut/Fjern telefon");
            do
            {
                string input;
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        _doorSimulator.SimulateDoorOpen();
                        break;

                    case 'C':
                        _doorSimulator.SimulateDoorClose();
                        break;

                    case 'T':
                        if (_stationControl._state == StationControl.LadeskabState.DoorOpen)
                        {
                            _usbChargerSimulator.SimulateConnected(!_usbChargerSimulator.Connected);
                            Console.WriteLine("Telephone connected: " + _usbChargerSimulator.Connected);
                        }
                        else Console.WriteLine("Open door first!");
                        break;

                    case 'R':
                        Console.WriteLine("Indtast RFID id: ");
                        string idString = Console.ReadLine();

                        int id = Convert.ToInt32(idString);
                        _RFIDReaderSimulator.SimulateReadRFID(id);
                        break;

                    default:
                        break;
                }

            } while (!finish);
        }

    }
}
