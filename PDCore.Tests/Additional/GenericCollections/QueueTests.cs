using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Tests.Additional.GenericCollections
{
    [TestClass]
    public class QueueTests
    {
        [TestMethod]
        public void Can_Peek_At_Next_Item()
        {
            var queue = new Queue<int>();
            queue.Enqueue(1); //Zakolejkowanie
            queue.Enqueue(2);
            queue.Enqueue(3);

            Assert.AreEqual(1, queue.Peek()); //Zerknięcie
        }

        [TestMethod]
        public void Can_Search_With_Contains()
        {
            var queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            Assert.IsTrue(queue.Contains(2));
        }

        [TestMethod]
        public void Can_Convert_Queue_To_Array()
        {
            var queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var asArray = queue.ToArray(); //Kopia kolejki w formie tablicy
            queue.Dequeue();

            Assert.AreEqual(1, asArray[0]);
            Assert.AreEqual(2, queue.Count);
        }
    }
}
