using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using IllySystem.Auditor;

namespace IllySystem.Auditor.Tests
{
    [TestClass]
    public class SimpleClassTests
    {
        public string Forename { get; set; }
        public DateTime DateOf_birth { get; set; }
        public int _SpecialNumber { get; set; }

        public SimpleClassTests()
        {
        }

        public SimpleClassTests(string forename, DateTime dob, int number)
        {
            this.Forename = forename;
            this.DateOf_birth = dob;
            this._SpecialNumber= number;
        }


        [TestMethod]
        public void TestSimpleClass_Equals()
        {

            SimpleClassTests firstObj = new SimpleClassTests("Guillaume", new DateTime(1989, 06, 16), 8);
            SimpleClassTests secondObj = new SimpleClassTests("Guillaume", new DateTime(1989, 06, 16), 8);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(0, differences.Count);
        }

        [TestMethod]
        public void TestSimpleClass_Diff()
        {

            SimpleClassTests firstObj = new SimpleClassTests("Guillaume", new DateTime(1989, 06, 16), 8);
            SimpleClassTests secondObj = new SimpleClassTests("William", new DateTime(1989, 06, 16), 42);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(2, differences.Count);
            Assert.AreEqual("Forename changed from Guillaume to William", differences[0]);
            Assert.AreEqual("Special Number changed from 8 to 42", differences[1]);
        }
    }
}
