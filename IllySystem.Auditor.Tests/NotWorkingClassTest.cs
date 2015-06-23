using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IllySystem.Auditor.Tests
{
    /// <summary>
    /// Description résumée pour NotWorkingClassTest
    /// </summary>
    [TestClass]
    public class NotWorkingClassTest
    {
        public Dictionary<int, List<string>> Basket { get; set; }

        public NotWorkingClassTest(List<string> list, int number)
        {
            Basket = new Dictionary<int, List<string>>();
            Basket.Add(number, list);
        }

        [TestMethod]
        public void NotWorkingClassTest_Equals()
        {
            List<string> myList = new List<string>();
            myList.Add("Item1");
            myList.Add("Item2");
            myList.Add("Item3");

            NotWorkingClassTest firstObj = new NotWorkingClassTest(myList, 1);
            NotWorkingClassTest secondObj = new NotWorkingClassTest(myList, 1);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(0, differences.Count);
        }

        [TestMethod]
        public void NotWorkingClassTest_Diff()
        {
            List<string> myFirstList = new List<string>();
            myFirstList.Add("Item1");
            myFirstList.Add("Item2");
            myFirstList.Add("Item3");

            List<string> mySecondList = new List<string>();
            mySecondList.Add("Item1");
            mySecondList.Add("Item42");
            mySecondList.Add("Item3");

            NotWorkingClassTest firstObj = new NotWorkingClassTest(myFirstList, 1);
            NotWorkingClassTest secondObj = new NotWorkingClassTest(mySecondList, 2);

            List<string> differences = Auditor.GetChanges(firstObj, secondObj);
            Assert.AreEqual(2, differences.Count);
        }
    }
}
