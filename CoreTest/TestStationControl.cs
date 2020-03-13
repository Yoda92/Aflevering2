using System;
using Core;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace CoreTest
{
    public class TestStationControl
    {
        private IDisplay _display;
        private IDoor _door;
        private ILogFile _logfile;
        private IRFIDReader _RFIDReader;
        private IUsbCharger _usbCharger;
        private StationControl _sc; 
        
        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
            _usbCharger = Substitute.For<IUsbCharger>();
            _door = Substitute.For<IDoor>();
            _logfile = Substitute.For<ILogFile>();
            _RFIDReader = Substitute.For<IRFIDReader>();
            _sc = new StationControl(_display, _door, _logfile, _RFIDReader, _usbCharger);
        }
        
        [TestCase(false, StationControl.LadeskabState.DoorOpen, "Dør er lukket. Indlæs RFID.", StationControl.LadeskabState.Available)]
        [TestCase(true, StationControl.LadeskabState.DoorOpen, "Dør er åben", StationControl.LadeskabState.DoorOpen)]
        [TestCase(false, StationControl.LadeskabState.Available, "Dør er åben. Tilslut telefon.", StationControl.LadeskabState.DoorOpen)]
        [TestCase(true, StationControl.LadeskabState.Available, "Dør er lukket. Indlæs RFID", StationControl.LadeskabState.Available)]
        public void HandleDoorStateChangedTest(bool open, StationControl.LadeskabState inputState, string outputString, StationControl.LadeskabState outputState)
        {
            _sc._state = inputState;
            _door.DoorStateChangedEvent += Raise.EventWith<DoorStateEventArgs>(new DoorStateEventArgs() {Open = open});
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
            Assert.That(_sc._state, Is.EqualTo(outputState));
        }
    }
}