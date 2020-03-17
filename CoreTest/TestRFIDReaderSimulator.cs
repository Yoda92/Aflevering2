using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core;

namespace RFIDReaderSimulator.test
{
    [TestFixture]
    class TestRFIDReaderSimulator
    {
        private Core.RFIDReaderSimulator _uut;
        private RFIDReadEventArgs _RFIDReadEventArgs;
        int Valid_ID = 2323;

        [SetUp]
        public void Setup()
        {
            _RFIDReadEventArgs = null;
            _uut = new Core.RFIDReaderSimulator();

            _uut.RFIDReadEvent +=
                (o, args) => { _RFIDReadEventArgs = args; };
        }



        [Test]

        public void RFID_Read_EventFired()
        {
            _uut.SimulateReadRFID(2323);
            Assert.That(_RFIDReadEventArgs.ID, Is.Not.Null);
        }


        [Test]

        public void RFIDRead_Succes() 
        {
            _uut.SimulateReadRFID(2323);

            Assert.That(_RFIDReadEventArgs.ID, Is.EqualTo(Valid_ID));
        }


        [Test]
        public void RFIDRead_failed()
        {
            _uut.SimulateReadRFID(8989);

            Assert.That(_RFIDReadEventArgs.ID, Is.EqualTo(Valid_ID));
        }



    }
}
