using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Helpers.DataStructures.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Tests
{
    [TestClass]
    public class BookTests
    {
        [TestMethod]
        public void BookCalculatesAnAverageGrade()
        {
            // arrange
            var book = new InMemoryBook("");
            book.AddGrade(89.1);
            book.AddGrade(90.5);
            book.AddGrade(77.3);

            // act
            var result = book.GetStatistics();

            // assert
            Assert.AreEqual(85.6, result.Average, 1);
            Assert.AreEqual(90.5, result.Max, 1);
            Assert.AreEqual(77.3, result.Min, 1);
            Assert.AreEqual('B', result.Letter);
        }

        [TestMethod]
        public void BookValidatesGrade()
        {
            // arrange
            var book = new InMemoryBook("");

            // act
            var result = book.GetStatistics().Max;

            // assert
            Assert.AreEqual(double.MinValue, result);
            Assert.ThrowsException<ArgumentException>(() => book.AddGrade(105));
        }
    }
}
