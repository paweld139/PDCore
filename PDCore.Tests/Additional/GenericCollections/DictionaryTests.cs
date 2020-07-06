using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Tests.Additional.GenericCollections
{
    [TestClass]
    public class DictionaryTests
    {
        [TestMethod]
        public void Can_Use_Dictionary_As_Map()
        {
            var map = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            }; //Zoptymalizowany pod kątem wydajnego wstawiania, usuwania i wyszukiwania pod indeksie, natomiast SortedList pod kątem iteracji.

            Assert.AreEqual("one", map[1]);
        }

        [TestMethod]
        public void Can_Search_Key_With_ContainsKey()
        {
            var map = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            Assert.IsTrue(map.ContainsKey(2));
        }

        [TestMethod]
        public void Can_Remove_By_Key()
        {
            var map = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            map.Remove(1);

            Assert.AreEqual(1, map.Count);
        }
    }
}
