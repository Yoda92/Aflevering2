using Core;
using NUnit.Framework;

namespace CoreTest
{
    public class TestDoorSimulator
    {
        private DoorSimulator _door;
        private DoorStateEventArgs _receivedEventArgs; 
        [SetUp]
        public void Setup()
        {
            _receivedEventArgs = null;
           _door = new DoorSimulator();
           _door.DoorStateChangedEvent += (o, args) => { _receivedEventArgs = args; };
        }

        [Test]
        public void ctor_LockedIsFalse()
        {
            Assert.That(_door.IsLocked, Is.False);
        }

        [Test]
        public void LockingSetsStateToLocked()
        {
            _door.LockDoor();
            Assert.That(_door.IsLocked, Is.True);
        }

        [Test]
        public void UnlockingSetsStateToUnlocked()
        {
           _door.UnlockDoor();
           Assert.That(_door.IsLocked, Is.False);
        }

        [Test]
        public void OpeningDoorInvokesDoorStateEvent()
        {
            _door.SimulateDoorOpen();
            Assert.That(_receivedEventArgs.Open, Is.True);
        }

        [Test]
        public void ClosingDoorInvokesDoorStateEvent()
        {
            _door.SimulateDoorClose();
            Assert.That(_receivedEventArgs.Open, Is.False);
        }
    }
}