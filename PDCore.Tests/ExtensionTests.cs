using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Enums;
using PDCore.Extensions;
using PDCore.Utils;

namespace PDCore.Tests
{
    [TestClass]
    public class ExtensionTests
    {
        #region StringExtension

        [TestMethod]
        public void CanAddSpacesToString()
        {
            string text = "PawełDywanYoJa";

            text = text.AddSpaces();

            string expected = "Paweł Dywan Yo Ja";

            Assert.AreEqual(expected, text);
        }

        [TestMethod]
        public void CanConvertStringToEnum()
        {
            string enumString = "WSS";

            var actual = enumString.ParseEnum<CertificateType>();

            var expected = CertificateType.WSS;

            Assert.AreEqual(expected, actual);
        }

        #endregion


        #region ObjectExtension

        [TestMethod]
        public void CanCalculateSampledAverage()
        {
            var numbers = new double[] { 1, 2, 3, 4, 5, 6 };

            var actual = numbers.SampledAverage();

            var expected = 3.0;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanMultiply()
        {
            var multiplicand = 2.0;

            var multiplier = 3;

            var actual = multiplicand.Multiply(multiplier);

            var expected = 6.0;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanExtractDescriptionFromEnumValue()
        {
            CertificateType enumValue = CertificateType.WSS;

            string actual = enumValue.GetDescription();

            string expected = "Certyfikat WSS służący do podpisywania wiadomości";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanExtractEnumValueFromDescription()
        {
            string description = "Certyfikat służący do zabezpieczenia komunikacji";

            CertificateType actual = description.ToEnumValue<CertificateType>();

            CertificateType expected = CertificateType.TLS;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
