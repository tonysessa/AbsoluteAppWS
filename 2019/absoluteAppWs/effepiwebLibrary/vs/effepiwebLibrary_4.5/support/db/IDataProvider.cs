using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;
using System.Configuration;
using Support.Web;

namespace Support.db
{
    /// <summary>
    /// Struttura usata per passare i parametri delle query all'oggetto DataProvider
    /// </summary>
    /// <paramref name=param_value>Stringa contenente i valori da inserire nella query</paramref>
    /// <paramref name=param_value>Stringa contenente i nomi dei valori da sostituire nella query</paramref>
    
    /*
    public struct QueryParams
    {
        public string param_value;
        public string param_name;
    }
    */

    public class SimpleDataSet
    {
        public SimpleDataSet(DataSet _ds)
        {
            ds = _ds;
        }
        //
        public DataRowCollection Rows
        {
            get { return ds.Tables[0].Rows; }
        }
        //
        public DataTable Table
        {
            get { return ds.Tables[0]; }
        }
        //
        public DataSet ds = null;
        // public DataRowCollection Rows = null;
    }

    public class BlockDataSet : SimpleDataSet
    {
        public BlockDataSet(DataSet _ds) : base(_ds)
        {
        }
        //
        public int blockSize    = 10;
        public int currentBlock = 0;
        public bool hasPrev     = false;
        public bool hasNext     = false;
    }

    public interface IDataProvider
    {
          String getConnectionString();

          DbConnection createDbConnection( String connString );

          bool open();

          void close();

          bool isOpen();

          SimpleDataSet executeQuery(string query);

          SimpleDataSet executeQuery(string query, int maxRows);

          BlockDataSet executeQuery(string query, int blockSize, int currentBlock);
            
          bool executeStatement(string query);

          bool executeStatement(string query, Hashtable paramters);

          bool insert(string tableName, Hashtable fields);

          bool insert(string sSql);

          long insertRetId(string tableName, Hashtable fields);

          long insertRetId(string sSql);

          bool update(string tableName, Hashtable fields, string keyExpr);

          bool delete(string tableName, string keyExpr);


        /**
         * I tipi che gestiamo sono:
         * String
         * int16, int32, int64, uint16, uint32, uint64
         * DateTime
         * */
        String toDbFormat(Object obj);

        /**
         * Reader
         * Richiede la connessione aperta
         * */
        DbDataReader openCursor( string sql );
    }

    public class IDataProviderFactory
    {
        public static IDataProvider factory(String dataProviderName)
        {
            try
            {
                if (dataProviderName.Equals("OleDbProvider"))
                    return new OleDbProvider();
                if (dataProviderName.Equals("DataProviderSqlServer"))
                    return new DataProviderSqlServer();
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static IDataProvider factory()
        {
            return factory(WebContext.getConfig("%.dataProvider"));
        }

    }
}

