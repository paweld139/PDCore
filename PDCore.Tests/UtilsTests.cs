using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Utils;

namespace PDCore.Tests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void CanGetCallerMethodName()
        {
            string actual = ObjectUtils.GetCallerMethodName(1);

            string expected = "CanGetCallerMethodName";


            Assert.AreEqual(expected, actual);
        }
    }
}
