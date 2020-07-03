using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Extensions;
using PDCore.Utils;

namespace PDCore.Tests
{
    [TestClass]
    public class PDCoreTests
    {
        [TestMethod]
        public void CanAddSpacesToString()
        {
            string text = "PawełDywanYoJa";

            text = text.AddSpaces();

            string expected = "Paweł Dywan Yo Ja";

            Assert.AreEqual(expected, text);
        }
    }
}
