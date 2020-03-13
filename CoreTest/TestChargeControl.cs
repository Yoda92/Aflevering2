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

        [TestCase(0.01)]
        [TestCase(3.0)]
        [TestCase(5.0)]
        public void HandleCurrentChangedBetweenZeroAndFive(double current)
        {
            _charger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(new CurrentEventArgs() {Current = current});
            _disp.Received().DisplayChargingMessage(Arg.Is<string>("Not charging..."));
        }

        [TestCase(5.01)]
        [TestCase(25.0)]
        [TestCase(200.3)]
        [TestCase(500.0)]
        public void HandleCurrentChangedBetweenFiveAndFiveHundred(double current)
        {
            _charger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(new CurrentEventArgs() {Current = current});
            _disp.Received().DisplayChargingMessage(Arg.Is<String>("Charging...")); 
        }

        [TestCase(500.01)]
        [TestCase(700)]
        [TestCase(120000)]
        public void HandleCurrentChangedOverFiveHundred(double current)
        {
            _charger.CurrentValueEvent += Raise.EventWith<CurrentEventArgs>(new CurrentEventArgs() {Current = current});
            _disp.Received().DisplayChargingMessage(Arg.Is<String>("Error - Stopped Charging!"));
            _charger.Received().StopCharge();
        }
    }
}