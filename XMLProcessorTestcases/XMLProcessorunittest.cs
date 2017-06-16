using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLProcessor_Assessment2_;
using System.Windows;

namespace XMLProcessorTestcases
{
    [TestClass]
    public class XMLProcessorunittest
    {
        [TestMethod]
        public void TestCreateloggerfile()
        {
            XMLProcessorform objXmlprocessorform = new XMLProcessorform();
            string returnresult = objXmlprocessorform.Createloggerfile("LoggerDocument");
            string expectedresult = "True";
            Assert.Equals(returnresult,expectedresult);
        }
    }
}
