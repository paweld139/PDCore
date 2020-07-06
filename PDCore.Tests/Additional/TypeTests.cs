using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Helpers.DataStructures.Book;

namespace PDCore.Tests.Additional
{  
    public delegate string WriteLogDelegate(string logMessage);

    [TestClass]
    public class TypeTests
    {
        int count = 0;

        [TestMethod]
        public void WriteLogDelegateCanPointToMethod()
        {
            WriteLogDelegate log = ReturnMessage;

            log += ReturnMessage;
            log += IncrementCount;

            var result = log("Hello!");
            Assert.AreEqual(3, count);
            Assert.AreEqual("hello!", result);
        }

        string IncrementCount(string message)
        {
            count++;
            return message.ToLower();
        }

        string ReturnMessage(string message)
        {
            count++;
            return message;
        }


        [TestMethod]
        public void ValueTypesAlsoPassByValue()
        {
            var x = GetInt();
            SetInt(ref x);

            Assert.AreEqual(42, x);
        }

        private void SetInt(ref int z)
        {
            z = 42;
        }

        private int GetInt()
        {
            return 3;
        }

        [TestMethod]
        public void CSharpCanPassByRef()
        {
            var book1 = GetBook("Book 1");
            GetBookSetName(ref book1, "New Name");

            Assert.AreEqual("New Name", book1.Name);
        }

        private void GetBookSetName(ref InMemoryBook book, string name)
        {
            book = new InMemoryBook(name);
        }

        [TestMethod]
        public void CSharpIsPassByValue()
        {
            var book1 = GetBook("Book 1");
            GetBookSetName(book1, "New Name");

            Assert.AreEqual("Book 1", book1.Name);
        }

        private void GetBookSetName(InMemoryBook book, string name)
        {
            _ = book;

            book = new InMemoryBook(name);  
            
            _ = book;
        }

        [TestMethod]
        public void CanSetNameFromReference()
        {
            var book1 = GetBook("Book 1");
            SetName(book1, "New Name");

            Assert.AreEqual("New Name", book1.Name);
        }

        private void SetName(InMemoryBook book, string name)
        {
            book.Name = name;
        }

        [TestMethod]
        public void StringsBehaveLikeValueTypes()
        {
            string name = "Scott";
            var upper = MakeUppercase(name);

            Assert.AreEqual("Scott", name);
            Assert.AreEqual("SCOTT", upper);
        }

        private string MakeUppercase(string parameter)
        {
            return parameter.ToUpper();
        }

        [TestMethod]
        public void GetBookReturnsDifferentObjects()
        {
            var book1 = GetBook("Book 1");
            var book2 = GetBook("Book 2");

            Assert.AreEqual("Book 1", book1.Name);
            Assert.AreEqual("Book 2", book2.Name);
            Assert.AreNotSame(book1, book2);
        }

        [TestMethod]
        public void TwoVarsCanReferenceSameObject()
        {
            var book1 = GetBook("Book 1");
            var book2 = book1;

            Assert.AreSame(book1, book2);
            Assert.IsTrue(Object.ReferenceEquals(book1, book2));
        }

        InMemoryBook GetBook(string name)
        {
            return new InMemoryBook(name);
        }
    }
}
