using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Core
{
    public class StationControl
    {
        public enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        public LadeskabState _state {get; set;}
        public int _oldId;

        private IDisplay _display;
        private IDoor _door;
        private ILogFile _logfile;
        private IRFIDReader _RFIDReader;
        private IUsbCharger _usbCharger;

        public StationControl(IDisplay display, IDoor door, ILogFile logfile, IRFIDReader RFIDReader, IUsbCharger usbCharger)
        {
            _display = display;
            _door = door;
            _logfile = logfile;
            _RFIDReader = RFIDReader;
            _usbCharger = usbCharger;

            _state = LadeskabState.Available;

            _door.DoorStateChangedEvent += HandleDoorStateChanged;
            _RFIDReader.RFIDReadEvent += HandleRFIDRead;
        }

        private void HandleDoorStateChanged(Object s, DoorStateEventArgs e)
        {
            switch (_state)
            {
                case LadeskabState.DoorOpen:
                    if (!e.Open)
                    {
                        _state = LadeskabState.Available;
                        _display.DisplayUserInstructions("Dør er lukket. Indlæs RFID.");
                    }
                    else
                    { 
                        _display.DisplayUserInstructions("Dør er åben");
                    }
                    break;

                case LadeskabState.Available:
                    if (e.Open)
                    {
                        _state = LadeskabState.DoorOpen;
                        _display.DisplayUserInstructions("Dør er åben. Tilslut telefon.");
                    }
                    else
                    {
                        _display.DisplayUserInstructions("Dør er lukket. Indlæs RFID.");
                    }
                    break;

                case LadeskabState.Locked:
                    break;

            }
        }

        private void HandleRFIDRead(Object s, RFIDReadEventArgs e)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    if (_usbCharger.Connected)
                    {
                        _door.LockDoor();
                        _logfile.LogDoorLocked(e.ID);
                        _oldId = e.ID;
                        _state = LadeskabState.Locked;
                        _display.DisplayUserInstructions("Ladeskab optaget.");
                        _usbCharger.StartCharge();
                    }
                    else
                    {
                        _display.DisplayUserInstructions("Tilslutningsfejl.");
                    }
                    break;

                case LadeskabState.DoorOpen:
                    _display.DisplayUserInstructions("Luk døren.");
                    break;

                case LadeskabState.Locked:
                    if (e.ID == _oldId)
                    {
                        _usbCharger.StopCharge();
                        _door.UnlockDoor();
                        _logfile.LogDoorUnlocked(e.ID);
                        _state = LadeskabState.Available;
                        _display.DisplayUserInstructions("Fjern telefon.");
                    }
                    else
                    {
                        _display.DisplayUserInstructions("Forkert RFID tag.");
                    }
                    break;
            }
        }
    }
}
