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
	public class DateUtil
	{
        public DateUtil() { }

        #region Date Function
        /// <summary>
        /// Metodo che verifica che la data sia valida
        /// </summary>
        /// <param name="pDateValue">stringa data passata</param>
        /// <returns>Bolean</returns>
        public Boolean isValidDate(DateTime Data)
        {
            try
            {
                DateTime testar = Convert.ToDateTime(Data);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        /// <summary>
        /// Metodo che verifica che la stringa sia una data corretta con Format Provider it-IT
        /// </summary>
        /// <param name="pDateValue">stringa data passata</param>
        /// <returns>Bolean</returns>
        public Boolean isValidDate(String pDateValue)
        {

            System.IFormatProvider format = new System.Globalization.CultureInfo("it-IT", true);
            return isValidDate(pDateValue, format);

        }
        /// <summary>
        /// Metodo che verifica che la stringa sia una data corretta con Format Provider passato
        /// </summary>
        /// <param name="pDateValue">stringa data passata</param>
        /// <param name="sGiorno">Format Provider {it-IT, en-EN}</param>
        /// <returns>Bolean</returns>
        public Boolean isValidDate(String pDateValue, System.IFormatProvider format)
        {
            System.DateTime dt_CONVERT = new DateTime();

            if (pDateValue.Length > 0)
            {
                try
                {
                    dt_CONVERT = System.DateTime.Parse(pDateValue, format, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    return true;
                }
                catch (System.Exception e)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Metodo che data una stringa restituisce il giorno in letter
        /// </summary>
        /// <param name="sGiorno">stringa passata con numero del giorno</param>
        /// <returns>String con numero del giorno in lettere</returns>
        public String DayNumberWordENG(String sGiorno)
        {

            String sGiorno_word = "";

            if (sGiorno.Length < 2)
                sGiorno = "0" + sGiorno;

            if (sGiorno == "01")
                sGiorno_word = "ONE'st";
            else if (sGiorno == "02")
                sGiorno_word = "TWO'nd";
            else if (sGiorno == "03")
                sGiorno_word = "THREE'rd";
            else if (sGiorno == "04")
                sGiorno_word = "FOUR'th";
            else if (sGiorno == "05")
                sGiorno_word = "FIVE'th";
            else if (sGiorno == "06")
                sGiorno_word = "SIX'th";
            else if (sGiorno == "07")
                sGiorno_word = "SEVEN'th";
            else if (sGiorno == "08")
                sGiorno_word = "EIGHT'th";
            else if (sGiorno == "09")
                sGiorno_word = "NINE'th";
            else if (sGiorno == "10")
                sGiorno_word = "TEN'th";
            else if (sGiorno == "11")
                sGiorno_word = "ELEVEN'th";
            else if (sGiorno == "12")
                sGiorno_word = "TWELVE'th";
            else if (sGiorno == "13")
                sGiorno_word = "THIRTEEN'th";
            else if (sGiorno == "14")
                sGiorno_word = "FOURTEEN'th";
            else if (sGiorno == "15")
                sGiorno_word = "FIVETEEN'th";
            else if (sGiorno == "16")
                sGiorno_word = "SIXTEEN'th";
            else if (sGiorno == "17")
                sGiorno_word = "SEVENTEEN'th";
            else if (sGiorno == "18")
                sGiorno_word = "EIGHTEEN'th";
            else if (sGiorno == "19")
                sGiorno_word = "NINETEEN'th";
            else if (sGiorno == "20")
                sGiorno_word = "TWENTIE'th";
            else if (sGiorno == "21")
                sGiorno_word = "TWENTYONE'st";
            else if (sGiorno == "22")
                sGiorno_word = "TWENTYTWO'nd";
            else if (sGiorno == "23")
                sGiorno_word = "TWENTYTHREE'rd";
            else if (sGiorno == "24")
                sGiorno_word = "TWENTYFOUR'th";
            else if (sGiorno == "25")
                sGiorno_word = "TWENTYFIVE'th";
            else if (sGiorno == "26")
                sGiorno_word = "TWENTYSIX'th";
            else if (sGiorno == "27")
                sGiorno_word = "TWENTYSEVEN'th";
            else if (sGiorno == "28")
                sGiorno_word = "TWENTYEIGHT'th";
            else if (sGiorno == "29")
                sGiorno_word = "TWENTYNINE'th";
            else if (sGiorno == "30")
                sGiorno_word = "THIRTIE'th";
            else if (sGiorno == "31")
                sGiorno_word = "THIRTIETHONE'st";
            else
                sGiorno_word = "";

            sGiorno_word = sGiorno_word.Replace(" ", "");
            return sGiorno_word;
        }
        /// <summary>
        /// Metodo che data una stringa restituisce il giorno in letter
        /// </summary>
        /// <param name="sGiorno">stringa passata con numero del giorno</param>
        /// <returns>String con numero del giorno in lettere</returns>
        public String DayNumberWordITA(String sGiorno)
        {
            String sGiorno_word = "";

            if (sGiorno.Length < 2)
                sGiorno = "0" + sGiorno;

            if (sGiorno == "01")
                sGiorno_word = "PRIMO";
            else if (sGiorno == "02")
                sGiorno_word = "DUE";
            else if (sGiorno == "03")
                sGiorno_word = "TRE";
            else if (sGiorno == "04")
                sGiorno_word = "QUATTRO";
            else if (sGiorno == "05")
                sGiorno_word = "CINQUE";
            else if (sGiorno == "06")
                sGiorno_word = "SEI";
            else if (sGiorno == "07")
                sGiorno_word = "SETTE";
            else if (sGiorno == "08")
                sGiorno_word = "OTTO";
            else if (sGiorno == "09")
                sGiorno_word = "NOVE";
            else if (sGiorno == "10")
                sGiorno_word = "DIECI";
            else if (sGiorno == "11")
                sGiorno_word = "UNDICI";
            else if (sGiorno == "12")
                sGiorno_word = "DODICI";
            else if (sGiorno == "13")
                sGiorno_word = "TREDICI";
            else if (sGiorno == "14")
                sGiorno_word = "QUATTORDICI";
            else if (sGiorno == "15")
                sGiorno_word = "QUINDICI";
            else if (sGiorno == "16")
                sGiorno_word = "SEDICI";
            else if (sGiorno == "17")
                sGiorno_word = "DICIASETTE";
            else if (sGiorno == "18")
                sGiorno_word = "DICIOTTO";
            else if (sGiorno == "19")
                sGiorno_word = "DICIANNOVE";
            else if (sGiorno == "20")
                sGiorno_word = "VENTI";
            else if (sGiorno == "21")
                sGiorno_word = "VENTUNO";
            else if (sGiorno == "22")
                sGiorno_word = "VENTIDUE";
            else if (sGiorno == "23")
                sGiorno_word = "VENTITRE";
            else if (sGiorno == "24")
                sGiorno_word = "VENTIQUATTRO";
            else if (sGiorno == "25")
                sGiorno_word = "VENTICINQUE";
            else if (sGiorno == "26")
                sGiorno_word = "VENTISEI";
            else if (sGiorno == "27")
                sGiorno_word = "VENTISETTE";
            else if (sGiorno == "28")
                sGiorno_word = "VENTOTTO";
            else if (sGiorno == "29")
                sGiorno_word = "VENTINOVE";
            else if (sGiorno == "30")
                sGiorno_word = "TRENTA";
            else if (sGiorno == "31")
                sGiorno_word = "TRENTUNO";
            else
                sGiorno_word = "";

            return sGiorno_word;
        }
        /// <summary>
        /// Metodo che data una stringa restituisce il nome del mese
        /// </summary>
        /// <param name="sMese">stringa passata con numero del mese</param>
        /// <returns>String con nome del mese</returns>
		public String MonthWordITA(String sMese)
		{
			String sMese_word = "";

			if (sMese.Length <2)
				sMese = "0" + sMese;

			if (sMese == "01")
				sMese_word = "Gennaio";
			else if (sMese == "02")
				sMese_word = "Febbraio";
			else if (sMese == "03")
				sMese_word = "Marzo";
			else if (sMese == "04")
				sMese_word = "Aprile";
			else if (sMese == "05")
				sMese_word = "Maggio";
			else if (sMese == "06")
				sMese_word = "Giugno";
			else if (sMese == "07")
				sMese_word = "Luglio";
			else if (sMese == "08")
				sMese_word = "Agosto";
			else if (sMese == "09")
				sMese_word = "Settembre";
			else if (sMese == "10")
				sMese_word = "Ottobre";
			else if (sMese == "11")
				sMese_word = "Novembre";
			else if (sMese == "12")
				sMese_word = "Dicembre";
			else
				sMese_word = "";				

			return sMese_word;
		}
        /// <summary>
        /// Metodo che data una stringa restituisce il nome del mese
        /// </summary>
        /// <param name="sMese">stringa passata con numero del mese</param>
        /// <returns>String con nome del mese</returns>
		public String MonthWordENG(String sMese)
		{
			String sMese_word = "";

			if (sMese.Length <2)
				sMese = "0" + sMese;

			if (sMese == "01")
				sMese_word = "January";
			else if (sMese == "02")
				sMese_word = "February";
			else if (sMese == "03")
				sMese_word = "March";
			else if (sMese == "04")
				sMese_word = "April";
			else if (sMese == "05")
				sMese_word = "May";
			else if (sMese == "06")
				sMese_word = "June";
			else if (sMese == "07")
				sMese_word = "July";
			else if (sMese == "08")
				sMese_word = "August";
			else if (sMese == "09")
				sMese_word = "September";
			else if (sMese == "10")
				sMese_word = "October";
			else if (sMese == "11")
				sMese_word = "November";
			else if (sMese == "12")
				sMese_word = "December";
			else
				sMese_word = "";				

			return sMese_word;
		}
        /// <summary>
        /// Metodo che data una stringa restituisce il nome del mese
        /// </summary>
        /// <param name="sMese">stringa passata con numero del mese</param>
        /// <returns>String con nome del mese</returns>
        public String MonthWordESP(String sMese)
		{
			String sMese_word = "";

			if (sMese.Length <2)
				sMese = "0" + sMese;
                            
			if (sMese == "01")
				sMese_word = "Enero";
			else if (sMese == "02")
				sMese_word = "Febrero";
			else if (sMese == "03")
				sMese_word = "Marzo";
			else if (sMese == "04")
				sMese_word = "Abril";
			else if (sMese == "05")
				sMese_word = "Mayo";
			else if (sMese == "06")
				sMese_word = "Junio";
			else if (sMese == "07")
				sMese_word = "Julio";
			else if (sMese == "08")
				sMese_word = "Agosto";
			else if (sMese == "09")
				sMese_word = "Septiembre";
			else if (sMese == "10")
				sMese_word = "Octubre";
			else if (sMese == "11")
				sMese_word = "Noviembre";
			else if (sMese == "12")
                sMese_word = "Diciembre";
			else
				sMese_word = "";				

			return sMese_word;
		}
        /// <summary>
        /// Metodo che data una stringa restituisce il nome del mese
        /// </summary>
        /// <param name="d">data passata </param>
        /// <returns>String con data nel formato dd nomeMese anno</returns>
        static String GetMonthName(Int32 monthNum)
        {
            return GetMonthName(monthNum, false);
        }
        /// <summary>
        /// Metodo che data una stringa restituisce il nome del mese
        /// </summary>
        /// <param name="d">data passata </param>
        /// <returns>String con data nel formato dd nomeMeseBreve anno</returns>
        static String GetMonthName(Int32 monthNum, Boolean abbreviate)
        {
            if (monthNum < 1 || monthNum > 12)
                throw new ArgumentOutOfRangeException("monthNum");
            DateTime date = new DateTime(1, monthNum, 1);
            if (abbreviate)
                return date.ToString("MMM");
            else
                return date.ToString("MMMM");
        }
        /// <summary>
        /// Metodo che data una stringa restituisce il nome del mese
        /// </summary>
        /// <param name="d">data passata </param>
        /// <returns>String con data nel formato dd nomeMese aaaa</returns>
		public String DateToWordIta(DateTime d)
		{
			return d.Day.ToString() + " " + MonthWordITA(d.Month.ToString()) + " " + d.Year.ToString();
		}
        /// <summary>
        /// Metodo che data una stringa restituisce il nome del mese
        /// </summary>
        /// <param name="d">data passata </param>
        /// <returns>String con data nel formato dd nomeMese yyyy</returns>
        public String DateToWordEng(DateTime d)
        {
            return d.Day.ToString() + " " + MonthWordENG(d.Month.ToString()) + " " + d.Year.ToString();
        }
        /// <summary>
        /// Metodo che data una stringa restituisce il nome del mese
        /// </summary>
        /// <param name="d">data passata </param>
        /// <returns>String con data nel formato dd nomeMese yyyy</returns>
        public String DateToWordEsp(DateTime d)
        {
            return d.Day.ToString() + " " + MonthWordESP(d.Month.ToString()) + " " + d.Year.ToString();
        }
        /// <summary>
        /// Metodo che data una data restituisce la data nel formato richiesto
        /// </summary>
        /// <param name="d">data passata </param>
        /// <param name="sFormat">Formato della data passata {GMA, MGA, MAG, AMG, AGM}</param>
        /// <param name="sDateSeparator">Carattere separatore della data da restituire</param>
        /// <returns>String con data nel formato richiesto</returns>
        public String DateToString(DateTime d, String sFormat, String sDateSeparator)
        {
            String sDateReturn = String.Empty;

            if (sFormat == "GMA")
                sDateReturn = d.Day.ToString().PadLeft(2, '0') + sDateSeparator + d.Month.ToString().PadLeft(2, '0') + sDateSeparator + d.Year.ToString();
            else if (sFormat == "MGA")
                sDateReturn = d.Month.ToString().PadLeft(2, '0') + sDateSeparator + d.Day.ToString().PadLeft(2, '0') + sDateSeparator + d.Year.ToString();
            else if (sFormat == "MAG")
                sDateReturn = d.Month.ToString().PadLeft(2, '0') + sDateSeparator + d.Year.ToString() + sDateSeparator + d.Day.ToString().PadLeft(2, '0');
            else if (sFormat == "AMG")
                sDateReturn = d.Year.ToString() + sDateSeparator + d.Month.ToString().PadLeft(2, '0') + sDateSeparator + d.Day.ToString().PadLeft(2, '0');
            else if (sFormat == "AGM")
                sDateReturn = d.Year.ToString() + sDateSeparator + d.Day.ToString().PadLeft(2, '0') + sDateSeparator + d.Month.ToString().PadLeft(2, '0');

            return sDateReturn;
        }
        /// <summary>
        /// Metodo che data una data restituisce la data nel formato richiesto
        /// </summary>
        /// <param name="d">data passata </param>
        /// <param name="sFormat">Formato della data passata {GMA, MGA, MAG, AMG, AGM}</param>
        /// <param name="sDateSeparator">Carattere separatore della data da restituire</param>
        //// <param name="sTimeSeparator">Carattere separatore dell'ora da restituire</param>
        /// <returns>String con data nel formato richiesto con l'ora</returns>
        public String DateTimeToString(DateTime d, String sFormat, String sDateSeparator, String sTimeSeparator)
        {
            String sDateReturn = String.Empty;

            if (sFormat == "GMA")
                sDateReturn = d.Day.ToString().PadLeft(2, '0') + sDateSeparator + d.Month.ToString().PadLeft(2, '0') + sDateSeparator + d.Year.ToString();
            else if (sFormat == "MGA")
                sDateReturn = d.Month.ToString().PadLeft(2, '0') + sDateSeparator + d.Day.ToString().PadLeft(2, '0') + sDateSeparator + d.Year.ToString();
            else if (sFormat == "MAG")
                sDateReturn = d.Month.ToString().PadLeft(2, '0') + sDateSeparator + d.Year.ToString() + sDateSeparator + d.Day.ToString().PadLeft(2, '0');
            else if (sFormat == "AMG")
                sDateReturn = d.Year.ToString() + sDateSeparator + d.Month.ToString().PadLeft(2, '0') + sDateSeparator + d.Day.ToString().PadLeft(2, '0');
            else if (sFormat == "AGM")
                sDateReturn = d.Year.ToString() + sDateSeparator + d.Day.ToString().PadLeft(2, '0') + sDateSeparator + d.Month.ToString().PadLeft(2, '0');

            sDateReturn += " " + d.Hour.ToString().PadLeft(2, '0') + sTimeSeparator + d.Minute.ToString().PadLeft(2, '0') + sTimeSeparator + d.Second.ToString().PadLeft(2, '0');

            return sDateReturn;
        }
        /// <summary>
        /// Metodo che data una stringa restituisce la data
        /// </summary>
        /// <param name="sData">data passata in formato stringa</param>
        /// <param name="sFormat">Formato della data passata {GMA, MGA, MAG, AMG, AGM}</param>
        /// <param name="sSeparator">Carattere separatore della data passata </param>
        /// <returns>Data</returns>
        public DateTime StringToDate(String sData, String sFormat, String sSeparator)
        {
            DateTime dDateReturn = new DateTime();
            char[] chSeparator = sSeparator.ToCharArray(0, 1);
            String[] myDateArray = sData.Split(chSeparator[0]);

            try
            {
                if (sFormat == "GMA")
                    dDateReturn = System.DateTime.Parse(myDateArray[2] + "-" + myDateArray[1] + "-" + myDateArray[0]);
                else if (sFormat == "MGA")
                    dDateReturn = System.DateTime.Parse(myDateArray[2] + "-" + myDateArray[0] + "-" + myDateArray[1]);
                else if (sFormat == "MAG")
                    dDateReturn = System.DateTime.Parse(myDateArray[1] + "-" + myDateArray[0] + "-" + myDateArray[2]);
                else if (sFormat == "AMG")
                    dDateReturn = System.DateTime.Parse(myDateArray[0] + "-" + myDateArray[1] + "-" + myDateArray[2]);
                else if (sFormat == "AGM")
                    dDateReturn = System.DateTime.Parse(myDateArray[0] + "-" + myDateArray[2] + "-" + myDateArray[1]);
            }
            catch (FormatException)
            {
                dDateReturn = System.DateTime.MinValue;
            }
            return dDateReturn;
        }
        /// <summary>
        /// Metodo che data una stringa restituisce la data e l'ora
        /// </summary>
        /// <param name="sData">data passata in formato stringa</param>
        /// <param name="sFormat">Formato della data passata {GMA, MGA, MAG, AMG, AGM}</param>
        /// <param name="sSeparator">Carattere separatore tra data e ora passata</param>
        /// <param name="sDateSeparator">Carattere separatore della data passata</param>
        /// <param name="sTimeSeparator">Carattere separatore dell'ora passata</param>
        /// <returns>Data e ora</returns>
        public DateTime StringToDateTime(String sData, String sFormat, String sSeparator, String sDateSeparator, String sTimeSeparator)
        {
            DateTime dDateReturn = new DateTime();
            char[] chSeparator = sSeparator.ToCharArray(0, 1);
            char[] chTimeSeparator = sTimeSeparator.ToCharArray(0, 1);
            char[] chDateSeparator = sDateSeparator.ToCharArray(0, 1);

            String[] myArray = sData.Split(chSeparator[0]);
            String[] myDateArray = myArray[0].Split(chDateSeparator[0]);
            String[] myTimeArray = myArray[1].Split(chTimeSeparator[0]);

            try
            {
                if (sFormat == "GMA")
                    dDateReturn = new DateTime(Convert.ToInt16(myDateArray[2]),  Convert.ToInt16(myDateArray[1]), Convert.ToInt16(myDateArray[0]) ,  Convert.ToInt16(myTimeArray[0]),Convert.ToInt16(myTimeArray[1]),Convert.ToInt16(myTimeArray[2]));
                else if (sFormat == "MGA")
                    dDateReturn = new DateTime(Convert.ToInt16(myDateArray[2]),  Convert.ToInt16(myDateArray[0]), Convert.ToInt16(myDateArray[1]) ,  Convert.ToInt16(myTimeArray[0]),Convert.ToInt16(myTimeArray[1]),Convert.ToInt16(myTimeArray[2]));
                else if (sFormat == "MAG")
                    dDateReturn = new DateTime(Convert.ToInt16(myDateArray[1]),  Convert.ToInt16(myDateArray[0]), Convert.ToInt16(myDateArray[2]) ,  Convert.ToInt16(myTimeArray[0]),Convert.ToInt16(myTimeArray[1]),Convert.ToInt16(myTimeArray[2]));
                else if (sFormat == "AMG")                    
                    dDateReturn = new DateTime(Convert.ToInt16(myDateArray[0]),  Convert.ToInt16(myDateArray[1]), Convert.ToInt16(myDateArray[2]) ,  Convert.ToInt16(myTimeArray[0]),Convert.ToInt16(myTimeArray[1]),Convert.ToInt16(myTimeArray[2]));
                else if (sFormat == "AGM")
                    dDateReturn = new DateTime(Convert.ToInt16(myDateArray[0]), Convert.ToInt16(myDateArray[2]), Convert.ToInt16(myDateArray[1]), Convert.ToInt16(myTimeArray[0]), Convert.ToInt16(myTimeArray[1]), Convert.ToInt16(myTimeArray[2]));
            }
            catch (FormatException)
            {
                dDateReturn = System.DateTime.MinValue;
            }
            return dDateReturn;
        }
		#endregion      
    }
}
