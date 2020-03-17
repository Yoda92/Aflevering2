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
        
        [TestCase(false, "Dør er lukket. Indlæs RFID.", StationControl.LadeskabState.Available)]
        [TestCase(true, "Dør er åben. Tilslut telefon.", StationControl.LadeskabState.DoorOpen)]
        public void HandleDoorStateChangedDoorOpenTest(bool open, string outputString, StationControl.LadeskabState outputState)
        {
            _door.DoorStateChangedEvent += Raise.EventWith<DoorStateEventArgs>(new DoorStateEventArgs() {Open = open});
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
            Assert.That(_sc._state, Is.EqualTo(outputState));
        }

        [TestCase(true, "Dør er åben. Tilslut telefon.", StationControl.LadeskabState.DoorOpen)]
        [TestCase(false, "Dør er lukket. Indlæs RFID.", StationControl.LadeskabState.Available)]
        public void HandleDoorStateChangedAvailableTest(bool open, string outputString, StationControl.LadeskabState outputState)
        {
            _door.DoorStateChangedEvent += Raise.EventWith<DoorStateEventArgs>(new DoorStateEventArgs() { Open = open });
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
            Assert.That(_sc._state, Is.EqualTo(outputState));
        }

        [Test]
        public void HandleDoorStateChangedLockedTest()
        {
            _usbCharger.Connected = true;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() { });
            _usbCharger.ClearReceivedCalls();
            _RFIDReader.ClearReceivedCalls();
            _door.DoorStateChangedEvent += Raise.EventWith<DoorStateEventArgs>(new DoorStateEventArgs() { Open = true });
            _door.DidNotReceive();
            _usbCharger.DidNotReceive();
            _display.DidNotReceive();
            _logfile.DidNotReceive();
            _RFIDReader.DidNotReceive();
        }

        [TestCase(10, "Ladeskab optaget.")]
        public void HandleRFIDReadAvaibleConnectedTest(int id, string outputString)
        {
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
            _usbCharger.Connected = false;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() {});
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
        }

        [TestCase("Luk døren.")]
        public void HandleRFIDReadDoorOpenTest(string outputString)
        {
            _door.DoorStateChangedEvent += Raise.EventWith<DoorStateEventArgs>(new DoorStateEventArgs() { Open = true });
            _usbCharger.Connected = false;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() {});
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
        }

        [TestCase(10, "Fjern telefon.")]
        public void HandleRFIDReadLockedEqualTest(int id, string outputString)
        {
            _usbCharger.Connected = true;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() { ID = id });
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() { ID = id });
            _door.Received().UnlockDoor();
            _logfile.Received().LogDoorUnlocked(id);
            _usbCharger.Received().StopCharge();
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
            Assert.That(_sc._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }

        [TestCase(10, "Forkert RFID tag.")]
        public void HandleRFIDReadLockedNotEqualTest(int id, string outputString)
        {
            _usbCharger.Connected = true;
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() { ID = (id + 1) });
            _RFIDReader.RFIDReadEvent += Raise.EventWith<RFIDReadEventArgs>(new RFIDReadEventArgs() { ID = id });
            _display.Received().DisplayUserInstructions(Arg.Is<string>(outputString));
        }
    }
}