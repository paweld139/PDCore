using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Tests.Additional.GenericCollections
{
    [TestClass]
    public class LinkedListTests
    {
         /*
         * Lepiej użyć LinkedList a nie List, gdy jest wiele modyfikacji położenia elementów, wiele wstawiania i usuwania wewnątrz.
         * Lista jest wydajna tylko jak dodaje się lub usuwa elementy na końcu. LinkedList operuje na węzłach.
         * Dodawanie przed i po węźle lub przeszukiwanie sekwencyjne jest wydajne w przypadku LinkedList.
         * W przypadku LinkedList stały czas mają operacje: Append, Prepend, Insertion after and before node, removal of node.
         */

        [TestMethod]
        public void Can_Add_After()
        {
            var list = new LinkedList<string>();
            list.AddFirst("Hello");
            list.AddLast("World");
            list.AddAfter(list.First, "there");

            Assert.AreEqual("there", list.First.Next.Value);
        }

        [TestMethod]
        public void Can_Remove_Last()
        {
            var list = new LinkedList<string>();
            list.AddFirst("Hello");
            list.AddLast("World");
            list.RemoveLast();

            Assert.AreEqual(list.First, list.Last);
        }

        [TestMethod]
        public void Can_Find_Items()
        {
            var list = new LinkedList<string>();
            list.AddFirst("Hello");
            list.AddLast("World");

            Assert.IsTrue(list.Contains("Hello"));
        }
    }
}
