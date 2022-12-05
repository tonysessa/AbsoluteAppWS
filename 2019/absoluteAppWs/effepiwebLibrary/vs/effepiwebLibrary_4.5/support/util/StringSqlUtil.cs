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

namespace Support.Library
{
    public class StringSqlUtil
	{
        public StringSqlUtil() { }

        #region StringSql Function
        /// <summary>
        /// Metodo che crea la parte where di una query
        /// </summary>
        /// <param name="where">stringa where</param>
        /// <param name="arg">stringa con nome del campo</param>
        /// <param name="val">stringa con valore del campo</param>
        /// <param name="quote">bool per quote</param>
        /// <param name="exact">bool per valore esatto</param>
        /// <returns>String con numero del giorno in lettere</returns>
        public String MakeWhere(String where, String arg, String val, bool quote, bool exact)
        {
            String ret = String.Empty;
            String val1 = String.Empty;
            StringUtil objStringUtil = new StringUtil();

            ret = where;
            val = val.Trim();
            val1 = val;

            if (!exact)
                val1 = val1 + "%";

            if (val.Length > 0)
            {
                if (ret.Length > 0)
                    ret = ret + " AND ";

                if (quote)
                    val1 = "'" + objStringUtil.sAquote(val1) + "'";

                if (exact)
                    ret = ret + arg + " = " + val1;
                else
                    ret = ret + arg + " LIKE(" + val1 + ")";

            }
            return ret;
        }

        /// <summary>
        /// Metodo che crea la parte where di una query
        /// </summary>
        /// <param name="where">stringa where</param>
        /// <param name="arg">stringa con nome del campo</param>
        /// <param name="val">stringa con valore del campo</param>
        /// <param name="quote">bool per quote</param>
        /// <param name="exact">bool per valore esatto</param>
        /// <returns>String con numero del giorno in lettere</returns>
        public String MakeWhere(String where, String arg, String val, bool quote, String operatore)
        {
            String ret = String.Empty;
            String val1 = String.Empty;
            StringUtil objStringUtil = new StringUtil();

            ret = where;
            val = val.Trim();
            val1 = val;

            if (val.Length > 0)
            {
                if (ret.Length > 0)
                    ret = ret + " AND ";

                if (quote)
                    val1 = "'" + objStringUtil.sAquote(val1) + "'";

                 ret = ret + arg + " " + operatore + " " + val1;
            }
            return ret;
        }        

        /// <summary>
        /// Metodo che crea la parte where di una query
        /// </summary>
        /// <param name="where">stringa where</param>
        /// <param name="arg">stringa con nome del campo</param>
        /// <param name="val">stringa con valore del campo</param>
        /// <param name="pre">bool per prefisso in like</param>
        /// <param name="post">bool per suffisso in like</param>
        /// <returns>String con numero del giorno in lettere</returns>
        public String MakeWhereLike(String where, String arg, String val, bool pre, bool post)
        {
            String ret = String.Empty;
            String val1 = String.Empty;
            StringUtil objStringUtil = new StringUtil();

            ret = where;
            val = val.Trim();
            val1 = val;

            if (pre)
                val1 = "%" + val1;

            if (post)
                val1 = val1 + "%" ;

            if (val.Length > 0)
            {
                if (ret.Length > 0)
                    ret = ret + " AND ";

                val1 = "'" + objStringUtil.sAquote(val1) + "'";
                 ret = ret + arg + " LIKE(" + val1 + ")";

            }
            return ret;
        }

        /// <summary>
        /// Metodo che crea la parte where di una query in OR
        /// </summary>
        /// <param name="where">stringa where</param>
        /// <param name="arg">stringa con nome del campo</param>
        /// <param name="val">stringa con valore del campo</param>
        /// <param name="pre">bool per prefisso in like</param>
        /// <param name="post">bool per suffisso in like</param>
        /// <returns>String con numero del giorno in lettere</returns>
        public String MakeWhereLikeOR(String where, String arg, String val, bool pre, bool post)
        {
            String ret = String.Empty;
            String val1 = String.Empty;
            StringUtil objStringUtil = new StringUtil();

            ret = where;
            val = val.Trim();
            val1 = val;

            if (pre)
                val1 = "%" + val1;

            if (post)
                val1 = val1 + "%";

            if (val.Length > 0)
            {
                if (ret.Length > 0)
                    ret = ret + " OR ";

                val1 = "'" + objStringUtil.sAquote(val1) + "'";
                ret = ret + arg + " LIKE(" + val1 + ")";

            }
            return ret;
        }

        /// <summary>
        /// Metodo che restituisce la data in formato stringa per il database
        /// </summary>
        /// <param name="_data">data</param>
        /// <param name="_format">formato della data passata</param>
        /// <param name="_separator">simbolo di separazione della data passata</param>
        /// <returns>String con numero del giorno in lettere</returns>
        public String StringDate2DBStr(String _data, String _format, String _separator)
        {
            String sDateReturn = String.Empty;
            char[] chSeparator = _separator.ToCharArray(0, 1);
            String[] myDateArray = _data.Split(chSeparator[0]);

            if (_format == "GMA")
                sDateReturn = myDateArray[2] + "-" + myDateArray[1] + "-" + myDateArray[0];
            else if (_format == "MGA")
                sDateReturn = myDateArray[2] + "-" + myDateArray[0] + "-" + myDateArray[1];
            else if (_format == "MAG")
                sDateReturn = myDateArray[1] + "-" + myDateArray[0] + "-" + myDateArray[2];
            else if (_format == "AMG")
                sDateReturn = myDateArray[0] + "-" + myDateArray[1] + "-" + myDateArray[2];
            else if (_format == "AGM")
                sDateReturn = myDateArray[0] + "-" + myDateArray[2] + "-" + myDateArray[1];

            return sDateReturn;
        }

        /// <summary>
        /// Metodo che restituisce la data in formato stringa per il database
        /// </summary>
        /// <param name="_data">data</param>
        /// <param name="_format">formato della data passata</param>
        /// <param name="_dateSeparator">simbolo di separazione della data passata</param>
        /// <param name="_timeSeparator">simbolo di separazione della data passata</param>
        /// <returns>String con numero del giorno in lettere</returns>
        public String StringDateTime2DBStr(String _data, String _format, String _dateSeparator, String _timeSeparator)
        {

            String sDateTimeSeparator = " ";
            char[] chSeparator = sDateTimeSeparator.ToCharArray(0, 1);
            String[] myDateTimeArray = _data.Split(chSeparator[0]);

            String sDateReturn = myDateTimeArray[0];
            String sTimeReturn = myDateTimeArray[1];

            char[] chDateSeparator = _dateSeparator.ToCharArray(0, 1);
            String[] myDateArray = sDateReturn.Split(chDateSeparator[0]);

            char[] chTimeSeparator = _timeSeparator.ToCharArray(0, 1);
            String[] myTimeArray = sTimeReturn.Split(chTimeSeparator[0]);

            if (_format == "GMA")
                sDateReturn = myDateArray[2] + "-" + myDateArray[1] + "-" + myDateArray[0] + " " + myTimeArray[0] + ":" + myTimeArray[1] + ":" + myTimeArray[2];
            else if (_format == "MGA")
                sDateReturn = myDateArray[2] + "-" + myDateArray[0] + "-" + myDateArray[1] + " " + myTimeArray[0] + ":" + myTimeArray[1] + ":" + myTimeArray[2];
            else if (_format == "MAG")
                sDateReturn = myDateArray[1] + "-" + myDateArray[0] + "-" + myDateArray[2] + " " + myTimeArray[0] + ":" + myTimeArray[1] + ":" + myTimeArray[2];
            else if (_format == "AMG")
                sDateReturn = myDateArray[0] + "-" + myDateArray[1] + "-" + myDateArray[2] + " " + myTimeArray[0] + ":" + myTimeArray[1] + ":" + myTimeArray[2];
            else if (_format == "AGM")
                sDateReturn = myDateArray[0] + "-" + myDateArray[2] + "-" + myDateArray[1] + " " + myTimeArray[0] + ":" + myTimeArray[1] + ":" + myTimeArray[2];

            return sDateReturn;
        }        
        #endregion
    }
}
