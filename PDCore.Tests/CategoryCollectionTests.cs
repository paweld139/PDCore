using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Helpers;
using PDCore.Helpers.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Tests
{
    [TestClass]
    public class CategoryCollectionTests
    {
        [TestMethod]
        public void CategoryAndNamedObjectAreSorted()
        {
            CategoryCollection categoryCollection = new CategoryCollection();

            categoryCollection.AddRange("Kursy", new NamedObject("Python"),
                                                 new NamedObject("C#"),
                                                 new NamedObject("Java"))

                              .AddRange("Audiobooki", new NamedObject("HTML"),
                                                      new NamedObject("CSS"),
                                                      new NamedObject("JavaScript"),
                                                      new NamedObject("Angular"));

            var firstElement = categoryCollection.ElementAt(0);
            var secondElement = categoryCollection.ElementAt(1);

            Assert.AreEqual("Audiobooki", firstElement.Key);
            Assert.AreEqual("HTML", firstElement.Value.ElementAt(2).Name);
            Assert.AreEqual("Java", secondElement.Value.ElementAt(1).Name);
        }

        [TestMethod]
        public void CategoryAndNamedObjectAreUnique()
        {
            CategoryCollection categoryCollection = new CategoryCollection();

            categoryCollection.Add("Kursy", new NamedObject("Python"))
                              .Add("Kursy", new NamedObject("Python"))
                              .Add("Kursy", new NamedObject("Python"));

            categoryCollection.Add("Audiobooki", new NamedObject("HTML"))
                              .Add("Audiobooki", new NamedObject("CSS"))
                              .Add("Audiobooki", new NamedObject("HTML"))
                              .Add("Audiobooki", new NamedObject("HTML"));

            var firstElement = categoryCollection.ElementAt(0);
            var secondElement = categoryCollection.ElementAt(1);

            Assert.AreEqual(2, categoryCollection.Count);
            Assert.AreEqual(2, firstElement.Value.Count);
            Assert.AreEqual(1, secondElement.Value.Count);
        }

        [TestMethod]
        public void ThrowsArgumentExceptionWhenElementsToAddAreNotSpecified()
        {
            CategoryCollection categoryCollection = new CategoryCollection();

            void addRange(IEnumerable<NamedObject> range) => categoryCollection.AddRange("Kursy", range);

            Assert.ThrowsException<ArgumentNullException>(() => addRange(null));
            Assert.ThrowsException<ArgumentException>(() => addRange(Enumerable.Empty<NamedObject>()));
        }
    }
}
