using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace IllySystem.Auditor.Tests
{
    [TestClass]
    public class UserdefinedClassTest
    {

        public SimpleClassTests spClass;
        // Doesn't work with a Dictionary so 'private'...
        private Dictionary<int, List<string>> Basket { get; set; }

        public UserdefinedClassTest()
        {
        }

        public UserdefinedClassTest(SimpleClassTests myClass, List<string> list, int number)
        {
            this.spClass = myClass;
            Basket = new Dictionary<int, List<string>>();
            Basket.Add(number, list);
        }

        [TestMethod]
        public void UserdefinedClassTest_Equals()
        {
            List<string> myFirstList = new List<string>();
            myFirstList.Add("Item1");
            myFirstList.Add("Item2");
            myFirstList.Add("Item3");

            List<string> mySecondList = new List<string>();
            mySecondList.Add("Item1");
            mySecondList.Add("Item2");
            mySecondList.Add("Item3");

            SimpleClassTests firstSPObj = new SimpleClassTests("Guillaume", new DateTime(1989, 06, 16), 8);
            SimpleClassTests secondSPObj = new SimpleClassTests("Guillaume", new DateTime(1989, 06, 16), 8);

            UserdefinedClassTest firstObj = new UserdefinedClassTest(firstSPObj, myFirstList, firstSPObj._SpecialNumber);
            UserdefinedClassTest secondObj = new UserdefinedClassTest(secondSPObj, mySecondList, secondSPObj._SpecialNumber);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(0, differences.Count);
        }

        [TestMethod]
        public void UserdefinedClassTest_Diff()
        {
            List<string> myFirstList = new List<string>();
            myFirstList.Add("Item1");
            myFirstList.Add("Item2");
            myFirstList.Add("Item3");

            List<string> mySecondList = new List<string>();
            mySecondList.Add("Item1");
            mySecondList.Add("Item42");
            mySecondList.Add("Item3");

            SimpleClassTests firstSPObj = new SimpleClassTests("Guillaume", new DateTime(1989, 06, 16), 8);
            SimpleClassTests secondSPObj = new SimpleClassTests("Guillaume", new DateTime(1989, 07, 16), 8);

            // This class contains a private property so no differnces chould be found
            UserdefinedClassTest firstObj = new UserdefinedClassTest(firstSPObj, myFirstList, firstSPObj._SpecialNumber);
            UserdefinedClassTest secondObj = new UserdefinedClassTest(secondSPObj, mySecondList, secondSPObj._SpecialNumber);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(0, differences.Count);
        }

        [TestMethod]
        public void UserdefinedClassTest_DiffPlus()
        {
            List<string> myFirstList = new List<string>();
            myFirstList.Add("Item1");
            myFirstList.Add("Item2");
            myFirstList.Add("Item3");

            List<string> mySecondList = new List<string>();
            mySecondList.Add("Item1");
            mySecondList.Add("Item42");
            mySecondList.Add("Item16");

            SimpleClassTests firstSPObj = new SimpleClassTests("Guillaume", new DateTime(1989, 06, 16), 8);
            SimpleClassTests secondSPObj = new SimpleClassTests("William", new DateTime(1989, 06, 16), 10);

            UserdefinedClassTest firstObj = new UserdefinedClassTest(firstSPObj, myFirstList, firstSPObj._SpecialNumber);
            UserdefinedClassTest secondObj = new UserdefinedClassTest(secondSPObj, mySecondList, secondSPObj._SpecialNumber);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(0, differences.Count);
        }
    }
}
