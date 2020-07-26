using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Extensions;
using PDCore.Helpers.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Tests
{
    [TestClass]
    public class TranslatorTests
    {
        [TestMethod]
        public void CanTranslateErrors()
        {
            ErrorTranslator errorTranslator = new ErrorTranslator();

            var actual = new[] 
            { 
                "Passwords must have at least one digit ('0'-'9').",
                "Passwords must have at least one non letter or digit character.",
                "Invalid token.",
                "Passwords must have at least one uppercase ('A'-'Z').",
                "Name is already taken.",
                "E-mail is already taken.",
                "Pawl is taken?!",
                "Passwords must have at least one digit ('0'-'9'). Name is already taken."
            };

            for (int i = 0; i < actual.Length; i++)
            {
                errorTranslator.TranslateText(ref actual[i]);
            }

            var expected = new[]
            {
                "Hasło musi zawierać co najmniej jedną cyfrę.",
                "Hasło musi zawierać co najmniej jeden znak specjalny.",
                "Niepoprawny token.",
                "Hasło musi zawierać co najmniej jedną wielką literę.",
                "E-mail jest już zajęty.",
                "E-mail jest już zajęty.",
                "Pawl jest zajęty?!",
                "Hasło musi zawierać co najmniej jedną cyfrę. E-mail jest już zajęty."
            };

            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
