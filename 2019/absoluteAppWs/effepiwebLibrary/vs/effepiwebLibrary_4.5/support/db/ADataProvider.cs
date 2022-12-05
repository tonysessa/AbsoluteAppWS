using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Globalization;
using System.Data.Common;
using Support.Web;

namespace Support.db
{
    public abstract class ADataProvider: IDataProvider
    {
        protected DbConnection dbConn = null;
        //protected QueryParams[] query_params;

        protected ADataProvider()
        {
            //query_params = null;
        }

        /// <summary>
        /// Metodo per la memorizzazione di stringhe per la parametrizzazione delle query
        /// </summary>
        /// <param name="params_name">array di stringhe contente i nomi dei parametri contenuti nella query</param>
        /// <param name="parameters">array di stringhe contenti i valori da parametrizzare nella query</param>
        /*
        public void SetQueryParams(string[] params_name, string[] parameters)
		{
			if ((params_name == null)&&(parameters == null))
			{
				query_params = null;
				return;
			}
			query_params = new QueryParams[parameters.Length];
			for (int i=0; i<parameters.Length; i++)
			{
				query_params[i].param_name = params_name[i];
				query_params[i].param_value = parameters[i];
			}
		}
        */

        public virtual String getConnectionString()
        {
            return WebContext.getConfig("%.dataConnection"); 
        }

        public abstract DbConnection createDbConnection( String connString );

        public virtual bool open()
        {
            bool bRet = false;
            if (!isOpen())
            {
                try
                {
                    dbConn = createDbConnection(getConnectionString());
                    dbConn.Open();
                    bRet = true;
                }
                catch (Exception)
                {
                    dbConn = null;
                }
            }
            else
            {
                bRet = true;
            }
            return bRet;
        }

        public virtual void close()
        {
            try
            {
                if (isOpen())
                {
                    dbConn.Close();
                    dbConn.Dispose();
                }
            }
            catch (Exception)
            {
            }
            dbConn = null;
        }

        public virtual bool isOpen()
        {
            if (dbConn == null)
                return false;
            try
            {
                return dbConn.State == ConnectionState.Open;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected int autoOpen()
        {
            if (isOpen())
                return 1;
            if (!open())
                return 0;
            //
            return 2;
        }

        protected void autoClose( int ts )
        {
            if (ts == 2)
                close();
        }

        public abstract SimpleDataSet executeQuery(string query, int maxRows);

        public SimpleDataSet executeQuery(string query)
        {
            return executeQuery(query, -1);
        }

        public abstract bool executeStatement(string query);

        public abstract bool executeStatement(string query, Hashtable paramters);

        public abstract BlockDataSet executeQuery(string query, int blockSize, int currentBlock);

        public virtual bool update(string tableName, Hashtable fields, string keyExpr)
        {
            int count = 0;
            String sql = "UPDATE " + tableName + " SET ";
            foreach (String k in fields.Keys)
            {
                sql += (count > 0 ? "," : "") + k + " = ";
                String v = "@" + k; //toDbFormat(fields[k]);
                sql += v;
                count++;
            }
            //
            if (!String.IsNullOrEmpty(keyExpr))
                sql += " WHERE " + keyExpr;
            //
            return executeStatement(sql,fields);
        }

        public virtual bool delete(string tableName, string keyExpr)
        {
            String sql = "DELETE FROM " + tableName;
            //
            if (!String.IsNullOrEmpty(keyExpr))
                sql += " WHERE " + keyExpr;
            //
            return executeStatement(sql);
        }

        public virtual bool insert(string tableName, Hashtable fields)
        {
            String names = "";
            String values = "";
            foreach (String k in fields.Keys)
            {
                names += (names.Length > 0 ? "," : "") + k;
                String v = "@" + k; //toDbFormat(fields[k]);
                values += (values.Length > 0 ? "," : "") + v;
            }
            String sql = "INSERT INTO " + tableName + " ( " + names + " ) VALUES ( " + values + " )";
            // 
            return executeStatement(sql,fields);
        }

        public virtual bool insert(string sSql)
        {
            return executeStatement(sSql);
        }

        public abstract long insertRetId(string tableName, Hashtable fields);

        public abstract long insertRetId(string sSql);

        public abstract DbDataReader openCursor(string sql);

        public virtual String toDbFormat(Object obj)
        {
            if (obj == null)
                return "NULL";
            // Byte array
            // if (obj is Byte[])
            //    return "";
            // Char
            if (obj is Char)
                return "'"+obj.ToString()+"'";
            // Boolean
            if (obj is Boolean)
                return obj.ToString();
            // Numeri senza decimali
            if (obj is Byte)
                return obj.ToString();
            if (obj is SByte)
                return obj.ToString();
            if (obj is Int16)
                return obj.ToString();
            if (obj is Int32)
                return obj.ToString();
            if (obj is Int64)
                return obj.ToString();
            if (obj is UInt16)
                return obj.ToString();
            if (obj is UInt32)
                return obj.ToString();
            if (obj is UInt64)
                return obj.ToString();
            // Numeri con decimali
            if (obj is Single)
                return ((Single)obj).ToString(NumberFormatInfo.InvariantInfo);
            if (obj is Decimal)
                return ((Decimal)obj).ToString(NumberFormatInfo.InvariantInfo);
            if (obj is Double)
                return ((Double)obj).ToString(NumberFormatInfo.InvariantInfo);
            // TimeSpan
            if (obj is TimeSpan)
                return ((TimeSpan)obj).ToString();
            // Date
            if (obj is DateTime)
                return ((DateTime)obj).ToString("dd/MM/yyyy HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo);
            // Stringa
            return "'" + obj.ToString().Replace("'", "''") + "'";
        }

    }
}
