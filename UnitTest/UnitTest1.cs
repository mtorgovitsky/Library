using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookLib;


namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Book b = new Book("Boker Tov", DateTime.Now);
        }
    }
}
