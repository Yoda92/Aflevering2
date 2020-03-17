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
        
        [Test]
        public void ctor_IsAvailable()
        {
           Assert.That(_sc._state, Is.EqualTo(StationControl.LadeskabState.Available)); 
        }

        [Test]
        public void ctor_IsListeneningToDoorEvents()
        {
           _door.Received().DoorStateChangedEvent += Arg.Any<EventHandler<DoorStateEventArgs>>();
        }

        [Test]
        public void ctor_IsListeningToRFIDEvents()
        {
           _RFIDReader.Received().RFIDReadEvent += Arg.Any<EventHandler<RFIDReadEventArgs>>();
        }
        
        [TestCase(false, StationControl.LadeskabState.DoorOpen, "Dør er lukket. Indlæs RFID.", StationControl.LadeskabState.Available)]
        [TestCase(true, StationControl.LadeskabState.DoorOpen, "Dør er åben", StationControl.LadeskabState.DoorOpen)]
        [TestCase(true, StationControl.LadeskabState.Available, "Dør er åben. Tilslut telefon.", StationControl.LadeskabState.DoorOpen)]
        [TestCase(false, StationControl.LadeskabState.Available, "Dør er lukket. Indlæs RFID", StationControl.LadeskabState.Available)]
        public void HandleDoorStateChangedTest(bool open, StationControl.LadeskabState inputState, string outputString, StationControl.LadeskabState outputState)
        {
            _sc._state = inputState;
            _door.DoorStateChangedEvent += Raise.EventWith<DoorStateEventArgs>(new DoorStateEventArgs() {Open = open});
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
            Assert.That(_sc._state, Is.EqualTo(outputState));
        }

        [TestCase(10, "Ladeskab optaget.")]
        public void HandleRFIDReadAvaibleConnectedTest(int id, string outputString)
        {
            _sc._state = StationControl.LadeskabState.Available;
            _usbCharger.Connected = true;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() {ID = id});
            _door.Received().LockDoor();
            _logfile.Received().LogDoorLocked(id);
            _usbCharger.Received().StartCharge();
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
            Assert.That(_sc._state, Is.EqualTo(StationControl.LadeskabState.Locked));
            Assert.That(_sc._oldId, Is.EqualTo(id));
        }

        [TestCase("Tilslutningsfejl.")]
        public void HandleRFIDReadAvaibleNotConnectedTest(string outputString)
        {
            _sc._state = StationControl.LadeskabState.Available;
            _usbCharger.Connected = false;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() {});
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
        }

        [TestCase("Luk døren.")]
        public void HandleRFIDReadDoorOpenTest(string outputString)
        {
            _sc._state = StationControl.LadeskabState.DoorOpen;
            _usbCharger.Connected = false;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() {});
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
        }

        [TestCase(10, "Fjern telefon.")]
        public void HandleRFIDReadLockedEqualTest(int id, string outputString)
        {
            _sc._state = StationControl.LadeskabState.Locked;
            _sc._oldId = id;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() {ID = id });
            _door.Received().UnlockDoor();
            _logfile.Received().LogDoorUnlocked(id);
            _usbCharger.Received().StopCharge();
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
            Assert.That(_sc._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }

        [TestCase(10, "Forkert RFID tag.")]
        public void HandleRFIDReadLockedNotEqualTest(int id, string outputString)
        {
            _sc._state = StationControl.LadeskabState.Locked;
            _sc._oldId = id + 1;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() {});
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
        }
    }
}