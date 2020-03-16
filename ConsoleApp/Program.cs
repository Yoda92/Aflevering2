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

            _doorSimulator.SimulateDoorOpen();
            _usbChargerSimulator.SimulateConnected(true);
            _doorSimulator.SimulateDoorClose();
            _doorSimulator.SimulateDoorOpen();
            _RFIDReaderSimulator.SimulateReadRFID(50);
            _doorSimulator.SimulateDoorClose();
            _RFIDReaderSimulator.SimulateReadRFID(50);
            _RFIDReaderSimulator.SimulateReadRFID(25);
            _RFIDReaderSimulator.SimulateReadRFID(125);
            _RFIDReaderSimulator.SimulateReadRFID(50);
            _usbChargerSimulator.SimulateConnected(false);
            _doorSimulator.SimulateDoorClose();
            while (true) { }
        }

    }
}
