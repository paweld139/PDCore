using FTCore.CoreLibrary.SQLLibrary;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PDCore.Utils
{
    public static class SqlUtils
    {
        public static string SQLQuery(string objectName = null, string projection = "*", string selection = null, string joins = "", string order = null, 
            string orderType = "", bool isUpdate = false, bool isInsert = false, bool isDelete = false, bool isStoredProcedure = false, bool isFunction = false, string parameters = "")
        {
            StringBuilder q = new StringBuilder("set dateformat ymd; ");

            if (isStoredProcedure)
            {
                q.AppendFormat("exec {0} {1}", objectName, parameters);
            }
            else if (isFunction)
            {
                q.AppendFormat("select {0} from {1}({2})", projection, objectName, parameters);
            }
            else
            {
                if (isUpdate)
                {
                    q.AppendFormat("update {0} set {1}", objectName, projection);
                }
                else if (isInsert)
                {
                    q.AppendFormat("insert into {0} ({1}) values ({2})", objectName, projection, selection);
                }
                else if (isDelete)
                {
                    q.AppendFormat("delete from {0}", objectName);
                }
                else
                {
                    q.AppendFormat("select {0} from {1} {2}", projection, objectName, joins);
                }

                if (!isInsert && !string.IsNullOrWhiteSpace(selection))
                {
                    q.AppendFormat(" where {0}", selection);
                }
            }

            if (!string.IsNullOrWhiteSpace(order))
            {
                q.AppendFormat(" order by {0} {1}", order, orderType);
            }

            q.Append(";");

            string query = q.ToString();

            return query;
        }

        public static void FindByDate<T>(string dateF, string dateT, ref IQueryable<T> result) where T : class, IByDateFindable
        {
            if (result == null)
            {
                return;
            }

            if (!StringUtils.AreNullOrWhiteSpace(dateF, dateT))
            {
                DateTime.TryParse(dateF, out DateTime dateFrom);

                DateTime.TryParse(dateT, out DateTime dateTo);

                if (dateTo != DateTime.MinValue)
                {
                    dateTo = dateTo.AddDays(1).AddSeconds(-1);
                }

                if (StringUtils.AreNotNullOrWhiteSpace(dateF, dateT))
                {
                    if (dateTo > dateFrom)
                    {
                        result = result.Where(x => x.Date >= dateFrom && x.Date <= dateTo);
                    }
                }
                else if (string.IsNullOrWhiteSpace(dateF))
                {
                    result = result.Where(x => x.Date <= dateTo);
                }
                else
                {
                    result = result.Where(x => x.Date >= dateFrom);
                }
            }
        }

        public static DataTable GetDataTable(string query)
        {
            DataTable dataTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            conn.Close();
            conn.Dispose();
            da.Dispose();

            return dataTable;
        }

        public static DataSet GetDataSet(string query)
        {
            DataSet dataSet = new DataSet();
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataSet);
            conn.Close();
            conn.Dispose();
            da.Dispose();

            return dataSet;
        }

        public static LocalSqlHelper EnableSQLConnection()
        {
            LocalSqlHelper helper = new LocalSqlHelper(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            return helper;
        }

        public static string GetSaveChangesResults(params string[] results)
        {
            string result = WebUtils.ResultOkIndicator;

            foreach (string item in results)
            {
                if (item != result)
                {
                    result = item;

                    break;
                }
            }

            return result;
        }
    }
}
