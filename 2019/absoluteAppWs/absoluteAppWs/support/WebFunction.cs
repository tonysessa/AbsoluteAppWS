using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;

//
using Support.Library;
using Support.Web;
using Support.db;
using Support.Mail;
using System.Web.UI.HtmlControls;

namespace Support.WebFunction
{
    public class WebFunction
    {
        public WebFunction() { }

        public WebContext wctx = null;


        #region objUtil
        protected Library.DateUtil objLibDate = new Library.DateUtil();
        protected Library.DbUtil objLibDB = new Library.DbUtil();
        protected Library.StringUtil objLibString = new Library.StringUtil();
        protected Library.StringSqlUtil objLibSqlString = new Library.StringSqlUtil();
        protected Library.MathUtil objLibMath = new Library.MathUtil();
        protected Library.CriptUtil objLibCript = new Library.CriptUtil();
        protected Library.LogUtil objLibLog = new Library.LogUtil();
        #endregion

        #region Costanti
        protected String sStartingPage = WebContext.getConfig("%.startingPage").ToString();
        protected String sCryptKey = WebContext.getConfig("%.cryptKey").ToString();
        public String sLogDir = WebContext.getConfig("%.LogDir").ToString();
        public String sLogFile = WebContext.getConfig("%.LogFile").ToString();
        public const string tokenFb = "EAAYGkOb6YVoBACCXkJF0ZCwj9xwvU2I89mo3C7mXNlGHYW0YUdmvMaZCvZAO2mIEnkez4Odu5vnrmmZAYLcVD42ZAE231JDAP1E5b3EtA2qLycCH8NZCssGuHUGBZBjpn2WAEAFrCnGtrbdJZCCB2IUz7hoZAUs5D5GRLnleaSAlvpgZDZD";
        public const string FB_Absolute_ID = "127242343981956";
        public const string FB_Absolute_MAGAZINE_ID = "1951015871788999";
        public string titolo = "ABSOLUTE - Campionati e Tornei Calcio a 5 e Calcio a 7 - Moncalieri, Torino e provincia";
        public string titoloBreve = "ABSOLUTE - ";
        public const string placeholderLogoTeam = "/assets/images/soccer/placeholder-squadra.png";
        public const string placeholderCompetizione = "/assets/images/soccer/placeholder-evento.jpg";
        public const string placeholderGiocatore = "/assets/images/soccer/placeholder_giocatore.jpg";
        public const string googleApiKey = "AIzaSyDKW19yRTHr4C9kgjgwjupxl-t-u1XXU5w";
        public const int numeroLiveNews = 10;
        #endregion

        #region Utility
        public String GetDataOraEstesa(DateTime _dt)
        {
            String sReturn = String.Empty;
            if (_dt != null)
            {
                String sData = objLibDate.DateToWordIta(_dt);
                String sOra = objLibDate.DateTimeToString(_dt, "GMA", "/", ":");
                sOra = sOra.Substring(11, sOra.Length - 14); //elimino data + millisecondi
                sReturn = GiornoDellaSettimanaIt(_dt) + " " + sData + " - ore " + sOra;
            }

            return sReturn;
        }
        public String GetDataEstesa(DateTime _dt)
        {
            String sReturn = String.Empty;
            if (_dt != null)
            {
                String sData = objLibDate.DateToWordIta(_dt);
                sReturn = GiornoDellaSettimanaIt(_dt) + " " + sData;
            }

            return sReturn;
        }
        public String GiornoDellaSettimanaIt(DateTime _dt)
        {
            String sReturn = String.Empty;
            if (_dt != null)
            {
                sReturn = ((int)_dt.DayOfWeek).ToString();
                if (sReturn.Equals("0"))
                    sReturn = "Domenica";
                if (sReturn.Equals("1"))
                    sReturn = "Lunedì";
                if (sReturn.Equals("2"))
                    sReturn = "Martedì";
                if (sReturn.Equals("3"))
                    sReturn = "Mercoledì";
                if (sReturn.Equals("4"))
                    sReturn = "Giovedì";
                if (sReturn.Equals("5"))
                    sReturn = "Venerdì";
                if (sReturn.Equals("6"))
                    sReturn = "Sabato";
            }

            return sReturn;
        }
        public String TroncaTesto(String _text, int _nChars)
        {
            String sReturn = String.Empty;
            String tempString = String.Empty;

            if (!String.IsNullOrEmpty(_text))
            {
                tempString = _text;
                int maxLength = _text.Length;
                if (maxLength > _nChars)
                {
                    maxLength = _nChars;
                    tempString = _text.Substring(0, maxLength);
                    int trimOffset = tempString.LastIndexOf(" ");
                    tempString = tempString.Substring(0, trimOffset) + " (&#8230;)";
                }

                sReturn = tempString;
            }


            return sReturn;
        }

        public String CheckStagioneDefault()
        {
            String sReturn = String.Empty;

            String sql = "SELECT ANNO FROM Mssql27442.STAGIONI WHERE ST_DEFAULT = '1'";
            IDataProvider dp = IDataProviderFactory.factory();
            SimpleDataSet ds = dp.executeQuery(sql);
            if (ds.Table.Rows.Count > 0)
                sReturn = ds.Table.Rows[0]["ANNO"].ToString();


            return sReturn;
        }

        #endregion

        #region MetaTags
        public void InitMetaTags(System.Web.UI.Page _HtmlPage, DataRow _dr, String _uploaddir)
        {

            if (!String.IsNullOrEmpty("" + _dr["TITOLO"]))
                titolo = "" + _dr["TITOLO"];

            _HtmlPage.Title = titolo;


            HtmlHead head = (HtmlHead)_HtmlPage.Header;
            HtmlMeta meta = new HtmlMeta();
            if (!String.IsNullOrEmpty("" + _dr["TESTO"]))
            {
                meta = new HtmlMeta();
                meta.Name = "description";
                meta.Content = "" + _dr["TESTO"];
                head.Controls.Add(meta);
            }

            #region FB
            //FB
            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:site_name");
            meta.Content = "Absolute 5";
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:url");
            meta.Content = HttpContext.Current.Request.Url.ToString();
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:title");
            meta.Content = titolo;
            head.Controls.Add(meta);

            if (!string.IsNullOrEmpty("" + _dr["TESTO"]))
            {
                meta = new HtmlMeta();
                meta.Attributes.Add("property", "og:description");
                meta.Content = "" + _dr["TESTO"];
                head.Controls.Add(meta);
            }

            if (_dr.Table.Columns.Contains("IMMAGINE"))
            {
                if (!string.IsNullOrEmpty("" + _dr["IMMAGINE"]))
                {
                    meta = new HtmlMeta();
                    meta.Attributes.Add("property", "og:image");
                    meta.Content = sStartingPage + _uploaddir + ("" + _dr["IMMAGINE"]);
                    head.Controls.Add(meta);
                }
            }

            if (_dr.Table.Columns.Contains("FOTO"))
            {
                if (!string.IsNullOrEmpty("" + _dr["FOTO"]))
                {
                    meta = new HtmlMeta();
                    meta.Attributes.Add("property", "og:image");
                    meta.Content = sStartingPage + _uploaddir + ("" + _dr["FOTO"]);
                    head.Controls.Add(meta);
                }
            }

            #endregion

        }
        public void InitMetaTagsRouting(System.Web.UI.Page _HtmlPage, DataRow _dr)
        {

            if (!String.IsNullOrEmpty("" + _dr["META_TITLE"]))
                titolo = "" + _dr["META_TITLE"];

            _HtmlPage.Title = titolo;


            HtmlHead head = (HtmlHead)_HtmlPage.Header;
            HtmlMeta meta = new HtmlMeta();
            if (!String.IsNullOrEmpty("" + _dr["META_DESCRIPTION"]))
            {
                meta = new HtmlMeta();
                meta.Name = "description";
                meta.Content = "" + _dr["META_DESCRIPTION"];
                head.Controls.Add(meta);
            }

            #region FB
            //FB
            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:site_name");
            meta.Content = "Absolute 5";
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:url");
            meta.Content = HttpContext.Current.Request.Url.ToString();
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:title");
            meta.Content = titolo;
            head.Controls.Add(meta);

            if (!string.IsNullOrEmpty("" + _dr["META_DESCRIPTION"]))
            {
                meta = new HtmlMeta();
                meta.Attributes.Add("property", "og:description");
                meta.Content = "" + _dr["META_DESCRIPTION"];
                head.Controls.Add(meta);
            }

            #endregion

        }
        public void InitMetaTagsCompetizioni(System.Web.UI.Page _HtmlPage, DataRow _dr, String _uploaddir)
        {

            if (!String.IsNullOrEmpty("" + _dr["NOME"]))
                titolo = "" + _dr["NOME"];

            _HtmlPage.Title = titoloBreve + titolo;

            #region FB
            HtmlHead head = (HtmlHead)_HtmlPage.Header;
            HtmlMeta meta = new HtmlMeta();
            //FB

            meta.Attributes.Add("property", "og:site_name");
            meta.Content = "Absolute 5";
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:url");
            meta.Content = HttpContext.Current.Request.Url.ToString();
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:title");
            meta.Content = titolo;
            head.Controls.Add(meta);


            if (!string.IsNullOrEmpty("" + _dr["FILE_COPERTINA_FRONTE"]))
            {
                Array c = _dr["IMG_COPERTINA"].ToString().Split('|');
                meta = new HtmlMeta();
                meta.Attributes.Add("property", "og:image");
                meta.Content = c.GetValue(0).ToString();
                head.Controls.Add(meta);
            }
            #endregion

        }
        public void InitMetaTagsEventi(System.Web.UI.Page _HtmlPage, DataRow _dr, string _prefix = null)
        {

            if (!String.IsNullOrEmpty("" + _dr["NOME"]))
                titolo = "" + _dr["NOME"];

            _HtmlPage.Title = titoloBreve + titolo;

            if (_prefix != null)
                _HtmlPage.Title = titoloBreve + _prefix + " " + titolo;

            #region FB
            HtmlHead head = (HtmlHead)_HtmlPage.Header;
            HtmlMeta meta = new HtmlMeta();
            //FB

            meta.Attributes.Add("property", "og:site_name");
            meta.Content = "Absolute 5";
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:url");
            meta.Content = HttpContext.Current.Request.Url.ToString();
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:title");
            meta.Content = titolo;
            head.Controls.Add(meta);

            #endregion

        }
        public void InitMetaTagsSquadre(System.Web.UI.Page _HtmlPage, DataRow _dr, String _uploaddir, string _prefix = null)
        {

            if (!String.IsNullOrEmpty("" + _dr["NOME"]))
                titolo = "" + _dr["NOME"];

            _HtmlPage.Title = titoloBreve + titolo;

            if (_prefix != null)
                _HtmlPage.Title = titoloBreve + titolo + " " +_prefix;

            #region FB
            HtmlHead head = (HtmlHead)_HtmlPage.Header;
            HtmlMeta meta = new HtmlMeta();
            //FB

            meta.Attributes.Add("property", "og:site_name");
            meta.Content = "Absolute 5";
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:url");
            meta.Content = HttpContext.Current.Request.Url.ToString();
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:title");
            meta.Content = titolo;
            head.Controls.Add(meta);

            string img = string.Empty;
            if (!string.IsNullOrEmpty("" + _dr["FOTO_SQUADRA"]))
                img = sStartingPage + _uploaddir + _dr["FOTO_SQUADRA"];
            else if (!string.IsNullOrEmpty("" + _dr["LOGO"]))
                img = sStartingPage + _uploaddir + _dr["LOGO"];
            if (!string.IsNullOrEmpty(img))
            {
                meta = new HtmlMeta();
                meta.Attributes.Add("property", "og:image");
                meta.Content = img;
                head.Controls.Add(meta);
            }

            #endregion

        }
        public void InitMetaTagsGiocatore(System.Web.UI.Page _HtmlPage, DataRow _dr, String _uploaddir, string _prefix = null)
        {

            if (!String.IsNullOrEmpty("" + _dr["NOME"]))
                titolo = string.Format("{0} {1}", "" + _dr["NOME"], "" + _dr["COGNOME"]);

            _HtmlPage.Title = titoloBreve + titolo;

            if (_prefix != null)
                _HtmlPage.Title = titoloBreve + titolo + " " + _prefix;

            #region FB
            HtmlHead head = (HtmlHead)_HtmlPage.Header;
            HtmlMeta meta = new HtmlMeta();
            //FB

            meta.Attributes.Add("property", "og:site_name");
            meta.Content = "Absolute 5";
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:url");
            meta.Content = HttpContext.Current.Request.Url.ToString();
            head.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:title");
            meta.Content = titolo;
            head.Controls.Add(meta);
            

            #endregion

        }
        #endregion



    }
}







