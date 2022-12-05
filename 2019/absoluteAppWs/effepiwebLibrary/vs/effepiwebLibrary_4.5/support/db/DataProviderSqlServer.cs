using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Data.Common;
//

namespace Support.db
{

	/// <summary>
	/// Oggetto che si occupa della connessione al database, della gestione delle query e della formattazione dei risultati.
	/// </summary>
	/// <paramref name="xml_data">Oggetto privato di tipo XmlDataDocument che contine i records restituiti dalla query in formato XML</paramref>
	/// <paramref name="string_data">Stringa privata che contiene i records restituiti dalla query (usato per retrocompatibilità con le sezioni Rete)</paramref>
	/// <paramref name="query_params">Struttura privata di tipo QueryParams che contiene i dati per la parametrizzazione delle query</paramref>
	/// <paramref name="msg_nontrovato">Stringa privata che contiene il testo da restituire in caso di dati assenti</paramref>
	/// <paramref name="ConnStr">Stringa di connessione al database, prelevata da web.config</paramref>
    public class DataProviderSqlServer : ADataProvider
	{
		///<summary>
		///Costruttore dell'oggetto DataProvider.
		///In automatito setta a null query_params e xml_data.
		///</summary>
        public DataProviderSqlServer():base()
		{
		}

        public override DbConnection createDbConnection(String connString)
        {
            return new SqlConnection(connString);
        }

 				
		/// <summary>
		/// Metodo che data la query, restituisce il DataSet
		/// </summary>
		/// <param name="query">query usata per il recupero dei records</param>
		/// <returns>DataSet contenente le informaizoni richieste nella query</returns>
        public override SimpleDataSet executeQuery(string query, int maxRows)
		{
            int ao = autoOpen();
            //
			//DataTable rt = new DataTable();
			DataSet ds = new DataSet();			
		    //
            SqlCommand cmd = new SqlCommand(query);
 	        //
			cmd.CommandType = CommandType.Text;
            cmd.Connection = (SqlConnection)dbConn;
			//
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (maxRows > 0)
                    da.Fill(ds, 0, maxRows, "RET");
                else
                    da.Fill(ds);
			}
			catch (Exception ex)
			{
                string sErr = ex.Message;
                ds = null;
			}
            //
            autoClose(ao);
            //
			return ds!= null ? new SimpleDataSet(ds) : null;
		}

        public override BlockDataSet executeQuery(string query, int blockSize, int currentBlock)
        {
            int ao = autoOpen();
            //
            BlockDataSet ret = null;
            DataTable rt = new DataTable();
            DataSet ds = new DataSet();
            //
            SqlCommand cmd = new SqlCommand(query);
            //
            cmd.CommandType = CommandType.Text;
            cmd.Connection = (SqlConnection)dbConn;
            //
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, blockSize * currentBlock, blockSize+1, "RET");
                //
                ret = new BlockDataSet(ds);
                ret.blockSize = blockSize;
                ret.currentBlock = currentBlock;
                ret.hasPrev  = currentBlock>0;
                ret.hasNext  = ds.Tables[0].Rows.Count > blockSize;
                if (ret.Rows.Count > blockSize)
                    ret.Rows.RemoveAt(blockSize);
            }
            catch (Exception ex)
            {
                string sErr = ex.Message;
                ds = null;
            }
            //
            autoClose(ao);
            //
            return ret;
        }


        public override bool executeStatement(string query)
        {
            int ao = autoOpen();
            //
            bool bReturn = false;
            //        
            try
            {
                SqlCommand cmd = new SqlCommand(query, (SqlConnection)dbConn);
                cmd.ExecuteNonQuery();
                bReturn = true;

            }
            catch (Exception ex)
            {
                string sErr = ex.Message;
            }
            //
            autoClose(ao);
            //
            return bReturn;
        }

        public override bool executeStatement(string query, Hashtable paramters)
        {
            int ao = autoOpen();
            //
            bool bReturn = false;
            //        
            try
            {
                SqlCommand cmd = new SqlCommand(query, (SqlConnection)dbConn);
                // params
                foreach (String k in paramters.Keys)
                {
                    SqlParameter p = null;
                    // p.Value = DBNull.Value;
                    if (paramters[k]==null)
                        p = new SqlParameter("@" + k, DBNull.Value );
                    else
                        p = new SqlParameter("@" + k, paramters[k]);
                    //
                    cmd.Parameters.Add(p);
                }
                //
                cmd.ExecuteNonQuery();
                bReturn = true;

            }
            catch (Exception)
            { 
            }
            //
            autoClose(ao);
            //
            return bReturn;
        }

        public override long insertRetId(string tableName, Hashtable fields)
        {
            int ao = autoOpen();
            //
            long ret = -1;
            //
            if (insert(tableName, fields))
            {
                // SimpleDataSet ds = executeQuery("SELECT SCOPE_IDENTITY()");
                SimpleDataSet ds = executeQuery("SELECT  @@IDENTITY");
                DataRow r = ds.Rows[0];
                ret = Int64.Parse(r[0].ToString());
            }
            //
            autoClose(ao);
            //
            return ret;
        }

        public override long insertRetId(string sSql)
        {
            int ao = autoOpen();
            //
            long ret = -1;
            //
            if (insert(sSql))
            {
                // SimpleDataSet ds = executeQuery("SELECT SCOPE_IDENTITY()");
                SimpleDataSet ds = executeQuery("SELECT  @@IDENTITY");
                DataRow r = ds.Rows[0];
                ret = Int64.Parse(r[0].ToString());
            }
            //
            autoClose(ao);
            //
            return ret;
        }

        public override DbDataReader openCursor(string sql)
        {
            if (!this.isOpen())
                return null;
            //
            try
            {
                SqlCommand command = new SqlCommand(sql, (SqlConnection)dbConn);
                //
                SqlDataReader reader = command.ExecuteReader();
                //
                return reader;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override String toDbFormat(Object obj)
        {
            if (obj == null)
                return "NULL";
            // Byte array
            // if (obj is Byte[])
            //    return "";
            // Char
            if (obj is Char)
                return "'" + obj.ToString() + "'";
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
                return "CONVERT(datetime,'" + ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo) + "', 121)";
            // Stringa
            return "'" + obj.ToString().Replace("'", "''") + "'";
        }

 	}
}
