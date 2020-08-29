using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDCore.Utils;
using System.Linq;
using FTCore.CoreLibrary.SQLLibrary;
using System.Data.SqlClient;
using System.Data;
using Moq;
using PDCore.Services.IServ;
using PDCore.Extensions;

namespace PDCore.Tests
{
    [TestClass]
    public class UtilsTests
    {
        #region ObjectUtils

        [TestMethod]
        public void CanGetCallerMethodName()
        {
            string actual = ReflectionUtils.GetCallerMethodName(1);

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

            int i = 0;

            Tuple<bool, int> func(string t, string p) => Tuple.Create(SqlUtils.TestConnectionString(t, p), i++);

            var actual = texts.Select(t => func(t, null).Item1).ToList();

            var actual2 = texts.Select(t => func(t, "System.Data.SqlClient").Item1);


            Assert.IsTrue(actual.SequenceEqual(actual2));


            var expected = new[] { false, false, false, true, true, false };


            Assert.IsTrue(expected.SequenceEqual(actual));

            Assert.AreEqual(12, i);
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

        [TestMethod]
        public void LocalSqlHelperNotClosesSqlConnectionImmediatelyAfterGetDataTable()
        {
            LocalSqlHelper localSqlHelper = SqlUtils.EnableSQLConnection();

            string query = SqlUtils.SQLQuery("Employees", selection: "Id in (3, 4, 7)", order: "Name");


            SqlConnection sqlConnection = localSqlHelper.GetOpenConnection();

            Assert.IsTrue(sqlConnection.State == ConnectionState.Open);


            DataTable dataTable = localSqlHelper.GetDataTable(query);

            Assert.IsNotNull(dataTable);

            Assert.IsFalse(sqlConnection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void CanGetDefaultSchema()
        {
            string[] connectionStrings =
            {
                "Data Source=LAPTOP-JHQ9SF1E\\SQLEXPRESSS;Initial Catalog=MainTest;User ID=sa;Password=hasloos",
                "Data Source=LAPTOP-JHQ9SF1E\\SQLEXPRESSS;Initial Catalog=MedicSylwia;User ID=sa;Password=hasloos"
            };

            var sqlConnections = connectionStrings.Select(s => SqlUtils.GetDbConnection(s, false)).Cast<SqlConnection>();

            var schemas = sqlConnections.Select(SqlUtils.GetDefaultSchema);

            var expected = new[] { "dbo", "dbo" };

            Assert.IsTrue(expected.SequenceEqual(schemas));
        }

        #endregion


        #region IOUtils

        [TestMethod]
        public void CanGetFileCountForDirectory()
        {
            string[] directories = { @"C:\Fraps", @"C:\Frapso" };

            var actual = directories.Select(d => IOUtils.GetFilesCount(d)).ToArray();

            int actual2 = IOUtils.GetFilesCount(directories[0], true);

            int expected = 0;

            Assert.IsTrue(actual2 > actual[0]);
            Assert.AreEqual(expected, actual[1]);
        }

        #endregion
    }
}
