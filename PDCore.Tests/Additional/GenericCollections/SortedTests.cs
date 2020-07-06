using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Tests.Additional.GenericCollections
{
    [TestClass]
    public class SortedTests
    {
        [TestMethod]
        public void Can_Use_Sorted_List()
        {
            var list = new SortedList<int, string>
            {
                { 3, "three" },
                { 1, "one" },
                { 2, "two" }
            }; //Szybka iteracja w przeciwieństwie do SortedDictionary, klucze także muszą być unikalne. Mało wydajne wstawianie i usuwanie, dość wydajne szukanie.

            Assert.AreEqual(0, list.IndexOfKey(1));
            Assert.AreEqual(1, list.IndexOfValue("two"));
        }

        [TestMethod]
        public void Can_Use_SortedSet()
        {
            var set = new SortedSet<int>
            {
                3,
                1,
                2
            };

            var enumerator = set.GetEnumerator();
            enumerator.MoveNext();

            Assert.AreEqual(1, enumerator.Current);
        }
    }
}
