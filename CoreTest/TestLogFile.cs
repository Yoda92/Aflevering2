//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using Microsoft.VisualStudio.TestPlatform.Utilities;
//using NUnit.Framework;

//namespace CoreTest
//{
//    class TestLogFile
//    {
//        private Core.LogFile _l;
//        private StringWriter stringWriter;
//        private TextWriter originalOutput;

//        [SetUp]
//        public void Setup()
//        {
//            _l = new Core.LogFile();
//        }

//        [TestCase(37)]
//        public void TestChargingMessage(int ID)
//        {
//            using (StringWriter sw = new StringWriter())
//            {
//                Console.SetOut(sw);
//                _l.LogDoorLocked(ID);
//                string expected = string.Format("Charging Message: {0}{1}", ID, Environment.NewLine);
//                Assert.AreEqual(expected, sw.ToString());
//            }
//        }

//        [TestCase(17)]
//        public void TestUserInstruction(int ID)
//        {
//            using (StringWriter sw = new StringWriter())
//            {
//                Console.SetOut(sw);
//                _l.LogDoorUnlocked(ID);
//                string expected = string.Format("User Instruction: {0}{1}", ID, Environment.NewLine);
//                Assert.AreEqual(expected, sw.ToString());
//            }
//        }
//    }
//}