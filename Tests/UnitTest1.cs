using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SGMParser;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string path = @"../../../Articles/reut2-000.sgm";
            SGMLReader reader = new SGMLReader();
            reader.ReadSGML(path);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string baseDirectory = @"../../../Articles";
            SGMLReader reader = new SGMLReader();
            reader.ReadAllSGMLFromDirectory(baseDirectory);
        }
    }
}
