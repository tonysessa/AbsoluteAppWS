using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
//
using Support.db;
using Support.Web;
using Support.Library;
using System.Data.SqlClient;
using System.Data.Common;

namespace Support.Library
{
    public class DbUtil
    {
        public DbUtil() { }

        #region DataBase Function
        /// <summary>
        /// Metodo che restituisce il valore massimo della tabella per il campo passato
        /// </summary>
        /// <param name="field">campo da controllare</param>
        /// <param name="table">tabella di appartenenza</param>
        /// <param name="bNext"restituisce il velore successi se true</param>
        /// <returns>Int32 con valore massimo</returns>
        public Int32 GetMaxValueFromTable(String _field, String _table)
        {
            return GetMaxValueFromTable(_field, _table, false);
        }
        /// <summary>
        /// Metodo che restituisce il valore massimo della tabella per il campo passato
        /// </summary>
        /// <param name="field">campo da controllare</param>
        /// <param name="table">tabella di appartenenza</param>
        /// <param name="bNext"restituisce il valore successivo se true</param>
        /// <returns>Int32 con valore massimo</returns>
        public Int32 GetMaxValueFromTable(String _field, String _table, Boolean bNext)
        {
            Int32 nReturn = 0;
            String sReturn = String.Empty;
            String sSql = String.Empty;

            sSql = "SELECT MAX(" + _field + ") AS " + _field + " FROM " + _table;

            IDataProvider dp = IDataProviderFactory.factory();
            SimpleDataSet ds = null;
            DataTable rt = null;
            DataRow dr;

            ds = dp.executeQuery(sSql);
            rt = ds.Table;

            if (rt.Rows.Count > 0)
            {
                dr = rt.Rows[0];
                sReturn = dr[_field].ToString();
                if (sReturn.Length > 0)
                    nReturn = (Int32)dr[_field];
            }

            if (bNext)
                nReturn++;

            return nReturn;
        }
        /// <summary>
        /// Metodo che restituisce il valore massimo della tabella per il campo passato
        /// </summary>
        /// <param name="_field">campo da controllare</param>
        /// <param name="_table">tabella di appartenenza</param>
        /// <param name="_fieldSearch">campo di ricerca</param>
        /// <param name="_fieldSearchValue">valore di ricerca</param>
        /// <returns>Int32 con valore massimo</returns>
        public Int32 GetMaxValueFromTable(String _field, String _table, String _fieldSearch, String _fieldSearchValue)
        {
            return GetMaxValueFromTable(_field, _table, _fieldSearch, _fieldSearchValue, false);
        }
        /// <summary>
        /// Metodo che restituisce il valore massimo della tabella per il campo passato
        /// </summary>
        /// <param name="_field">campo da controllare</param>
        /// <param name="_table">tabella di appartenenza</param>
        /// <param name="_fieldSearch">campo di ricerca</param>
        /// <param name="_fieldSearchValue">valore di ricerca</param>
        /// <param name="bNext"restituisce il velore successi se true</param>
        /// <returns>Int32 con valore massimo</returns>
        public Int32 GetMaxValueFromTable(String _field, String _table, String _fieldSearch, String _fieldSearchValue, Boolean bNext)
        {
            Int32 nReturn = 0;
            String sReturn = String.Empty;
            String sSql = String.Empty;

            sSql = "SELECT MAX(" + _field + ") AS " + _field + " FROM " + _table;
            if (_fieldSearch.Length > 0 && _fieldSearchValue.Length > 0)
                sSql += " WHERE " + _fieldSearch + "=" + _fieldSearchValue;


            IDataProvider dp = IDataProviderFactory.factory();
            SimpleDataSet ds = null;
            DataTable rt = null;
            DataRow dr;

            ds = dp.executeQuery(sSql);
            rt = ds.Table;

            if (rt.Rows.Count > 0)
            {
                dr = rt.Rows[0];
                sReturn = dr[_field].ToString();
                if (sReturn.Length > 0)
                    nReturn = (Int32)dr[_field];
            }

            if (bNext)
                nReturn++;

            return nReturn;
        }
        /// <summary>
        /// Metodo che restituisce il valore del campo richiesto
        /// </summary>
        /// <param name="_table">tabella di appartenenza</param>
        /// <param name="_field">campo richiesto</param>
        /// <param name="_fieldSearch">campo di ricerca</param>
        /// <param name="_fieldSearchValue">valore di ricerca</param>
        /// <returns>restituisce il primo DataRow trovato</returns>
        public String GetFieldFromTable(String _table, String _field, String _fieldSearch, String _fieldSearchValue)
        {
            String sSql = String.Empty;
            String sReturn = String.Empty;

            sSql = "SELECT " + _field + " FROM " + _table;

            //if ((_fieldSearch.Length > 0) && (_fieldSearchValue.Length > 0))
            //    sSql += " WHERE " + _fieldSearch + " = " + _fieldSearchValue;

            if (_fieldSearch.Length > 0)
                sSql += " WHERE " + _fieldSearch;
            if (_fieldSearchValue.Length > 0)
                sSql += " = " + _fieldSearchValue;

            IDataProvider dp = IDataProviderFactory.factory();
            SimpleDataSet ds = null;
            DataTable rt = null;
            DataRow dr;

            ds = dp.executeQuery(sSql);
            rt = ds.Table;

            if (rt.Rows.Count > 0)
            {
                dr = rt.Rows[0];
                sReturn = dr[_field].ToString();
            }

            return sReturn;
        }
        /// <summary>
        /// Metodo che restituisce un DataRow per i filtri passati
        /// </summary>
        /// <param name="_table">tabella di appartenenza</param>
        /// <param name="_fieldSearch">campo di ricerca</param>
        /// <param name="_fieldSearchValue">valore di ricerca</param>
        /// <returns>restituisce il primo DataRow trovato</returns>
        public DataRow GetDataRowFromTable(String _table, String _fieldSearch, String _fieldSearchValue)
        {
            return GetDataRowFromTable(_table, _fieldSearch, _fieldSearchValue, "");
        }
        /// <summary>
        /// Metodo che restituisce un DataRow per i filtri e ordinamento passati
        /// </summary>
        /// <param name="_table">tabella di appartenenza</param>
        /// <param name="_fieldSearch">campo di ricerca</param>
        /// <param name="_fieldSearchValue">valore di ricerca</param>
        /// <param name="_fieldOrder">valore di ordinamento </param>
        /// <returns>restituisce il primo DataRow trovato</returns>
        public DataRow GetDataRowFromTable(String _table, String _fieldSearch, String _fieldSearchValue, String _fieldOrder)
        {
            String sSql = String.Empty;
            DataRow drRturn = null;
            sSql = "SELECT * FROM " + _table;

            if ((_fieldSearch.Length > 0) && (_fieldSearchValue.Length > 0))
                sSql += " WHERE " + _fieldSearch + " = " + _fieldSearchValue;
            else if ((_fieldSearch.Length > 0) && (_fieldSearchValue.Length.Equals(0)))
                sSql += " WHERE " + _fieldSearch;

            if (_fieldOrder.Length > 0)
                sSql += " ORDER BY " + _fieldOrder;

            IDataProvider dp = IDataProviderFactory.factory();
            SimpleDataSet ds = null;
            DataTable rt = null;

            ds = dp.executeQuery(sSql);
            rt = ds.Table;

            if (rt.Rows.Count > 0)
                drRturn = rt.Rows[0];

            return drRturn;

        }
        /// <summary>
        /// Metodo che restituisce un SimpleDataSet passando query e string di connesione
        /// </summary>
        /// <param name="_query">Query di interrogazione</param>
        /// <param name="_connectionString">string di connessione al database</param>
        /// <returns>restituisce il primo DataRow trovato</returns>
        public SimpleDataSet Execute(String _query, String _connectionString)
        {
            DataTable rt = new DataTable();
            DataSet ds = new DataSet();

            SqlConnection dbConn = new SqlConnection(_connectionString);

            dbConn.Open();
            SqlCommand cmd = new SqlCommand(_query, dbConn);
            //
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                String sErr = ex.Message;
                ds = null;
            }
            //
            dbConn.Close();
            //            

            return ds != null ? new SimpleDataSet(ds) : null;
        }
        /// <summary>
        /// Metodo che restituisce un DataTable di una tabella
        /// </summary>
        /// <param name="_table">tabella di appartenenza</param>
        /// <returns>restituisce il DataTable trovato</returns>
        public DataTable GetDataTableFromTable(String _table)
        {
            return GetDataTableFromTable(_table, "", "");
        }
        /// <summary>
        /// Metodo che restituisce un DataTable per i filtri passati
        /// </summary>
        /// <param name="_table">tabella di appartenenza</param>
        /// <param name="_fieldSearch">campo di ricerca</param>
        /// <param name="_fieldSearchValue">valore di ricerca</param>
        /// <returns>restituisce il DataTable trovato</returns>
        public DataTable GetDataTableFromTable(String _table, String _fieldSearch, String _fieldSearchValue)
        {
            String sSql = String.Empty;
            DataTable dtReturn = null;
            sSql = "SELECT * FROM " + _table;

            //if (!objLibrary.isNumber(_fieldSearchValue))
            //    _fieldSearchValue = objLibrary.sQuote(_fieldSearchValue);

            if ((_fieldSearch.Length > 0) && (_fieldSearchValue.Length > 0))
            {
                sSql += " WHERE " + _fieldSearch;
                sSql += " = " + _fieldSearchValue;
            }

            IDataProvider dp = IDataProviderFactory.factory();
            SimpleDataSet ds = null;

            ds = dp.executeQuery(sSql);
            dtReturn = ds.Table;

            return dtReturn;

        }
        /// <summary>
        /// Metodo che restituisce un DataTable per i filtri e ordinamento passati
        /// </summary>
        /// <param name="_table">tabella di appartenenza</param>
        /// <param name="_fieldSearch">campo di ricerca</param>
        /// <param name="_fieldSearchValue">valore di ricerca</param>
        /// <param name="_fieldOrder">valore di ordinamento</param>
        /// <returns>restituisce il DataTable trovato</returns>
        public DataTable GetDataTableFromTable(String _table, String _fieldSearch, String _fieldSearchValue, String _fieldOrder)
        {
            String sSql = String.Empty;
            DataTable dtReturn = null;
            sSql = "SELECT * FROM " + _table;

            if ((_fieldSearch.Length > 0) && (_fieldSearchValue.Length > 0))
            {
                sSql += " WHERE " + _fieldSearch;
                sSql += " = " + _fieldSearchValue;
            }

            if (_fieldOrder.Length > 0)
                sSql += " ORDER BY " + _fieldOrder;

            IDataProvider dp = IDataProviderFactory.factory();
            SimpleDataSet ds = null;

            ds = dp.executeQuery(sSql);
            dtReturn = ds.Table;

            return dtReturn;

        }
        #endregion
    }
}
