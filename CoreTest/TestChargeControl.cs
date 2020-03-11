using System;
using Core;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using UsbSimulator;

namespace CoreTest
{
    public class TestChargeControl
    {
        private IDisplay _disp;
        private IUsbCharger _charger;
        private ChargeControl _uut; 
        
        [SetUp]
        public void Setup()
        {
            _disp = Substitute.For<IDisplay>();
            _charger = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(_disp, _charger);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsConnectedReturnsValueOfUsbChargerConnected(bool connected)
        {
            _charger.Connected.Returns(connected);
            Assert.That(_uut.IsConnected(), Is.EqualTo(connected));
        }

        [Test]
        public void StartChargeCallsUsbChargerStartCharge()
        {
            _uut.StartCharge();
            _charger.Received().StartCharge();
        }

        [Test]
        public void StopChargeCallsUsbChargerStopCharge()
        {
            _uut.StopCharge();
            _charger.Received().StopCharge();
        }


        [Test]
        public void HandleCurrentChangedUnderZeroThrowsError()
        {
            Assert.That(() => 
                _charger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(new CurrentEventArgs() {Current = -1.0}),
                Throws.TypeOf<Exception>());
        }

        [Test]
        public void HandleCurrentChangedToZero()
        {
            
        }
    }
}