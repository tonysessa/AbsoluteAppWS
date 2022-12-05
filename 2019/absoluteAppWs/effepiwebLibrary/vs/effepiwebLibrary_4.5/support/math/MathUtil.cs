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

namespace Support.Library
{
	public class MathUtil
	{
        public MathUtil() { }

        #region Math Function
        /// <summary>
        /// Metodo che data una stringa restituisce true per valore numerico valido
        /// </summary>
        /// <param name="str">valore da controllare</param>
        /// <returns>Bool con true per valore numerico corretto</returns>
        public Boolean isNumber(String str)
        {
            if (String.IsNullOrEmpty(str))
                return false;

            Boolean isNumber = str.Length > 0;
            int i = 0;
            while (i < str.Length && isNumber)
            {
                isNumber = Char.IsNumber(str, i);
                i++;
            }
            return isNumber;
        }
        /// <summary>
        /// Metodo che dati un minimo e un massimo restituisce un numero random all'interno del range passato
        /// </summary>
        /// <param name="min">valore minimo del range</param>
        /// <param name="max">valore massimo del range</param>
        /// <returns>Int contente il valore random</returns>
        public Int32 GetRandomNumber(Int32 min, Int32 max)
		{
			Random random = new Random();
			return random.Next(min, max); 
		}
        /// <summary>
        /// Metodo che restituisce il carattere per separare i decimali da sistema
        /// </summary>
        /// <returns>carattere per indicare i decimali</returns>
        public String GetDecimaSimbol()
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentCulture;
            System.Globalization.NumberFormatInfo nfi = c.NumberFormat;
            String sDecimaSimbol = nfi.CurrencyDecimalSeparator;
            return sDecimaSimbol;
        }
        /// <summary>
        /// Metodo che restituisce il carattere per indicare i decimali
        /// </summary>
        /// <param name="valore">valore</param>
        /// <returns>restituisce il numero decimale sempre separato da "," anche in base al simbolo decimale di sistema</returns>
        public String NumberDecimal(String valore)
        {
            String sReturn = String.Empty;
            Double dvalore = 0;
            if (valore.Length > 0)
            {
                if (isNumber(valore))
                {
                    dvalore = Convert.ToDouble(valore);
                    dvalore = dvalore / 100;
                    int PosizioneVirgola = dvalore.ToString().IndexOf(GetDecimaSimbol());
                    if (PosizioneVirgola > 0)
                    {
                        Int32 Lunghezza = dvalore.ToString().Length - PosizioneVirgola - 1;
                        String Num_decimal = (dvalore.ToString().Substring(PosizioneVirgola + 1, Lunghezza)).PadRight(2, '0');
                        String Num_Intero = dvalore.ToString().Substring(0, PosizioneVirgola);
                        sReturn = Num_Intero + "," + Num_decimal;
                    }
                    else
                        sReturn = dvalore.ToString();
                }
            }

            return sReturn;
        }
		#endregion	     
    }
}
