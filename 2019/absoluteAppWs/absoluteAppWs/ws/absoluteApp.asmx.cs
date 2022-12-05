using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
//

using Support.Web;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Support.WebFunction;
using System.Web.Script.Services;
using System.Xml;
using System.Xml.Linq;
using Support.db;
using System.Data;

namespace absoluteAppWs.ws
{
    /// <summary>
    /// Summary description for absoluteApp
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class absoluteApp : System.Web.Services.WebService
    {
        #region objUtil
        protected WebContext wctx = null;
        protected Support.Library.DateUtil objLibDate = new Support.Library.DateUtil();
        protected Support.Library.DbUtil objLibDB = new Support.Library.DbUtil();
        protected Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
        protected Support.Library.StringSqlUtil objLibSqlString = new Support.Library.StringSqlUtil();
        protected Support.Library.MathUtil objLibMath = new Support.Library.MathUtil();
        protected Support.Library.CriptUtil objLibCript = new Support.Library.CriptUtil();
        protected Support.Library.LogUtil objLibLog = new Support.Library.LogUtil();
        //
        protected WebFunction objWebFunction = new WebFunction();
        #endregion

        #region Parametri Sito
        public String sCryptKey = WebContext.getConfig("%.cryptKey").ToString();
        public String sStartingpage = WebContext.getConfig("%.startingpage").ToString();
        public String sLogDir = WebContext.getConfig("%.LogDir").ToString();
        public String sLogFile = WebContext.getConfig("%.LogFile").ToString();
        public String uploadDirBanner = WebContext.getConfig("%.uploadDirBanner").ToString();
        public String uploadDirNews = WebContext.getConfig("%.uploadDirNews").ToString();
        #endregion



        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode getToken()
        {
            XElement element = new XElement("token");
            try
            {
                element.Add(new XElement("stato_response", new XCData("1")));
                string str = this.objLibCript.uEncode(this.sCryptKey, this.objLibCript.getCryptKey(this.sCryptKey));
                element.Add(new XElement("id", new XCData(str)));
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }

        public static XElement GetXElement(XmlNode node)
        {
            XDocument document = new XDocument();
            using (XmlWriter writer = document.CreateWriter())
            {
                node.WriteTo(writer);
            }
            return document.Root;
        }

        public static XmlNode GetXmlNode(XElement element)
        {
            using (XmlReader reader = element.CreateReader())
            {
                XmlDocument document1 = new XmlDocument();
                document1.Load(reader);
                return document1;
            }
        }





        [WebMethod(Description = "Dettaglio Evento."), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode dettaglioEvento(string _token, string _idsquadra, string _idEvento)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            string str2 = _idsquadra;
            string str3 = _idEvento;
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else if (string.IsNullOrEmpty(str2) || string.IsNullOrEmpty(str3))
                {
                    element.Add(new XElement("stato_response", new XCData("2")));
                    element.Add(new XElement("msg", new XCData("param mancanti")));
                }
                else
                {
                    string query = string.Empty;
                    string str5 = string.Empty;
                    string str6 = this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str2));
                    DataRow row = this.objLibDB.GetDataRowFromTable("EVENTI", "ID", this.objLibString.sQuote(str3));
                    if (row == null)
                    {
                        element.Add(new XElement("stato_response", new XCData("2")));
                        element.Add(new XElement("msg", new XCData("Dettaglio evento non disponibile")));
                    }
                    else
                    {
                        element.Add(new XElement("stato_response", new XCData("1")));
                        string str7 = "" + row["TIPO_GIRONE"].ToString();
                        string str8 = "" + row["ORDINE_CLASSIFICA"].ToString();
                        string str9 = "" + row["ID_COMPETIZIONE"].ToString();
                        string str10 = this.objLibDB.GetFieldFromTable("COMPETIZIONI", "NOME", "ID", this.objLibString.sQuote(str9));
                        element.Add(new XElement("id_squadra", new XCData(str2)));
                        element.Add(new XElement("nome_squadra", new XCData(str6)));
                        element.Add(new XElement("id_competizione", new XCData(str9)));
                        element.Add(new XElement("nome_competizione", new XCData(str10)));
                        element.Add(new XElement("id_evento", new XCData(str3)));
                        element.Add(new XElement("nome_evento", new XCData(row["NOME"].ToString())));
                        element.Add(new XElement("tipo_evento", new XCData(str7)));
                        element.Add(new XElement("ordine_classifica", new XCData(str8)));
                        if (!string.IsNullOrEmpty(str7))
                        {
                            if (!str7.Equals("0") && !str7.Equals("1"))
                            {
                                if (str7.Equals("2"))
                                {
                                    string[] textArray4 = new string[] { "SELECT * FROM APP_INCONTRI_V WHERE (ID_SQUADRA_1=", this.objLibString.sQuote(str2), " OR ID_SQUADRA_2=", this.objLibString.sQuote(str2), ")" };
                                    IDataProvider provider2 = IDataProviderFactory.factory();
                                    SimpleDataSet set2 = provider2.executeQuery(string.Concat(textArray4) + " AND ID_EVENTO=" + this.objLibString.sQuote(str3) + " ORDER BY DATA_POSTICIPO ASC, DATA_EVENTO ASC");
                                    XElement content = new XElement("incontri");
                                    XAttribute attribute = null;
                                    if (set2.Table.Rows.Count <= 0)
                                    {
                                        attribute = new XAttribute("num", "0");
                                    }
                                    else
                                    {
                                        attribute = new XAttribute("num", set2.Table.Rows.Count.ToString());
                                        for (int i = 0; i < set2.Table.Rows.Count; i++)
                                        {
                                            DataRow row4 = set2.Table.Rows[i];
                                            string str35 = row4["ID_SQUADRA_1"].ToString();
                                            string str36 = row4["ID_SQUADRA_2"].ToString();
                                            string str37 = row4["ID_INCONTRO"].ToString();
                                            row4["ID_CALENDARIO"].ToString();
                                            string str38 = row4["NOME_CALENDARIO"].ToString();
                                            DateTime d = (DateTime)row4["DATA_EVENTO"];
                                            DateTime minValue = DateTime.MinValue;
                                            if (!string.IsNullOrEmpty(row4["DATA_POSTICIPO"].ToString()))
                                            {
                                                minValue = (DateTime)row4["DATA_POSTICIPO"];
                                            }
                                            string str39 = row4["ORA"].ToString();
                                            if (!string.IsNullOrEmpty(row4["ORA_POSTICIPO"].ToString()))
                                            {
                                                str39 = row4["ORA_POSTICIPO"].ToString();
                                            }
                                            string str40 = row4["ID_CAMPO"].ToString();
                                            if (!row4["ID_CAMPO_RECUPERO"].ToString().Equals("0") && !string.IsNullOrEmpty(row4["ID_CAMPO_RECUPERO"].ToString()))
                                            {
                                                str40 = row4["ID_CAMPO_RECUPERO"].ToString();
                                            }
                                            XElement element59 = new XElement("incontro");
                                            XElement element60 = new XElement("id", new XCData(str37));
                                            XElement element61 = new XElement("id_squadra1", new XCData(str35));
                                            XElement element62 = new XElement("id_squadra2", new XCData(str36));
                                            string str41 = string.Empty;
                                            string str42 = string.Empty;
                                            str41 = !str35.Equals("0") ? this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str35)) : "Vincente";
                                            str42 = !str36.Equals("0") ? this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str36)) : "Vincente";
                                            XElement element63 = new XElement("squadra1", new XCData(str41));
                                            XElement element64 = new XElement("squadra2", new XCData(str42));
                                            string str43 = this.objLibDB.GetFieldFromTable("CAMPI", "NOMECAMPO", "ID", this.objLibString.sQuote(str40));
                                            XElement element65 = new XElement("nomecampo", new XCData(str43));
                                            XElement element66 = null;
                                            if (minValue > d)
                                            {
                                                d = minValue;
                                            }
                                            element66 = new XElement("data", new XCData(this.objLibDate.DateToString(d, "GMA", "/")));
                                            XElement element67 = new XElement("ora", new XCData(str39));
                                            XElement element68 = new XElement("id_campo", new XCData(str40));
                                            XElement element69 = new XElement("nome_calendario", new XCData(str38));
                                            XElement element70 = null;
                                            XElement element71 = null;
                                            XElement element72 = null;
                                            string str44 = "SELECT * FROM LS_INCONTRI WHERE ID_INCONTRO=" + this.objLibString.sQuote(str37) + " AND ABILITATO = 1";
                                            IDataProvider provider1 = IDataProviderFactory.factory();
                                            SimpleDataSet set3 = provider1.executeQuery(str44);
                                            if (set3.Table.Rows.Count <= 0)
                                            {
                                                element72 = new XElement("flLive", new XCData("0"));
                                                element70 = new XElement("risultato_1", new XCData("-"));
                                                element71 = new XElement("risultato_2", new XCData("-"));
                                            }
                                            else
                                            {
                                                string str45 = set3.Table.Rows[0]["PUNTEGGIO_1"].ToString();
                                                string str46 = set3.Table.Rows[0]["PUNTEGGIO_2"].ToString();
                                                if (set3.Table.Rows[0]["FL_LIVE"].ToString().Equals("1"))
                                                {
                                                    element72 = new XElement("flLive", new XCData("1"));
                                                    element70 = string.IsNullOrEmpty(str45) ? new XElement("risultato_1", new XCData("0")) : new XElement("risultato_1", new XCData(str45));
                                                    element71 = string.IsNullOrEmpty(str46) ? new XElement("risultato_2", new XCData("0")) : new XElement("risultato_2", new XCData(str46));
                                                }
                                                else
                                                {
                                                    element72 = new XElement("flLive", new XCData("0"));
                                                    element70 = new XElement("risultato_1", new XCData("-"));
                                                    element71 = new XElement("risultato_2", new XCData("-"));
                                                }
                                            }
                                            provider1.close();
                                            query = "SELECT * FROM INCONTRI_RISULTATI_V WHERE ID=" + this.objLibString.sQuote(str37) + " AND PUNTEGGIO_1 IS NOT NULL AND PUNTEGGIO_2 IS NOT NULL";
                                            IDataProvider provider3 = IDataProviderFactory.factory();
                                            SimpleDataSet set4 = provider3.executeQuery(query);
                                            if (set4.Table.Rows.Count > 0)
                                            {
                                                element70 = new XElement("risultato_1", new XCData(set4.Table.Rows[0]["PUNTEGGIO_1"].ToString()));
                                                element71 = new XElement("risultato_2", new XCData(set4.Table.Rows[0]["PUNTEGGIO_2"].ToString()));
                                            }
                                            provider3.close();
                                            element59.Add(element60);
                                            element59.Add(element61);
                                            element59.Add(element62);
                                            element59.Add(element63);
                                            element59.Add(element64);
                                            element59.Add(element66);
                                            element59.Add(element67);
                                            element59.Add(element68);
                                            element59.Add(element65);
                                            element59.Add(element70);
                                            element59.Add(element71);
                                            element59.Add(element69);
                                            element59.Add(element72);
                                            content.Add(element59);
                                        }
                                    }
                                    provider2.close();
                                    content.Add(attribute);
                                    element.Add(content);
                                }
                            }
                            else
                            {
                                str5 = !str8.Equals("1") ? " ORDER BY PUNTI DESC, (GF-GS) DESC, NOME ASC" : " ORDER BY ORDINE ASC, PUNTI DESC, (GF-GS) DESC, NOME ASC";
                                string[] textArray1 = new string[] { "SELECT * FROM CLASSIFICHE_SQUADRE_V WHERE ID_EVENTO=", this.objLibString.sQuote(str3), " AND AZZERATA=0 AND ID_SQUADRA IN (SELECT DISTINCT(ID_SQUADRA) FROM EVENTI_SQUADRE WHERE ID_EVENTO=", this.objLibString.sQuote(str3), " AND STATO=1)" };
                                query = string.Concat(textArray1) + str5;
                                XElement content = new XElement("classifica");
                                XElement element12 = null;
                                if (str7.Equals("1"))
                                {
                                    element12 = new XElement("classifica_titolo", new XCData("Classifica Generale"));
                                }
                                else if (str7.Equals("0"))
                                {
                                    element12 = new XElement("classifica_titolo", new XCData("Classifica"));
                                }
                                content.Add(element12);
                                XElement element13 = new XElement("squadre");
                                IDataProvider provider = IDataProviderFactory.factory();
                                SimpleDataSet set = provider.executeQuery(query);
                                int num = 0;
                                while (true)
                                {
                                    if (num >= set.Table.Rows.Count)
                                    {
                                        string[] textArray2 = new string[] { "SELECT ID_SQUADRA FROM EVENTI_SQUADRE WHERE ID_EVENTO=", this.objLibString.sQuote(str3), " AND STATO=1 AND ID_SQUADRA NOT IN (SELECT ID_SQUADRA FROM CLASSIFICHE WHERE ID_EVENTO=", this.objLibString.sQuote(str3), " AND AZZERATA = 0)" };
                                        set = provider.executeQuery(string.Concat(textArray2));
                                        int num3 = 0;
                                        while (true)
                                        {
                                            if (num3 >= set.Table.Rows.Count)
                                            {
                                                content.Add(element13);
                                                element.Add(content);
                                                if (str7.Equals("1"))
                                                {
                                                    provider = IDataProviderFactory.factory();
                                                    set = provider.executeQuery("SELECT * FROM CLASSIFICHE_SQUADRE_V WHERE ID_EVENTO=" + this.objLibString.sQuote(str3) + " AND AZZERATA=1 " + str5);
                                                    if (set.Table.Rows.Count > 0)
                                                    {
                                                        content = new XElement("classifica");
                                                        content.Add(new XElement("classifica_titolo", new XCData("Classifica Clausura")));
                                                        element13 = new XElement("squadre");
                                                        int num4 = 0;
                                                        while (true)
                                                        {
                                                            if (num4 >= set.Table.Rows.Count)
                                                            {
                                                                string[] textArray3 = new string[] { "SELECT ID_SQUADRA FROM EVENTI_SQUADRE WHERE ID_EVENTO=", this.objLibString.sQuote(str3), " AND STATO=1 AND ID_SQUADRA NOT IN (SELECT ID_SQUADRA FROM CLASSIFICHE WHERE ID_EVENTO=", this.objLibString.sQuote(str3), " AND AZZERATA = 0)" };
                                                                set = provider.executeQuery(string.Concat(textArray3));
                                                                int num6 = 0;
                                                                while (true)
                                                                {
                                                                    if (num6 >= set.Table.Rows.Count)
                                                                    {
                                                                        content.Add(element13);
                                                                        element.Add(content);
                                                                        break;
                                                                    }
                                                                    DataRow row3 = set.Table.Rows[num6];
                                                                    XElement element47 = new XElement("squadra");
                                                                    string str33 = this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(row3["ID_SQUADRA"].ToString()));
                                                                    string str34 = ""+row3["ID_SQUADRA"].ToString();
                                                                    XElement element48 = new XElement("id_squadra", new XCData(str34));
                                                                    XElement element49 = new XElement("nome_squadra", new XCData(str33));
                                                                    XElement element50 = new XElement("pti", new XCData("0"));
                                                                    XElement element51 = new XElement("g", new XCData("0"));
                                                                    XElement element52 = new XElement("v", new XCData("0"));
                                                                    XElement element53 = new XElement("n", new XCData("0"));
                                                                    XElement element54 = new XElement("p", new XCData("0"));
                                                                    XElement element55 = new XElement("gf", new XCData("0"));
                                                                    XElement element56 = new XElement("gs", new XCData("0"));
                                                                    XElement element57 = new XElement("dr", new XCData("0"));
                                                                    element47.Add(element48);
                                                                    element47.Add(element49);
                                                                    element47.Add(element50);
                                                                    element47.Add(element51);
                                                                    element47.Add(element52);
                                                                    element47.Add(element53);
                                                                    element47.Add(element54);
                                                                    element47.Add(element55);
                                                                    element47.Add(element56);
                                                                    element47.Add(element57);
                                                                    element13.Add(element47);
                                                                    num6++;
                                                                }
                                                                break;
                                                            }
                                                            XElement element36 = new XElement("squadra");
                                                            DataRow row5 = set.Table.Rows[num4];
                                                            string str23 = "" + row5["ID_SQUADRA"];
                                                            string str24 = "" + row5["NOME"];
                                                            string str25 = "" + row5["PUNTI_PENALITA"];
                                                            string str26 = "" + row5["PUNTI"];
                                                            string str27 = "" + row5["GIOCATE"];
                                                            string str28 = "" + row5["VINTE"];
                                                            string str29 = "" + row5["PAREGGIATE"];
                                                            string str30 = "" + row5["PERSE"];
                                                            string str31 = "" + row5["GF"];
                                                            string str32 = "" + row5["GS"];
                                                            if (!string.IsNullOrEmpty(str25) && (Convert.ToInt32(str25) > 0))
                                                            {
                                                                str24 = str24 + "* penalit&agrave; - " + str25 + " punti";
                                                            }
                                                            if (string.IsNullOrEmpty(str31))
                                                            {
                                                                str31 = "0";
                                                            }
                                                            if (string.IsNullOrEmpty(str32))
                                                            {
                                                                str32 = "0";
                                                            }
                                                            int num5 = Convert.ToInt32(str31) - Convert.ToInt32(str32);
                                                            element36.Add(new XElement("id_squadra", new XCData(str23)));
                                                            element36.Add(new XElement("nome_squadra", new XCData(str24)));
                                                            element36.Add(new XElement("pti", new XCData(str26)));
                                                            element36.Add(new XElement("g", new XCData(str27)));
                                                            element36.Add(new XElement("v", new XCData(str28)));
                                                            element36.Add(new XElement("n", new XCData(str29)));
                                                            element36.Add(new XElement("p", new XCData(str30)));
                                                            element36.Add(new XElement("gf", new XCData(str31)));
                                                            element36.Add(new XElement("gs", new XCData(str32)));
                                                            element36.Add(new XElement("dr", new XCData(num5.ToString())));
                                                            element13.Add(element36);
                                                            num4++;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                            DataRow row2 = set.Table.Rows[num3];
                                            XElement element25 = new XElement("squadra");
                                            string str21 = this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(row2["ID_SQUADRA"].ToString()));
                                            string str22 = "" + row2["ID_SQUADRA"];
                                            XElement element26 = new XElement("nome_squadra", new XCData(str21));
                                            XElement element27 = new XElement("id_squadra", new XCData(str22));
                                            XElement element28 = new XElement("pti", new XCData("0"));
                                            XElement element29 = new XElement("g", new XCData("0"));
                                            XElement element30 = new XElement("v", new XCData("0"));
                                            XElement element31 = new XElement("n", new XCData("0"));
                                            XElement element32 = new XElement("p", new XCData("0"));
                                            XElement element33 = new XElement("gf", new XCData("0"));
                                            XElement element34 = new XElement("gs", new XCData("0"));
                                            XElement element35 = new XElement("dr", new XCData("0"));
                                            element25.Add(element27);
                                            element25.Add(element26);
                                            element25.Add(element28);
                                            element25.Add(element29);
                                            element25.Add(element30);
                                            element25.Add(element31);
                                            element25.Add(element32);
                                            element25.Add(element33);
                                            element25.Add(element34);
                                            element25.Add(element35);
                                            element13.Add(element25);
                                            num3++;
                                        }
                                        break;
                                    }
                                    XElement element14 = new XElement("squadra");
                                    DataRow row1 = set.Table.Rows[num];
                                    string str11 = "" + row1["ID_SQUADRA"];
                                    string str12 = "" + row1["NOME"];
                                    string str13 = "" + row1["PUNTI_PENALITA"];
                                    string str14 = "" + row1["PUNTI"];
                                    string str15 = "" + row1["GIOCATE"];
                                    string str16 = "" + row1["VINTE"];
                                    string str17 = "" + row1["PAREGGIATE"];
                                    string str18 = "" + row1["PERSE"];
                                    string str19 = "" + row1["GF"];
                                    string str20 = "" + row1["GS"];
                                    if (!string.IsNullOrEmpty(str13) && (Convert.ToInt32(str13) > 0))
                                    {
                                        str12 = str12 + "* penalit&agrave; - " + str13 + " punti";
                                    }
                                    if (string.IsNullOrEmpty(str19))
                                    {
                                        str19 = "0";
                                    }
                                    if (string.IsNullOrEmpty(str20))
                                    {
                                        str20 = "0";
                                    }
                                    int num2 = Convert.ToInt32(str19) - Convert.ToInt32(str20);
                                    element14.Add(new XElement("id_squadra", new XCData(str11)));
                                    element14.Add(new XElement("nome_squadra", new XCData(str12)));
                                    element14.Add(new XElement("pti", new XCData(str14)));
                                    element14.Add(new XElement("g", new XCData(str15)));
                                    element14.Add(new XElement("v", new XCData(str18)));
                                    element14.Add(new XElement("n", new XCData(str16)));
                                    element14.Add(new XElement("p", new XCData(str17)));
                                    element14.Add(new XElement("gf", new XCData(str19)));
                                    element14.Add(new XElement("gs", new XCData(str20)));
                                    element14.Add(new XElement("dr", new XCData(num2.ToString())));
                                    element13.Add(element14);
                                    num++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }

        [WebMethod(Description = "Dettaglio Gara"), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode dettaglioGara(string _token, string _idincontro)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            string str2 = _idincontro;
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else if (string.IsNullOrEmpty(str2))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Parametro non valido")));
                }
                else
                {
                    DataRow row = this.objLibDB.GetDataRowFromTable("APP_INCONTRI_V", "ID_INCONTRO", this.objLibString.sQuote(str2));
                    if (row == null)
                    {
                        element.Add(new XElement("stato_response", new XCData("2")));
                        element.Add(new XElement("msg", new XCData("Dettaglio incontro non disponibile")));
                    }
                    else
                    {
                        XElement element10;
                        XElement content = new XElement("incontro");
                        string str3 = row["ID_COMPETIZIONE"].ToString();
                        string str4 = row["NOME"].ToString();
                        string str5 = row["NOME_EVENTO"].ToString();
                        string str6 = row["NOME_CALENDARIO"].ToString();
                        string str7 = row["ID_SQUADRA_1"].ToString();
                        string str8 = row["ID_SQUADRA_2"].ToString();
                        DateTime d = (DateTime)row["DATA_EVENTO"];
                        DateTime minValue = DateTime.MinValue;
                        if (!string.IsNullOrEmpty(row["DATA_POSTICIPO"].ToString()))
                        {
                            minValue = (DateTime)row["DATA_POSTICIPO"];
                        }
                        string str9 = row["ORA"].ToString();
                        if (!string.IsNullOrEmpty(row["ORA_POSTICIPO"].ToString()))
                        {
                            str9 = row["ORA_POSTICIPO"].ToString();
                        }
                        string str10 = row["ID_CAMPO"].ToString();
                        if (!row["ID_CAMPO_RECUPERO"].ToString().Equals("0") && !string.IsNullOrEmpty(row["ID_CAMPO_RECUPERO"].ToString()))
                        {
                            str10 = row["ID_CAMPO_RECUPERO"].ToString();
                        }
                        string str11 = string.Empty;
                        string str12 = string.Empty;
                        str11 = !str7.Equals("0") ? this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str7)) : "Vincente";
                        str12 = !str8.Equals("0") ? this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str8)) : "Vincente";
                        string str13 = string.Empty;
                        string str14 = string.Empty;
                        string str15 = string.Empty;
                        string str16 = string.Empty;
                        DataRow row2 = this.objLibDB.GetDataRowFromTable("CAMPI", "ID", this.objLibString.sQuote(str10));
                        if (row2 != null)
                        {
                            str13 = row2["NOMECAMPO"].ToString();
                            if (!string.IsNullOrEmpty("" + row2["GMAP"]))
                            {
                                str14 = row2["GMAP"].ToString().Replace("&amp;ll=", "&ll=");
                                int index = str14.IndexOf("&ll=");
                                if (index > 0)
                                {
                                    str14 = str14.Substring(index + 4, str14.Length - (index + 4));
                                    index = str14.IndexOf("&");
                                    if (index > 0)
                                    {
                                        char[] separator = new char[] { ',' };
                                        string[] strArray = str14.Substring(0, index).Split(separator);
                                        if (strArray.Length != 0)
                                        {
                                            str15 = strArray[0];
                                            str16 = strArray[1];
                                        }
                                    }
                                }
                            }
                        }
                        if (minValue > d)
                        {
                            d = minValue;
                        }
                        XElement element3 = new XElement("id", new XCData(str2));
                        XElement element1 = new XElement("id", new XCData(str3));
                        XElement element26 = new XElement("nome", new XCData(str4));
                        XElement element27 = new XElement("nome_evento", new XCData(str5));
                        XElement element4 = new XElement("nome_calendario", new XCData(str6));
                        XElement element5 = new XElement("id_squadra1", new XCData(str7));
                        XElement element6 = new XElement("id_squadra2", new XCData(str8));
                        XElement element7 = new XElement("squadra1", new XCData(str11));
                        XElement element8 = new XElement("squadra2", new XCData(str12));
                        XElement element9 = new XElement("nomecampo", new XCData(str13));
                        element10 = element10 = new XElement("data", new XCData(this.objLibDate.DateToString(d, "GMA", "/")));
                        XElement element11 = new XElement("ora", new XCData(str9));
                        XElement element12 = new XElement("id_campo", new XCData(str10));
                        XElement element13 = new XElement("google_lat", new XCData(str15));
                        XElement element14 = new XElement("google_lon", new XCData(str16));
                        XElement element15 = null;
                        XElement element16 = null;
                        XElement element17 = null;
                        string query = "SELECT * FROM LS_INCONTRI WHERE ID_INCONTRO=" + this.objLibString.sQuote(str2) + " AND ABILITATO = 1";
                        IDataProvider provider1 = IDataProviderFactory.factory();
                        SimpleDataSet set = provider1.executeQuery(query);
                        if (set.Table.Rows.Count <= 0)
                        {
                            element17 = new XElement("flLive", new XCData("0"));
                            element15 = new XElement("risultato_1", new XCData("-"));
                            element16 = new XElement("risultato_2", new XCData("-"));
                        }
                        else
                        {
                            string str19 = set.Table.Rows[0]["PUNTEGGIO_1"].ToString();
                            string str20 = set.Table.Rows[0]["PUNTEGGIO_2"].ToString();
                            if (set.Table.Rows[0]["FL_LIVE"].ToString().Equals("1"))
                            {
                                element17 = new XElement("flLive", new XCData("1"));
                                element15 = new XElement("risultato_1", new XCData("0"));
                                element16 = new XElement("risultato_2", new XCData("0"));
                            }
                            else
                            {
                                element17 = new XElement("flLive", new XCData("0"));
                                element15 = new XElement("risultato_1", new XCData("-"));
                                element16 = new XElement("risultato_2", new XCData("-"));
                            }
                            if (!string.IsNullOrEmpty(str19))
                            {
                                element15 = new XElement("risultato_1", new XCData(str19));
                            }
                            if (!string.IsNullOrEmpty(str20))
                            {
                                element16 = new XElement("risultato_2", new XCData(str20));
                            }
                        }
                        provider1.close();
                        string str18 = "SELECT * FROM INCONTRI_RISULTATI_V WHERE ID=" + this.objLibString.sQuote(str2) + " AND PUNTEGGIO_1 IS NOT NULL AND PUNTEGGIO_2 IS NOT NULL";
                        IDataProvider provider2 = IDataProviderFactory.factory();
                        SimpleDataSet set2 = provider2.executeQuery(str18);
                        if (set2.Table.Rows.Count > 0)
                        {
                            element15 = new XElement("risultato_1", new XCData(set2.Table.Rows[0]["PUNTEGGIO_1"].ToString()));
                            element16 = new XElement("risultato_2", new XCData(set2.Table.Rows[0]["PUNTEGGIO_2"].ToString()));
                        }
                        provider2.close();
                        content.Add(element3);
                        content.Add(element4);
                        content.Add(element5);
                        content.Add(element6);
                        content.Add(element7);
                        content.Add(element8);
                        content.Add(element10);
                        content.Add(element11);
                        content.Add(element12);
                        content.Add(element9);
                        content.Add(element15);
                        content.Add(element16);
                        content.Add(element13);
                        content.Add(element14);
                        content.Add(element17);
                        element.Add(content);
                    }
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }


        [WebMethod(Description = "Dettaglio News."), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode dettaglioNews(string _token, string _idNews)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else
                {
                    string str2 = _idNews;
                    if (string.IsNullOrEmpty(str2))
                    {
                        element.Add(new XElement("stato_response", new XCData("99")));
                        element.Add(new XElement("msg", new XCData("id news non valido")));
                    }
                    else
                    {
                        element.Add(new XElement("stato_response", new XCData("1")));
                        string query = "SELECT * FROM APP_NEWS_V WHERE ID=" + this.objLibString.sQuote(str2);
                        SimpleDataSet set = IDataProviderFactory.factory().executeQuery(query);
                        XElement content = new XElement("news");
                        XAttribute attribute = null;
                        if (set.Table.Rows.Count <= 0)
                        {
                            attribute = new XAttribute("num", "0");
                        }
                        else
                        {
                            attribute = new XAttribute("num", set.Table.Rows.Count.ToString());
                            DataRow row = set.Table.Rows[0];
                            string str4 = row["TITOLO"].ToString();
                            string str5 = str4;
                            if (!string.IsNullOrEmpty(str5))
                            {
                                str5 = str5.Replace("'", @"\'").Replace("\n", "").Replace("<br />", @"\n").Replace("\"", @"\'");
                            }
                            string str6 = row["TESTO"].ToString();
                            string str7 = string.Empty;
                            if (!string.IsNullOrEmpty(str6))
                            {
                                str6 = str6.Replace("\r", "<br />");
                                str7 = str6.Replace("'", @"\'").Replace("\n", "").Replace("<br />", @"\n").Replace("\"", @"\'");
                            }
                            XElement element4 = new XElement("id", new XCData(row["ID"].ToString()));
                            XElement element5 = null;
                            DateTime d = (DateTime)row["DATA_NEWS"];
                            string str8 = this.objLibDate.DateToString(d, "GMA", "/");
                            element5 = new XElement("data", new XCData(str8));
                            XElement element6 = new XElement("titolo", new XCData(str4));
                            XElement element7 = new XElement("titolo_share", new XCData(str5));
                            XElement element8 = new XElement("testo", new XCData(str6));
                            XElement element9 = new XElement("testo_share", new XCData(str7));
                            XElement element10 = null;
                            XElement element11 = null;
                            if (string.IsNullOrEmpty("" + row["IMMAGINE"]))
                            {
                                content.Add(new XAttribute("foto", "0"));
                            }
                            else
                            {
                                content.Add(new XAttribute("foto", "1"));
                                element10 = new XElement("img", new XCData(this.sStartingpage + this.uploadDirNews + row["IMMAGINE"].ToString()));
                            }
                            if (!string.IsNullOrEmpty("" + row["LINK_ESTERNO"]))
                            {
                                element11 = new XElement("link", new XCData(row["LINK_ESTERNO"].ToString()));
                            }
                            content.Add(element4);
                            content.Add(element6);
                            content.Add(element7);
                            content.Add(element8);
                            content.Add(element9);
                            content.Add(element5);
                            content.Add(element10);
                            content.Add(element11);
                        }
                        content.Add(attribute);
                        element.Add(content);
                    }
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }

        [WebMethod(Description = "Elenco banner in home."), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode elencoBanner(string _token, string _num)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else
                {
                    element.Add(new XElement("stato_response", new XCData("1")));
                    string query = "SELECT TOP " + _num.ToString() + "* FROM APP_BANNER_V ORDER BY ID DESC";
                    SimpleDataSet set = IDataProviderFactory.factory().executeQuery(query);
                    XElement content = new XElement("banners");
                    XAttribute attribute = null;
                    if (set.Table.Rows.Count <= 0)
                    {
                        attribute = new XAttribute("num", "0");
                    }
                    else
                    {
                        attribute = new XAttribute("num", set.Table.Rows.Count.ToString());
                        for (int i = 0; i < set.Table.Rows.Count; i++)
                        {
                            DataRow row = set.Table.Rows[i];
                            XElement element4 = new XElement("banner");
                            XElement element5 = new XElement("id", new XCData(row["ID"].ToString()));
                            XElement element6 = new XElement("titolo", new XCData(row["TITOLO"].ToString()));
                            XElement element7 = null;
                            XElement element8 = null;
                            if (!string.IsNullOrEmpty("" + row["IMMAGINE"]))
                            {
                                element7 = new XElement("img", new XCData(this.sStartingpage + this.uploadDirBanner + row["IMMAGINE"].ToString()));
                            }
                            if (!string.IsNullOrEmpty(""+row["LINK_ESTERNO"]))
                            {
                                element8 = new XElement("link", new XCData(row["LINK_ESTERNO"].ToString()));
                            }
                            element4.Add(element5);
                            element4.Add(element6);
                            element4.Add(element7);
                            element4.Add(element8);
                            content.Add(element4);
                        }
                    }
                    content.Add(attribute);
                    element.Add(content);
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }


        [WebMethod(Description = "Elenco eventi di una squadra."), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode elencoEventiSquadra(string _token, string _idsquadra)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            string str2 = _idsquadra;
            int num = 0;
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else if (string.IsNullOrEmpty(str2))
                {
                    element.Add(new XElement("stato_response", new XCData("2")));
                    element.Add(new XElement("msg", new XCData("min 3 char.")));
                }
                else
                {
                    string[] textArray1 = new string[] { "SELECT ID_COMPETIZIONE, NOME FROM APP_INCONTRI_V WHERE (ID_SQUADRA_1=", this.objLibString.sQuote(str2), " OR ID_SQUADRA_2=", this.objLibString.sQuote(str2), ")" };
                    IDataProvider provider = IDataProviderFactory.factory();
                    SimpleDataSet set = provider.executeQuery(string.Concat(textArray1) + " GROUP BY ID_COMPETIZIONE, NOME ORDER BY ID_COMPETIZIONE ASC");
                    XElement content = new XElement("incontri");
                    if (set.Table.Rows.Count <= 0)
                    {
                        element.Add(new XElement("stato_response", new XCData("2")));
                        element.Add(new XElement("msg", new XCData("Nessun evento disponibile")));
                    }
                    else
                    {
                        element.Add(new XElement("stato_response", new XCData("1")));
                        string str4 = this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str2));
                        content.Add(new XElement("id_squadra", new XCData(str2)));
                        content.Add(new XElement("nome_squadra", new XCData(str4)));
                        int num2 = 0;
                        while (num2 < set.Table.Rows.Count)
                        {
                            XElement element6 = new XElement("competizione");
                            DataRow row1 = set.Table.Rows[num2];
                            string str5 = row1["ID_COMPETIZIONE"].ToString();
                            string str6 = row1["NOME"].ToString();
                            XElement element7 = new XElement("id_competizione", new XCData(str5));
                            XElement element8 = new XElement("nome_competizione", new XCData(str6));
                            element6.Add(element7);
                            element6.Add(element8);
                            XElement element9 = new XElement("eventi");
                            string[] textArray2 = new string[] { "SELECT ID_EVENTO, NOME_EVENTO FROM APP_INCONTRI_V WHERE (ID_SQUADRA_1=", this.objLibString.sQuote(str2), " OR ID_SQUADRA_2=", this.objLibString.sQuote(str2), ")" };
                            string query = (string.Concat(textArray2) + " AND ID_COMPETIZIONE=" + this.objLibString.sQuote(str5)) + " GROUP BY ID_EVENTO, NOME_EVENTO ORDER BY ID_EVENTO ASC";
                            IDataProvider provider2 = IDataProviderFactory.factory();
                            SimpleDataSet set2 = provider2.executeQuery(query);
                            num += set2.Table.Rows.Count;
                            int num3 = 0;
                            while (true)
                            {
                                if (num3 >= set2.Table.Rows.Count)
                                {
                                    provider2.close();
                                    element6.Add(element9);
                                    content.Add(element6);
                                    num2++;
                                    break;
                                }
                                DataRow row2 = set2.Table.Rows[num3];
                                string str8 = row2["ID_EVENTO"].ToString();
                                string str9 = row2["NOME_EVENTO"].ToString();
                                XElement element10 = new XElement("evento");
                                XElement element11 = new XElement("id_evento", new XCData(str8));
                                XElement element12 = new XElement("nome_evento", new XCData(str9));
                                element10.Add(element11);
                                element10.Add(element12);
                                element9.Add(element10);
                                num3++;
                            }
                        }
                    }
                    provider.close();
                    content.Add(new XAttribute("num", num));
                    element.Add(content);
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }


        [WebMethod(Description = "Elenco gare. Prese dalla tab in home page [o] OGGI, [i] IERI [d] DOMANI."), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode elencoGare(string _token, string _periodo)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            string str2 = string.Empty;
            int num = 0;
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else if (string.IsNullOrEmpty(_periodo))
                {
                    element.Add(new XElement("stato_response", new XCData("2")));
                    element.Add(new XElement("msg", new XCData("min 3 char.")));
                }
                else
                {
                    _periodo = _periodo.ToLower();
                    if (!"o,i,d".ToLower().Contains(_periodo))
                    {
                        element.Add(new XElement("stato_response", new XCData("99")));
                        element.Add(new XElement("msg", new XCData("Parametro non valido")));
                    }
                    else
                    {
                        string query = string.Empty;
                        if (_periodo.Equals("o"))
                        {
                            str2 = " WHERE (CONVERT(date, DATA_EVENTO,112) = CONVERT(date, GETDATE(),112) OR CONVERT(date, DATA_POSTICIPO,112) = CONVERT(date, GETDATE(),112))";
                            query = "SELECT ID_COMPETIZIONE, NOME FROM APP_INCONTRI_V" + str2 + " GROUP BY ID_COMPETIZIONE, NOME ORDER BY ID_COMPETIZIONE ASC";
                        }
                        else if (_periodo.Equals("i"))
                        {
                            query = "SELECT ID_COMPETIZIONE, NOME FROM APP_INCONTRI_V" + " WHERE (CONVERT(date, DATA_EVENTO,112) = CONVERT(date, GETDATE()-1,112) OR CONVERT(date, DATA_POSTICIPO,112) = CONVERT(date, GETDATE()-1,112))" + " GROUP BY ID_COMPETIZIONE, NOME ORDER BY ID_COMPETIZIONE ASC";
                        }
                        else if (_periodo.Equals("d"))
                        {
                            query = "SELECT ID_COMPETIZIONE, NOME FROM APP_INCONTRI_V" + " WHERE (CONVERT(date, DATA_EVENTO,112) = CONVERT(date, GETDATE()+1,112) OR CONVERT(date, DATA_POSTICIPO,112) = CONVERT(date, GETDATE()+1,112))" + " GROUP BY ID_COMPETIZIONE, NOME ORDER BY ID_COMPETIZIONE ASC";
                        }
                        IDataProvider provider = IDataProviderFactory.factory();
                        SimpleDataSet set = provider.executeQuery(query);
                        XElement content = new XElement("incontri");
                        if (set.Table.Rows.Count <= 0)
                        {
                            element.Add(new XElement("stato_response", new XCData("2")));
                            element.Add(new XElement("msg", new XCData("Nessuna gara in programma")));
                        }
                        else
                        {
                            element.Add(new XElement("stato_response", new XCData("1")));
                            int num2 = 0;
                            while (num2 < set.Table.Rows.Count)
                            {
                                XElement element4 = new XElement("competizione");
                                DataRow row1 = set.Table.Rows[num2];
                                string str4 = row1["ID_COMPETIZIONE"].ToString();
                                string str5 = row1["NOME"].ToString();
                                XElement element5 = new XElement("id", new XCData(str4));
                                XElement element6 = new XElement("nome", new XCData(str5));
                                element4.Add(element5);
                                element4.Add(element6);
                                XElement element7 = new XElement("eventi");
                                string str6 = (("SELECT ID_EVENTO, NOME_EVENTO FROM APP_INCONTRI_V " + str2) + " AND ID_COMPETIZIONE=" + this.objLibString.sQuote(str4)) + " GROUP BY ID_EVENTO, NOME_EVENTO ORDER BY NOME_EVENTO ASC";
                                IDataProvider provider2 = IDataProviderFactory.factory();
                                SimpleDataSet set2 = provider2.executeQuery(str6);
                                int num3 = 0;
                                while (true)
                                {
                                    if (num3 >= set2.Table.Rows.Count)
                                    {
                                        provider2.close();
                                        element4.Add(element7);
                                        content.Add(element4);
                                        num2++;
                                        break;
                                    }
                                    DataRow row2 = set2.Table.Rows[num3];
                                    string str7 = row2["ID_EVENTO"].ToString();
                                    string str8 = row2["NOME_EVENTO"].ToString();
                                    XElement element8 = new XElement("evento");
                                    XElement element9 = new XElement("id", new XCData(str7));
                                    XElement element10 = new XElement("nome", new XCData(str8));
                                    element8.Add(element9);
                                    element8.Add(element10);
                                    XElement element11 = new XElement("incontri");
                                    string str9 = (("SELECT * FROM APP_INCONTRI_V " + str2) + " AND ID_EVENTO=" + this.objLibString.sQuote(str7)) + " ORDER BY DATA_POSTICIPO ASC, ORA_POSTICIPO ASC, DATA_EVENTO ASC, ORA ASC";
                                    IDataProvider provider3 = IDataProviderFactory.factory();
                                    SimpleDataSet set3 = provider3.executeQuery(str9);
                                    num += set3.Table.Rows.Count;
                                    int num4 = 0;
                                    while (true)
                                    {
                                        if (num4 >= set3.Table.Rows.Count)
                                        {
                                            provider3.close();
                                            element8.Add(element11);
                                            element7.Add(element8);
                                            num3++;
                                            break;
                                        }
                                        DataRow row = set3.Table.Rows[num4];
                                        string str10 = row["ID_SQUADRA_1"].ToString();
                                        string str11 = row["ID_SQUADRA_2"].ToString();
                                        string str12 = row["ID_INCONTRO"].ToString();
                                        DateTime d = (DateTime)row["DATA_EVENTO"];
                                        DateTime minValue = DateTime.MinValue;
                                        if (!string.IsNullOrEmpty(row["DATA_POSTICIPO"].ToString()))
                                        {
                                            minValue = (DateTime)row["DATA_POSTICIPO"];
                                        }
                                        string str13 = row["ORA"].ToString();
                                        if (!string.IsNullOrEmpty(row["ORA_POSTICIPO"].ToString()))
                                        {
                                            str13 = row["ORA_POSTICIPO"].ToString();
                                        }
                                        string str14 = row["ID_CAMPO"].ToString();
                                        if (!row["ID_CAMPO_RECUPERO"].ToString().Equals("0") && !string.IsNullOrEmpty(row["ID_CAMPO_RECUPERO"].ToString()))
                                        {
                                            str14 = row["ID_CAMPO_RECUPERO"].ToString();
                                        }
                                        XElement element12 = new XElement("incontro");
                                        XElement element13 = new XElement("id", new XCData(str12));
                                        XElement element14 = new XElement("id_squadra1", new XCData(str10));
                                        XElement element15 = new XElement("id_squadra2", new XCData(str11));
                                        string str15 = string.Empty;
                                        string str16 = string.Empty;
                                        str15 = !str10.Equals("0") ? this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str10)) : "Vincente";
                                        str16 = !str11.Equals("0") ? this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str11)) : "Vincente";
                                        XElement element16 = new XElement("squadra1", new XCData(str15));
                                        XElement element17 = new XElement("squadra2", new XCData(str16));
                                        string str17 = this.objLibDB.GetFieldFromTable("CAMPI", "NOMECAMPO", "ID", this.objLibString.sQuote(str14));
                                        XElement element18 = new XElement("nomecampo", new XCData(str17));
                                        XElement element19 = null;
                                        if (minValue > d)
                                        {
                                            d = minValue;
                                        }
                                        element19 = new XElement("data", new XCData(this.objLibDate.DateToString(d, "GMA", "/")));
                                        XElement element20 = new XElement("ora", new XCData(str13));
                                        XElement element21 = new XElement("id_campo", new XCData(str14));
                                        XElement element22 = null;
                                        XElement element23 = null;
                                        XElement element24 = null;
                                        string str18 = "SELECT * FROM LS_INCONTRI WHERE ID_INCONTRO=" + this.objLibString.sQuote(str12) + " AND ABILITATO = 1";
                                        IDataProvider provider1 = IDataProviderFactory.factory();
                                        SimpleDataSet set4 = provider1.executeQuery(str18);
                                        if (set4.Table.Rows.Count <= 0)
                                        {
                                            element24 = new XElement("flLive", new XCData("0"));
                                            element22 = new XElement("risultato_1", new XCData("-"));
                                            element23 = new XElement("risultato_2", new XCData("-"));
                                        }
                                        else
                                        {
                                            string str19 = set4.Table.Rows[0]["PUNTEGGIO_1"].ToString();
                                            string str20 = set4.Table.Rows[0]["PUNTEGGIO_2"].ToString();
                                            if (set4.Table.Rows[0]["FL_LIVE"].ToString().Equals("1"))
                                            {
                                                element24 = new XElement("flLive", new XCData("1"));
                                                element22 = new XElement("risultato_1", new XCData("0"));
                                                element23 = new XElement("risultato_2", new XCData("0"));
                                            }
                                            else
                                            {
                                                element24 = new XElement("flLive", new XCData("0"));
                                                element22 = new XElement("risultato_1", new XCData("-"));
                                                element23 = new XElement("risultato_2", new XCData("-"));
                                            }
                                            if (!string.IsNullOrEmpty(str19))
                                            {
                                                element22 = new XElement("risultato_1", new XCData(str19));
                                            }
                                            if (!string.IsNullOrEmpty(str20))
                                            {
                                                element23 = new XElement("risultato_2", new XCData(str20));
                                            }
                                        }
                                        provider1.close();
                                        query = "SELECT * FROM INCONTRI_RISULTATI_V WHERE ID=" + this.objLibString.sQuote(str12) + " AND PUNTEGGIO_1 IS NOT NULL AND PUNTEGGIO_2 IS NOT NULL";
                                        IDataProvider provider4 = IDataProviderFactory.factory();
                                        SimpleDataSet set5 = provider4.executeQuery(query);
                                        if (set5.Table.Rows.Count > 0)
                                        {
                                            element22 = new XElement("risultato_1", new XCData(set5.Table.Rows[0]["PUNTEGGIO_1"].ToString()));
                                            element23 = new XElement("risultato_2", new XCData(set5.Table.Rows[0]["PUNTEGGIO_2"].ToString()));
                                        }
                                        provider4.close();
                                        element12.Add(element13);
                                        element12.Add(element14);
                                        element12.Add(element15);
                                        element12.Add(element16);
                                        element12.Add(element17);
                                        element12.Add(element19);
                                        element12.Add(element20);
                                        element12.Add(element21);
                                        element12.Add(element18);
                                        element12.Add(element22);
                                        element12.Add(element23);
                                        element12.Add(element24);
                                        element11.Add(element12);
                                        num4++;
                                    }
                                }
                            }
                        }
                        provider.close();
                        content.Add(new XAttribute("num", num));
                        element.Add(content);
                    }
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }



        [WebMethod(Description = "Elenco news in homepage o in archivio.<br> Il parametro _flHome \x00e8 [0] No e [1] Si. (optionale)"), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode elencoNews(string _token, string _flHome)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else
                {
                    element.Add(new XElement("stato_response", new XCData("1")));
                    string query = "SELECT * FROM APP_NEWS_V ";
                    if (_flHome.Equals("1"))
                    {
                        query = query + " WHERE FL_HOME = '1'";
                    }
                    query = query + " ORDER BY DATA_NEWS DESC";
                    SimpleDataSet set = IDataProviderFactory.factory().executeQuery(query);
                    XElement content = new XElement("news");
                    XAttribute attribute = null;
                    if (set.Table.Rows.Count <= 0)
                    {
                        attribute = new XAttribute("num", "0");
                    }
                    else
                    {
                        attribute = new XAttribute("num", set.Table.Rows.Count.ToString());
                        for (int i = 0; i < set.Table.Rows.Count; i++)
                        {
                            DataRow row = set.Table.Rows[i];
                            string str3 = row["TITOLO"].ToString();
                            if (str3.Length > 50)
                            {
                                str3 = this.objLibString.TroncaTesto(str3, 0x31, false) + "...";
                            }
                            XElement element4 = new XElement("news");
                            XElement element5 = new XElement("id", new XCData(row["ID"].ToString()));
                            XElement element6 = new XElement("titolo", new XCData(str3));
                            DateTime d = (DateTime)row["DATA_NEWS"];
                            XElement element7 = null;
                            string str4 = this.objLibDate.DateToString(d, "GMA", "/");
                            element7 = new XElement("data", new XCData(str4));
                            XElement element8 = null;
                            XElement element9 = null;
                            if (string.IsNullOrEmpty("" + row["IMMAGINE"]))
                            {
                                element4.Add(new XAttribute("foto", "0"));
                            }
                            else
                            {
                                element4.Add(new XAttribute("foto", "1"));
                                element8 = new XElement("img", new XCData(this.sStartingpage + this.uploadDirNews + row["IMMAGINE"].ToString()));
                            }
                            if (!string.IsNullOrEmpty("" + row["LINK_ESTERNO"]))
                            {
                                element9 = new XElement("link", new XCData(row["LINK_ESTERNO"].ToString()));
                            }
                            element4.Add(element5);
                            element4.Add(element7);
                            element4.Add(element6);
                            element4.Add(element8);
                            element4.Add(element9);
                            content.Add(element4);
                        }
                    }
                    content.Add(attribute);
                    element.Add(content);
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }


        [WebMethod(Description = "Elenco squadre passando una stringa."), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode elencoSquadre(string _token, string _text)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else if (string.IsNullOrEmpty(_text) || (_text.Length < 3))
                {
                    element.Add(new XElement("stato_response", new XCData("2")));
                    element.Add(new XElement("msg", new XCData("inserire minimo 3 caratteri per la ricerca")));
                }
                else
                {
                    element.Add(new XElement("stato_response", new XCData("1")));
                    string query = "SELECT * FROM APP_ELENCO_SQUADRE_V WHERE LOWER(NOME) LIKE '%" + this.objLibString.sAquote(_text.ToLower()) + "%' ORDER BY NOME ASC";
                    SimpleDataSet set = IDataProviderFactory.factory().executeQuery(query);
                    XElement content = new XElement("squadre");
                    XAttribute attribute = null;
                    if (set.Table.Rows.Count <= 0)
                    {
                        attribute = new XAttribute("num", "0");
                    }
                    else
                    {
                        attribute = new XAttribute("num", set.Table.Rows.Count.ToString());
                        for (int i = 0; i < set.Table.Rows.Count; i++)
                        {
                            DataRow row = set.Table.Rows[i];
                            XElement element4 = new XElement("squadra");
                            XElement element5 = new XElement("id", new XCData(row["ID_SQUADRA"].ToString()));
                            XElement element6 = new XElement("nome", new XCData(row["NOME"].ToString()));
                            element4.Add(element5);
                            element4.Add(element6);
                            content.Add(element4);
                        }
                    }
                    content.Add(attribute);
                    element.Add(content);
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }



        [WebMethod(Description = "Elenco gare future di una specifica squadra"), ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public XmlNode prossimeGare(string _token, string _idsquadra)
        {
            string str = this.objLibCript.uDecode(_token.Trim(), this.objLibCript.getCryptKey(this.sCryptKey));
            int num = 0;
            XElement element = new XElement("elenco");
            try
            {
                if (!str.Equals(this.sCryptKey))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Token non valido")));
                }
                else if (string.IsNullOrEmpty(_idsquadra))
                {
                    element.Add(new XElement("stato_response", new XCData("99")));
                    element.Add(new XElement("msg", new XCData("Parametro non valido")));
                }
                else
                {
                    string str2 = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0');
                    string str3 = ("( ((CONVERT(date, DATA_EVENTO,112) = CONVERT(date, GETDATE(),112) AND (REPLACE(ORA, ':', '')  > " + str2 + "))") + " OR (CONVERT(date, DATA_EVENTO,112) > CONVERT(date, GETDATE(),112)))";
                    string[] textArray1 = new string[] { str3, "AND (ID_SQUADRA_1 =", this.objLibString.sQuote(_idsquadra), " OR ID_SQUADRA_2=", this.objLibString.sQuote(_idsquadra), ") )" };
                    str3 = ((string.Concat(textArray1) + "OR (") + "((CONVERT(date, DATA_POSTICIPO,112) = CONVERT(date, GETDATE(),112) AND (REPLACE(ORA_POSTICIPO, ':', '')  > " + str2 + "))") + "OR (CONVERT(date, DATA_POSTICIPO,112) > CONVERT(date, GETDATE(),112)))";
                    string[] textArray2 = new string[] { str3, "AND (ID_SQUADRA_1 = ", this.objLibString.sQuote(_idsquadra), " OR ID_SQUADRA_2=", this.objLibString.sQuote(_idsquadra), ")" };
                    str3 = string.Concat(textArray2) + ")";
                    IDataProvider provider = IDataProviderFactory.factory();
                    SimpleDataSet set = provider.executeQuery((" SELECT * FROM APP_INCONTRI_V" + " WHERE " + str3) + " ORDER BY isnull(DATA_POSTICIPO, data_Evento)  ASC, isnull(ORA_POSTICIPO, ORA) ASC, DATA_EVENTO ASC, ORA ASC");
                    num += set.Table.Rows.Count;
                    XElement content = new XElement("incontri");
                    if (set.Table.Rows.Count <= 0)
                    {
                        element.Add(new XElement("stato_response", new XCData("2")));
                        element.Add(new XElement("msg", new XCData("Nessuna gara in programma")));
                    }
                    else
                    {
                        element.Add(new XElement("stato_response", new XCData("1")));
                        for (int i = 0; i < set.Table.Rows.Count; i++)
                        {
                            DataRow row = set.Table.Rows[i];
                            XElement element4 = new XElement("competizione");
                            string str5 = row["ID_COMPETIZIONE"].ToString();
                            string str6 = row["NOME"].ToString();
                            XElement element5 = new XElement("id", new XCData(str5));
                            XElement element6 = new XElement("nome", new XCData(str6));
                            element4.Add(element5);
                            element4.Add(element6);
                            XElement element7 = new XElement("eventi");
                            string str7 = row["ID_EVENTO"].ToString();
                            string str8 = row["NOME_EVENTO"].ToString();
                            XElement element8 = new XElement("evento");
                            XElement element9 = new XElement("id", new XCData(str7));
                            XElement element10 = new XElement("nome", new XCData(str8));
                            element8.Add(element9);
                            element8.Add(element10);
                            XElement element11 = new XElement("incontri");
                            string str9 = row["ID_SQUADRA_1"].ToString();
                            string str10 = row["ID_SQUADRA_2"].ToString();
                            string str11 = row["ID_INCONTRO"].ToString();
                            DateTime d = (DateTime)row["DATA_EVENTO"];
                            DateTime minValue = DateTime.MinValue;
                            if (!string.IsNullOrEmpty(row["DATA_POSTICIPO"].ToString()))
                            {
                                minValue = (DateTime)row["DATA_POSTICIPO"];
                            }
                            string str12 = row["ORA"].ToString();
                            if (!string.IsNullOrEmpty(row["ORA_POSTICIPO"].ToString()))
                            {
                                str12 = row["ORA_POSTICIPO"].ToString();
                            }
                            string str13 = row["ID_CAMPO"].ToString();
                            if (!row["ID_CAMPO_RECUPERO"].ToString().Equals("0") && !string.IsNullOrEmpty(row["ID_CAMPO_RECUPERO"].ToString()))
                            {
                                str13 = row["ID_CAMPO_RECUPERO"].ToString();
                            }
                            XElement element12 = new XElement("incontro");
                            XElement element13 = new XElement("id", new XCData(str11));
                            XElement element14 = new XElement("id_squadra1", new XCData(str9));
                            XElement element15 = new XElement("id_squadra2", new XCData(str10));
                            string str14 = string.Empty;
                            string str15 = string.Empty;
                            str14 = !str9.Equals("0") ? this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str9)) : "Vincente";
                            str15 = !str10.Equals("0") ? this.objLibDB.GetFieldFromTable("SQUADRE", "NOME", "ID", this.objLibString.sQuote(str10)) : "Vincente";
                            XElement element16 = new XElement("squadra1", new XCData(str14));
                            XElement element17 = new XElement("squadra2", new XCData(str15));
                            string str16 = this.objLibDB.GetFieldFromTable("CAMPI", "NOMECAMPO", "ID", this.objLibString.sQuote(str13));
                            XElement element18 = new XElement("nomecampo", new XCData(str16));
                            if (minValue > d)
                            {
                                d = minValue;
                            }
                            element12.Add(element13);
                            element12.Add(element14);
                            element12.Add(element15);
                            element12.Add(element16);
                            element12.Add(element17);
                            element12.Add(new XElement("data", new XCData(this.objLibDate.DateToString(d, "GMA", "/"))));
                            element12.Add(new XElement("ora", new XCData(str12)));
                            element12.Add(new XElement("id_campo", new XCData(str13)));
                            element12.Add(element18);
                            element12.Add(new XElement("risultato_1", new XCData("-")));
                            element12.Add(new XElement("risultato_2", new XCData("-")));
                            element12.Add(new XElement("flLive", new XCData("0")));
                            element11.Add(element12);
                            element8.Add(element11);
                            element7.Add(element8);
                            element4.Add(element7);
                            content.Add(element4);
                        }
                    }
                    provider.close();
                    content.Add(new XAttribute("num", num));
                    element.Add(content);
                }
            }
            catch (Exception exception)
            {
                element.Add(new XElement("stato_response", new XCData("99")));
                element.Add(new XElement("msg", new XCData(exception.Message)));
            }
            return GetXmlNode(element);
        }


    }
}
