using System;
using System.Runtime.CompilerServices;
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

            int testID = 100;
            _RFIDReaderSimulator.SimulateReadRFID(testID);

            while(true) { }
        }

    }
}
