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
    public class LogUtil
	{
        public LogUtil() { }       

        #region Log Function
        public void Log(String filename, String str)
        {
            StreamWriter SW;
            SW = File.AppendText(filename);
            SW.WriteLine(str);
            SW.Close();
        }

        public static DateTime LogLupone(String relativeFileName, String id, DateTime? s = null)
        {
            DateTime adesso = DateTime.Now;
            try
            {
                StreamWriter SW;
               
                String txt = id + " [[" + (s != null ? adesso.Subtract(s.Value).TotalMilliseconds.ToString() : "") + " ]] ";
                SW = File.AppendText(HttpContext.Current.Server.MapPath("~/logs/" + relativeFileName));
                SW.WriteLine((s!=null ? "* " : "")+txt+" --> "+HttpContext.Current.Request.Url.OriginalString);
                SW.Close();
            }
            catch
            {
            }

            //
            return adesso;
        }

        #endregion
    }
}
