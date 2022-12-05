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
using System.Text.RegularExpressions;
//
using Support.db;
using Support.Web;

namespace Support.Library
{
    public class StringUtil
	{
        public StringUtil() { }

        #region String Function
        /// <summary>
        /// Metodo che data una stringa restituisce il valore dalla stringa stessa o la string di default
        /// </summary>
        /// <param name="str">stringa passata</param>
        /// <param name="strDef">stringa di default</param>
        /// <returns>String contente il valore passato o il valore definito in caso il valore passato sia vuoto</returns>
        public String sSetVarDef(String str, String strDef)
		{	
			if ((str == null)||(str.Length==0))
				return strDef;
			else
				return str;
		}

        /// <summary>
        /// Metodo che data una stringa restituisce il la stringa stessa racchiuso da '
        /// </summary>
        /// <param name="str">stringa passata</param>
        /// <returns>String contente il valore passato racchiuso da '</returns>
		public String sQuote(String str)
		{	
			//return "'" + sAquote(str) + "'";
            return "N'" + sAquote(str) + "'";
		}

        /// <summary>
        /// Metodo che data una stringa con il carattere ' raddoppiato
        /// </summary>
        /// <param name="str">stringa passata</param>
        /// <returns>String contente il valore passato con ' raddoppiati</returns>
		public String sAquote(String str)
		{	
			return str.Replace("'","''");
		}

        /// <summary>
        /// Metodo che data una stringa restituisce la stringa con racchiusa da tag <![CDATA[str]]>
        /// </summary>
        /// <param name="str">stringa passata</param>
        /// <returns>String con il valore passato racchiuso da tag <![CDATA[str]]></returns>
		public String sCDATA(String str)
		{	
			return "<![CDATA[" + str + "]]>";
		}

        /// <summary>
        /// Metodo che data una stringa sostituisce il carattere ' con \'
        /// </summary>
        /// <param name="str">stringa passata</param>
        /// <returns>String</returns>
        public String sEscapeReplace(String str){
            str = str.Replace('’', '\'');
            return str;
        }

        /// <summary>
        /// Metodo che data una stringa elimina tutti i tag html presenti
        /// </summary>
        /// <param name="strHTML">stringa passata</param>
        /// <returns>String</returns>
        public String sStripHTML(String strHTML)
        {
            /*
            String strResult = String.Empty;
            int ePos;
            int sPos;

            if (!String.IsNullOrEmpty(strHTML))
            {

                if (strHTML.Length <= 0)
                    strHTML = "";

                strResult = strHTML;
                sPos = strResult.IndexOf('<', 0);

                while (sPos >= 0)
                {
                    ePos = strResult.IndexOf('>', sPos);
                    if (ePos >= 0)
                    {
                        String sTrTmp = String.Empty;
                        sTrTmp += strResult.Substring(sPos, ePos - sPos + 1);
                        strResult = strResult.Replace(sTrTmp, "");
                    }

                    sPos = strResult.IndexOf('<', 0);
                }
            }
             * */
            if (!String.IsNullOrEmpty(strHTML))
                return Regex.Replace(strHTML, @"<[^>]*>", String.Empty);
            else
                return strHTML;
        }
        public String sStripHTMLExcludeBR(String strHTML)
        {
            return Regex.Replace(strHTML, @"<(?!br[\x20/>])[^<>]+>", String.Empty);
        }
        

        /// <summary>
        /// Metodo che data una stringa elimina tutti i tag html presenti
        /// </summary>
        /// <param name="src">stringa passata</param>
        /// <param name="toFind">stringa da ricercare</param>
        /// <param name="toReplace">stringa con cui sostituire</param>
        /// <returns>String</returns>
        public String sReplace(String src, String toFind, String toReplace)
        {
            return src.Replace(toFind, toReplace);
        }

        /// <summary>
        /// Metodo che data una stringa la divide in un array per la stringa ricercata
        /// </summary>
        /// <param name="src">stringa passata</param>
        /// <param name="toFind">stringa da ricercare</param>
        /// <returns>String</returns>
        public String[] splitStringByString(String src, String toFind)
        {
            int offset = 0;
            int index = 0;
            int[] offsets = new int[src.Length + 1];

            while (index < src.Length)
            {
                int indexOf = src.IndexOf(toFind, index);
                if (indexOf != -1)
                {
                    offsets[offset++] = indexOf;
                    index = (indexOf + toFind.Length);
                }
                else
                {
                    index = src.Length;
                }
            }

            String[] final = new String[offset + 1];
            if (offset == 0)
            {
                final[0] = src;
            }
            else
            {
                offset--;
                final[0] = src.Substring(0, offsets[0]);
                for (int i = 0; i < offset; i++)
                {
                    final[i + 1] = src.Substring(offsets[i] + toFind.Length, offsets[i + 1] - offsets[i] - toFind.Length);
                }
                final[offset + 1] = src.Substring(offsets[offset] + toFind.Length);
            }
            return final;
        }

        /// <summary>
        /// Metodo che dato un array di byte restituisce la stringa
        /// </summary>
        /// <param name="source">array byte</param>
        /// <returns>Stringa</returns>
        public String byteArrayToString(byte[] source)
        {
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            return enc.GetString(source);
        }

        /// <summary>
        /// Metodo che data una string restituisce un array di byte
        /// </summary>
        /// <param name="source">stringa</param>
        /// <returns>Array di byte</returns>
        public byte[] stringToByteArray(String source)
        {
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            return enc.GetBytes(source);
        }

        /// <summary>
        /// Metodo che restituisce un numero in importo in euro
        /// </summary>
        /// <param name="val">stringa where</param>
        /// <returns>String con importo in euro</returns>
        public String NumberToEuro(double val)
        {

            long fixPart;
            long decPart;

            String ret1;
            String ret2;
            String sReturn = String.Empty;
            fixPart = (int)Math.Abs(val);
            decPart = System.Convert.ToInt64((Math.Abs(val) - fixPart) * 100) % 100;

            ret1 = NumberToLire(Math.Truncate(val));
            ret2 = decPart.ToString().PadLeft(2, '0');

            return ret1 + "," + ret2;
        }

        /// <summary>
        /// Metodo che restituisce un numero suddiviso in Migliaia
        /// </summary>
        /// <param name="val">stringa where</param>
        /// <returns>String con importo suddiviso in migliaia</returns>
        public String NumberToLire(double val)
        {
            String num = String.Empty;
            String ret = String.Empty;
            String sReturn = String.Empty;

            num = Math.Abs(val).ToString();
            while (num.Length > 3)
            {
                ret = "." + num.Substring(num.Length - 3, 3) + ret;
                num = num.Substring(0, num.Length - 3);
            }

            ret = num + ret;

            if (val < 0)
                ret = "-" + ret;

            return ret;
        }

        /// <summary>
        /// Metodo che restituisce la stringa con ogni parola con la prima lettera Maiuscola
        /// </summary>
        /// <param name="val">stringa where</param>
        /// <returns>String con importo suddiviso in migliaia</returns>
        public String ProperCase(String str)
        {
            int prop = 0;
            String Newstr = String.Empty;

            if (str.Length > 0)
            {
                str = str.ToLower();
                for (int i = 0; i < str.Length; i++)
                {
                    if (str.Substring(i, 1).Equals(" "))
                    {
                        prop = 1;
                        Newstr = Newstr + str.Substring(i, 1);
                    }
                    else
                    {
                        if (prop.Equals(1) || i.Equals(0))
                        {
                            Newstr = Newstr + str.Substring(i, 1).ToUpper();
                            prop = 0;
                        }
                        else
                        {
                            Newstr = Newstr + str.Substring(i, 1);
                        }
                    }
                }
            }
            else
            {
                Newstr = str;
            }

            return Newstr;
        }
        /// <summary>
        /// Metodo che restituisce la stringa senza caratteri non adatti alle url
        /// </summary>
        /// <param name="val">stringa where</param>
        /// <returns>String con importo suddiviso in migliaia</returns>
        public String ClearIDHTML(String _url)
        {
            String sReturn = String.Empty;
            if (!String.IsNullOrEmpty(_url))
            {
                _url = sStripHTML(_url);

                String replacement = "_";
                sReturn = Regex.Replace(_url, @"[^\w\.-]", replacement);
                sReturn = sReturn.Replace(".", "");
                sReturn = sReturn.Replace("-", "");
            }
            else
                sReturn = "";

            return sReturn;
        }
        /// <summary>
        /// Metodo che restituisce la stringa senza caratteri non adatti alle url
        /// </summary>
        /// <param name="val">stringa where</param>
        /// <returns>String con importo suddiviso in migliaia</returns>
        public String ClearUrlName(String _url)
        {
            String sReturn = String.Empty;
            if (!String.IsNullOrEmpty(_url))
            {
                _url = sStripHTML(_url);
                
                String replacement = "_";
                sReturn = _url.Replace("%20", replacement);
                sReturn = Regex.Replace(sReturn, @"[^\w\.-]", replacement);
                sReturn = sReturn.Replace(" ", "_");
                sReturn = sReturn.Replace(".", "");
                sReturn = sReturn.ToLower();
                sReturn = sReturn.Trim();
            }
            else
                sReturn = "";

            return sReturn;
        }
        /// <summary>
        /// Metodo che restituisce la stringa troncata
        /// </summary>
        /// <param name="val">stringa where</param>
        /// <returns>String con importo suddiviso in migliaia</returns>
        public String TroncaTesto(String _text, int _nChars)
        {
            return TroncaTesto(_text, _nChars, true);
        }
        /// <summary>
        /// Metodo che restituisce la stringa troncata
        /// </summary>
        /// <param name="val">stringa where</param>
        /// <returns>String con importo suddiviso in migliaia</returns>
        public String TroncaTesto(String _text, int _nChars, bool _bPointer)
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
                    if (trimOffset > -1)
                        if (_bPointer)
                            tempString = tempString.Substring(0, trimOffset) + "&#8230;";
                        else
                            tempString = tempString.Substring(0, trimOffset);
                }

                sReturn = tempString;
            }


            return sReturn;
        }
        /// <summary>
        /// Metodo che restituisce l'estensione del file dal nome file
        /// </summary>
        /// <param name="val">stringa _filename</param>
        /// <returns>String con estensione del file</returns>
        public String GetExtentionFromFileName(String _filename)
        {
            String extention = String.Empty;

            if (!String.IsNullOrEmpty(_filename))
            {
                int trimOffset = _filename.LastIndexOf(".");
                if (trimOffset > -1)
                    extention = _filename.Substring(trimOffset + 1);
            }
            return extention;
        }

        public String CombinePath(String baseUrl, String url1)
        {
            String u1 = Safe(baseUrl);
            String u2 = Safe(url1);
            // 
            if (u1.Length == 0)
                return u2;
            if (u2.Length == 0)
                return u1;
            if (u1.EndsWith("\\") || u1.EndsWith("/"))
            {
                if (u2.StartsWith("\\") || u2.StartsWith("/"))
                    return u1 + (u2.Length > 1 ? u2.Substring(1) : "");
                return u1 + u2;
            }
            if (u2.StartsWith("\\") || u2.StartsWith("/"))
                return u1 + u2;
            return u1 + "/" + u2;
        }
        public string Safe(String str)
        {
            return (str == null) ? "" : str;
        }
        #endregion
    }
}
