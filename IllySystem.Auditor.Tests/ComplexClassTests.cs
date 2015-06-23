using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IllySystem.Auditor.Tests
{
    [TestClass]
    public class ComplexClassTests
    {

        public string Forename { get; set; }
        public DateTime DateOf_birth { get; set; }
        public List<string> Basket { get; set; }
        public int _SpecialNumber { get; set; }

        public ComplexClassTests()
        {
        }

        public ComplexClassTests(string forename, DateTime dob, List<string> basket, int number)
        {
            this.Forename = forename;
            this.DateOf_birth = dob;
            this.Basket = basket;
            this._SpecialNumber= number;
        }

        [TestMethod]
        public void TestComplexClassTests_Equals()
        {
            List<string> myFirstList = new List<string>();
            myFirstList.Add("Item1");
            myFirstList.Add("Item2");
            myFirstList.Add("Item3");

            List<string> mySecondList = new List<string>();
            mySecondList.Add("Item1");
            mySecondList.Add("Item2");
            mySecondList.Add("Item3");

            ComplexClassTests firstObj = new ComplexClassTests("Guillaume", new DateTime(1989, 06, 16), myFirstList, 8);
            ComplexClassTests secondObj = new ComplexClassTests("Guillaume", new DateTime(1989, 06, 16), mySecondList, 8);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(0, differences.Count);
        }

        [TestMethod]
        public void TestComplexClassTests_Diff()
        {
            List<string> myFirstList = new List<string>();
            myFirstList.Add("Ite1");
            myFirstList.Add("Ite2");
            myFirstList.Add("Ite3");

            List<string> mySecondList = new List<string>();
            mySecondList.Add("Ite1");
            mySecondList.Add("Ite20");
            mySecondList.Add("Ite3");

            ComplexClassTests firstObj = new ComplexClassTests("Guillaume", new DateTime(1989, 06, 16), myFirstList, 8);
            ComplexClassTests secondObj = new ComplexClassTests("Guillaume", new DateTime(1989, 06, 16), mySecondList, 20);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(2, differences.Count);
        }

        [TestMethod]
        public void TestComplexClassTests_DiffPlus()
        {
            List<string> myFirstList = new List<string>();
            myFirstList.Add("Ite1");
            myFirstList.Add("Ite2");
            myFirstList.Add("Ite3");

            List<string> mySecondList = new List<string>();
            mySecondList.Add("Ite1");
            mySecondList.Add("Ite20");
            mySecondList.Add("Ite42");

            ComplexClassTests firstObj = new ComplexClassTests("Guillaume", new DateTime(1989, 06, 16), myFirstList, 8);
            ComplexClassTests secondObj = new ComplexClassTests("William", new DateTime(1989, 06, 16), mySecondList, 20);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(4, differences.Count);
            Assert.AreEqual("Forename changed from Guillaume to William", differences[0]);
            Assert.AreEqual("Special Number changed from 8 to 20", differences[3]);
        }
    }
}
