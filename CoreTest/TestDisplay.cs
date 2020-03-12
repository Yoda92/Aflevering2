using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;

namespace CoreTest
{
    class TestDisplay
    {
        private Core.Display _d;
        [SetUp]
        public void Setup()
        {
            _d = new Core.Display();
        }

        [TestCase("This is a message")]
        public void TestChargingMessage(string someString)
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                _d.DisplayChargingMessage(someString);
                string expected = string.Format("Charging Message: {0}{1}", someString, Environment.NewLine);
                Assert.AreEqual(expected, sw.ToString());
            }
        }

        [TestCase("This is a instruction")]
        public void TestUserInstruction(string someString)
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                _d.DisplayUserInstructions(someString);
                string expected = string.Format("User Instruction: {0}{1}", someString, Environment.NewLine);
                Assert.AreEqual(expected, sw.ToString());
            }
        }
    }
}
