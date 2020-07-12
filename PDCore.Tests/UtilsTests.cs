using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Utils;
using System.Linq;
using FTCore.CoreLibrary.SQLLibrary;
using System.Data.SqlClient;
using System.Data;

namespace PDCore.Tests
{
    [TestClass]
    public class UtilsTests
    {
        #region ObjectUtils

        [TestMethod]
        public void CanGetCallerMethodName()
        {
            string actual = ObjectUtils.GetCallerMethodName(1);

            string expected = "CanGetCallerMethodName";


            Assert.AreEqual(expected, actual);
        }

        #endregion


        #region SqlUtils

        [TestMethod]
        public void CanGetConnectionStringByNameOrConnectionString()
        {
            string[] texts =
            {
                "Main",
                "DefaultConnection",
                "Medic4You",
                "Data Source=LAPTOP-JHQ9SF1E\\SQLEXPRESSS;Initial Catalog=MainTest;User ID=sa;Password=hasloos",
                "Data Source=LAPTOP-JHQ9SF1E\\SQLEXPRESSS;Initial Catalog=MedicSylwia;User ID=sa;Password=hasloos"
            };

            var actual = texts.Select(SqlUtils.GetConnectionString);

            var expected = new[] { null, texts[3], texts[4], texts[3], texts[4] };


            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void CanTestConnectionString()
        {
            string[] texts =
            {
                "Main",
                "DefaultConnection",
                "Medic4You",
                "Data Source=LAPTOP-JHQ9SF1E\\SQLEXPRESSS;Initial Catalog=MainTest;User ID=sa;Password=hasloos",
                "Data Source=LAPTOP-JHQ9SF1E\\SQLEXPRESSS;Initial Catalog=MedicSylwia;User ID=sa;Password=hasloos",
                "Data Source=LAPTOP-JHQ9SF1E\\SQLEXPRESSS;Initial Catalog=MainTest;User ID=sa;Password=hasloos2"
            };

            var actual = texts.Select(SqlUtils.TestConnectionString);

            var expected = new[] { false, false, false, true, true, false };


            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void LocalSqlHelperClosesSqlConnectionAfterDispose()
        {
            LocalSqlHelper localSqlHelper = SqlUtils.EnableSQLConnection();


            SqlConnection sqlConnection = localSqlHelper.GetOpenConnection();

            Assert.IsTrue(sqlConnection.State == ConnectionState.Open);


            localSqlHelper.Dispose();

            Assert.IsTrue(sqlConnection.State == ConnectionState.Closed);
        }

        #endregion
    }
}
