using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Utils
{
	/// <summary>
	/// Summary description for DataHelper.
	/// </summary>
	public class DataHelper
	{
		public DataHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /// <summary>
        /// You will need to cast return value to appropriate type
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="dataType">Accepts values for String, Boolean, Int32, Decimal and DateTime</param>
        /// <returns>Null values will return minValues for type. A null boolean will throw an exception</returns>
        public static object GetColumnValue(DataRow row, string columnName, System.Data.DbType dataType)
        {
            switch (dataType.ToString())
            {
                case "String":
                    return DataHelper.GetColumnValue(row, columnName, dataType, null);
                case "Boolean":
                    return DataHelper.GetColumnValue(row, columnName, dataType, false);
                case "Int32":
                    return DataHelper.GetColumnValue(row, columnName, dataType, int.MinValue);
                case "Decimal":
                    return DataHelper.GetColumnValue(row, columnName, dataType, decimal.MinValue);
                case "DateTime":
                    return DataHelper.GetColumnValue(row, columnName, dataType, Utils.Constants._MinDate);
            }

            return null;
        }
        public static object GetColumnValue(DataRow row, string columnName, System.Data.DbType dataType, object defaultValue)
        {
            object obj = DataHelper.GetColumnValue(row, columnName);

            switch (dataType.ToString())
            {
                case "String":
                    return (obj == null) ? defaultValue : obj.ToString();
                case "Boolean":
                    try
                    {
                        return (obj.ToString() == "1");
                    }
                    catch
                    {
                        return bool.Parse(obj.ToString());
                    }
                    
                    //return (bool)defaultValue;
                case "Int32":
                    return (obj == null || obj.ToString().Trim().Length == 0) ? (int)defaultValue : (int)obj;
                case "Decimal":
                    return (obj == null || obj.ToString().Trim().Length == 0) ? (decimal)defaultValue : (decimal)obj;
                case "DateTime":
                    return (obj == null || obj.ToString().Trim().Length == 0) ? (DateTime)defaultValue : (DateTime)obj;
            }

            return null;
        }
        private static object GetColumnValue(DataRow row, string columnName)
        {   
            if(row != null && row.ItemArray != null && row.ItemArray.Length > 0)
            {
                int colIdx = row.Table.Columns.IndexOf(columnName);

                if(colIdx != -1)
                    return row.ItemArray.GetValue(colIdx);
            }

            return null;
        }

		/// <summary>
		/// returns the number of rows affected by the query
		/// </summary>
		public static int ExecuteNonQuery(StringBuilder query, string dsn)
		{
			int rowsAffected = -1;

			using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(dsn))
			{
				query.Insert(0, string.Format("BEGIN TRANSACTION\r\n\r\n"));
				query.AppendFormat("\r\nIF @@ERROR <> 0 BEGIN\r\n");
				query.AppendFormat("ROLLBACK TRANSACTION\r\n");
				query.AppendFormat("RETURN\r\n");
				query.AppendFormat("END\r\n\r\n");
				query.AppendFormat("COMMIT TRANSACTION\r\n");

				System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query.ToString(), conn);
				conn.Open();
				rowsAffected = cmd.ExecuteNonQuery();

				cmd.Dispose();
			}//using statement closes/disposes connection

			return rowsAffected;
		}

		public static DataSet ExecuteQuery(StringBuilder query, string dsn)
		{
			return ExecuteQuery(query, null, dsn);
		}
		public static DataSet ExecuteQuery(StringBuilder query, System.Collections.ArrayList queryParams, string dsn)
		{
			using (System.Data.DataSet ds = new System.Data.DataSet())
			{
				using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(dsn))
				{
					query.Insert(0, string.Format("BEGIN TRANSACTION\r\n\r\n"));
					query.AppendFormat("\r\nIF @@ERROR <> 0 BEGIN\r\n");
					query.AppendFormat("ROLLBACK TRANSACTION\r\n");
					query.AppendFormat("RETURN\r\n");
					query.AppendFormat("END\r\n\r\n");
					query.AppendFormat("COMMIT TRANSACTION\r\n");

					System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query.ToString(), conn);
					if(queryParams != null)
					{
						foreach(System.Web.UI.Pair p in queryParams)
						{
							cmd.Parameters.AddWithValue((string)p.First,(string)p.Second);
						}
					}

					System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
					
					conn.Open();

					da.Fill(ds);

					da.Dispose();
				}//using statement closes/disposes connection

				return ds;
			}
		}
        public static DataSet ExecuteQuery_ListItem(StringBuilder query, List<ListItem> queryParams, string dsn)
        {
            using (System.Data.DataSet ds = new System.Data.DataSet())
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(dsn))
                {
                    query.Insert(0, string.Format("BEGIN TRANSACTION\r\n\r\n"));
                    query.AppendFormat("\r\nIF @@ERROR <> 0 BEGIN\r\n");
                    query.AppendFormat("ROLLBACK TRANSACTION\r\n");
                    query.AppendFormat("RETURN\r\n");
                    query.AppendFormat("END\r\n\r\n");
                    query.AppendFormat("COMMIT TRANSACTION\r\n");

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query.ToString(), conn);
                    if (queryParams != null)
                    {
                        foreach (ListItem li in queryParams)
                            cmd.Parameters.AddWithValue(li.Text, li.Value);
                    }

                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);

                    conn.Open();

                    da.Fill(ds);

                    da.Dispose();
                }//using statement closes/disposes connection

                return ds;
            }
        }
	}
}
