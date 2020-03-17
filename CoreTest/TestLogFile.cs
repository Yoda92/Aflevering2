using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;

namespace CoreTest
{
    class TestLogFile
    {
        private Core.LogFile _l;

        [SetUp]
        public void Setup()
        {
            _l = new Core.LogFile();
        }

        [TestCase(37)]
        public void TestLogDoorLocked(int ID)
        { 
            _l.LogDoorLocked(ID);
            string line = File.ReadLines(@"log.txt").Last();
            string expected = string.Format("Door Locked with ID: " + ID); 
            Assert.AreEqual(expected, line);
        }

        [TestCase(17)]
        public void TestLogDoorUnlocked(int ID)
        {
            _l.LogDoorUnlocked(ID);
            string line = File.ReadLines(@"log.txt").Last();
            string expected = string.Format("Door Unlocked with ID: " + ID);
            Assert.AreEqual(expected, line);
        }
    }
}