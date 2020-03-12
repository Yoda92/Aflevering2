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

        [SetUp]
        public void Setup()
        {
            _RFIDReadEventArgs = null;
            _uut = new Core.RFIDReaderSimulator();

            _uut.RFIDReadEvent +=
                (o, args) => { _RFIDReadEventArgs = args; };
        }


    }
}
