using Core;
using NUnit.Framework;

namespace CoreTest
{
    public class TestDoorSimulator
    {
        private DoorSimulator door;
        [SetUp]
        public void Setup()
        {
           door = new DoorSimulator(); 
        }

        [Test]
        public void ctor_LockedIsFalse()
        {
            Assert.That(door.IsLocked, Is.False);
        }
    }
}