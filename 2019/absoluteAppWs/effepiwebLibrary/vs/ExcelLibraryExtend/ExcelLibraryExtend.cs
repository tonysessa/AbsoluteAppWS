using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Reflection;

namespace ExcelLibraryExtend
{
    public class ExcelLibraryExtend
    {
        public ExcelLibraryExtend() { }

        ExcelLibrary.SpreadSheet.Workbook workbook = new ExcelLibrary.SpreadSheet.Workbook();
        ExcelLibrary.SpreadSheet.Worksheet worksheet = new ExcelLibrary.SpreadSheet.Worksheet("");

        public Color color;

        public string FilePathAndName = "ExcelLibraryExtendFile.xls";
        public string worksheetName = "Sheet1";
        public string headerBKGcolor = "#fffff";
        public string BKGcolor = "#fffff";

        public MemoryStream StreamXLS = new MemoryStream();

        bool bExist = false;

        #region XLS Function

        /// <summary>
        /// Metodo che inizializza il worksheet con l'intestazione di colonna
        /// </summary>
        /// <param name="_headerValues">string[] valori delle testate di colonna</param>
        /// <param name="_worksheetName">nome del worksheet da creare</param>
        /// <returns>true</returns>
        public bool InitSheet(string[] _headerValues, string _worksheetName)
        {
            //creo sheet
            worksheet = new ExcelLibrary.SpreadSheet.Worksheet(_worksheetName);
            //color = ColorTranslator.FromHtml(headerBKGcolor);

            //riempio header
            for (int i = 0; i < _headerValues.Length; i++)
            {
                worksheet.Cells[0, i] = new ExcelLibrary.SpreadSheet.Cell(_headerValues[i]);
                //  worksheet.Cells[0, i].Style.BackColor = color;
            }

            workbook.Worksheets.Add(worksheet);
            SetworksheetName(_worksheetName);

            return true;
        }


        /// <summary>
        /// Metodo che salva il file xls
        /// </summary>
        /// <param name="_filename">path completo in cui salvare il file</param>
        /// <returns>String contente il path del file</returns>
        public String SaveFile(String _filename = "")
        {
            if (_filename != "")
                FilePathAndName = _filename;

            String path = FilePathAndName;

            if (File.Exists(path))
            {
                File.Copy(path, FilePathAndName + "." + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + "_old_" + DateTime.Now.Millisecond.ToString());
            }

            workbook.Save(path);

            return path;
        }

        /// <summary>
        /// Metodo che restituisce uno byte array con xls
        /// </summary>
        /// <returns>Byte Array</returns>
        public byte[] SaveStream()
        {

            workbook.SaveToStream(StreamXLS);

            byte[] StreamXLSbyte = new byte[StreamXLS.Length];
            int count = StreamXLS.Read(StreamXLSbyte, 0, StreamXLSbyte.Length);
            StreamXLS.Seek(0, 0);

            while (count < StreamXLS.Length)
            {
                StreamXLSbyte[count++] =
                    Convert.ToByte(StreamXLS.ReadByte());
            }


            return StreamXLSbyte;

        }

        /// <summary>
        /// Metodo che restituisce uno stream con xls per il web
        /// </summary>
        /// <param name="_filename">nome del file</param>
        /// <returns>File</returns>
        public void SaveXlsToWeb(String _filename = "")
        {
            if (_filename != "")
                FilePathAndName = _filename;

            if (StreamXLS.Length < 6000)
            {
                workbook.Worksheets.Add(new ExcelLibrary.SpreadSheet.Worksheet("SheetLast"));
                int sNum = GetSheetNumber("SheetLast");

                for (var k = 0; k < 200; k++)
                {
                    workbook.Worksheets[sNum].Cells[k, 0] = new ExcelLibrary.SpreadSheet.Cell(null);
                }

            }

            workbook.SaveToStream(StreamXLS);

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.AppendHeader("Content-Disposition", "attachment; filename=" + FilePathAndName.Replace("\\\\", "\\"));
            response.AppendHeader("Content-Type", "binary/octet-stream");
            response.BinaryWrite(StreamXLS.ToArray());

        }


        /// <summary>
        /// Metodo che prepara il nuovo worksheet
        /// </summary>
        /// <param name="_worksheetName">nome dello sheet da inizializzare</param>
        /// <returns>null</returns>
        public void SetworksheetName(String _worksheetName)
        {

            int i = 0;

            for (i = 0; i < workbook.Worksheets.Count; i++)
            {
                if (workbook.Worksheets[i].Name == _worksheetName)
                {
                    worksheet = workbook.Worksheets[GetSheetNumber(_worksheetName)];
                }

            }
            if (i == 0)
            {
                workbook.Worksheets.Add(new ExcelLibrary.SpreadSheet.Worksheet(_worksheetName));
                worksheet = workbook.Worksheets[GetSheetNumber(_worksheetName)];
            }
            worksheetName = _worksheetName;
        }

        /// <summary>
        /// Metodo che dato un DataRow crea una riga nel worksheet
        /// </summary>
        /// <param name="_dr">DataRow</param>
        /// <param name="_worksheetName">nome dello sheet</param>
        /// <returns>true</returns>
        public bool AddRow(DataRow _dr, string _worksheetName = "")
        {
            int s = 0;
            int j = 0;

            if (_worksheetName != "")
            {
                SetworksheetName(_worksheetName);
            }
            else
            {
                _worksheetName = "";
            }

            if (worksheetName != "")
                s = GetSheetNumber(worksheetName);

            if (_worksheetName == "" && worksheetName == "")
                return false;

            if (s != -1)
                j = workbook.Worksheets[s].Cells.LastRowIndex + 1;
            else
                return false;

            object[] _values = _dr.ItemArray;

            for (int i = 0; i < _values.Length; i++)
            {

                worksheet.Cells[j, i] = new ExcelLibrary.SpreadSheet.Cell(_values[i]);

            }

            return true;
        }

        /// <summary>
        /// Metodo che dato un string[] crea una riga nel worksheet
        /// </summary>
        /// <param name="_values">string[]</param>
        /// <param name="_worksheetName">nome dello sheet</param>
        /// <returns>true</returns>
        public bool AddRow(string[] _values, string _worksheetName = "")
        {
            int s = 0;
            int j = 0;

            if (_worksheetName != "")
            {
                SetworksheetName(_worksheetName);
            }
            else
            {
                _worksheetName = "";
            }

            if (worksheetName != "")
                s = GetSheetNumber(worksheetName);

            if (_worksheetName == "" && worksheetName == "")
                return false;

            if (s != -1)
                j = workbook.Worksheets[s].Cells.LastRowIndex + 1;
            else
                return false;

            for (int i = 0; i < _values.Length; i++)
            {

                worksheet.Cells[j, i] = new ExcelLibrary.SpreadSheet.Cell(_values[i]);

            }

            return true;
        }

        /// <summary>
        /// Metodo che dato un string[] crea una riga nel worksheet
        /// </summary>
        /// <param name="_values">object[]</param>
        /// <param name="_worksheetName">nome dello sheet</param>
        /// <returns>true</returns>
        public bool AddRow(object[] _values, string _worksheetName = "")
        {
            int s = 0;
            int j = 0;

            if (_worksheetName != "")
            {
                SetworksheetName(_worksheetName);
            }
            else
            {
                _worksheetName = "";
            }

            if (worksheetName != "")
                s = GetSheetNumber(worksheetName);

            if (_worksheetName == "" && worksheetName == "")
                return false;

            if (s != -1)
                j = workbook.Worksheets[s].Cells.LastRowIndex + 1;
            else
                return false;

            for (int i = 0; i < _values.Length; i++)
            {

                worksheet.Cells[j, i] = new ExcelLibrary.SpreadSheet.Cell(_values[i].ToString());

            }

            return true;
        }

        /// <summary>
        /// Metodo che dato un datatable crea il worksheet
        /// </summary>
        /// <param name="_dt">DataTable</param>
        /// <param name="_worksheetName">nome dello sheet</param>
        /// <param name="_append">Append</param>
        /// <returns>true</returns>
        public bool AddRows(DataTable _dt, string _worksheetName = "", bool _append = false)
        {
            int s = 0;
            int j = 0;

            if (_worksheetName != "")
            {
                SetworksheetName(_worksheetName);
            }
            else
            {
                _worksheetName = "";
            }

            if (worksheetName != "")
                s = GetSheetNumber(worksheetName);

            if (_worksheetName == "" && worksheetName == "")
                return false;

            if (s != -1 || _append)
                j = workbook.Worksheets[s].Cells.LastRowIndex + 1;
            else
                return false;

            int k = 0;

            for (k = j; k < _dt.Rows.Count; k++)
            {
                for (int i = 0; i < _dt.Columns.Count; i++)
                {

                    worksheet.Cells[k, i] = new ExcelLibrary.SpreadSheet.Cell(_dt.Rows[k][i].ToString());

                }
            }

            return true;
        }


        /// <summary>
        /// Metodo che dato un string[] aggiorna una riga nel worksheet
        /// </summary>
        /// <param name="_values">string[]</param>
        /// <param name="_row">numero della riga</param>
        /// <param name="_worksheetName">nome dello sheet</param>
        /// <returns>true</returns>
        public bool UpdateRow(string[] _values, int _row, string _worksheetName = "")
        {
            int s = 0;
            int j = 0;

            if (_worksheetName != "")
            {
                SetworksheetName(_worksheetName);
            }
            else
            {
                _worksheetName = "";
            }

            if (worksheetName != "")
                s = GetSheetNumber(worksheetName);

            if (_worksheetName == "" && worksheetName == "")
                return false;

            if (s != -1)
                j = _row;
            else
                return false;

            for (int i = 0; i < _values.Length; i++)
            {

                worksheet.Cells[j, i] = new ExcelLibrary.SpreadSheet.Cell(_values[i]);

            }

            return true;
        }



        /// <summary>
        /// Metodo che data una lista di oggetti crea il worksheet con anche l'header
        /// </summary>
        /// <param name="_list">List of object of string</param>
        /// <param name="_worksheetName">nome dello sheet</param>
        /// <param name="_append">Append</param>
        /// <returns>true</returns>
        public bool InitAndAddRowsObject(List<object> _list, string _worksheetName = "", bool _append = false)
        {
            int i = 0;

            foreach (object obj in _list)
            {
                if (obj == null)
                    continue;
                //
                var values = enumerazioneCampi(obj);

                string[] strings = values.Item2;

                if (i == 0)
                {
                    string[] names = values.Item1;
                    InitSheet(names, _worksheetName);
                }

                AddRow(strings, _worksheetName);
                i++;
            }

            return true;
        }

        /// <summary>
        /// Metodo che data una lista di oggetti e le colonne da utilizzare crea il worksheet con anche l'header nell'ordine delle colonne
        /// </summary>
        /// <param name="_list">List of object of string</param>
        /// <param name="_worksheetName">nome dello sheet</param>
        /// <param name="_append">Append</param>
        /// <returns>true</returns>
        public bool InitAndAddRowsObject(List<object> _list, string[] _columns, string _worksheetName = "", bool _append = false)
        {
            int i = 0;

            foreach (object obj in _list)
            {
                if (obj == null)
                    continue;
                //
                var values = enumerazioneCampi(obj, _columns);

                string[] strings = values.Item2;

                if (i == 0)
                {
                    string[] names = values.Item1;

                    InitSheet(names, _worksheetName);
                }

                AddRow(strings, _worksheetName);
                i++;
            }

            return true;
        }

        /// <summary>
        /// Metodo che data una lista di oggetti crea il worksheet
        /// </summary>
        /// <param name="_list">List of object of string</param>
        /// <param name="_worksheetName">nome dello sheet</param>
        /// <param name="_append">Append</param>
        /// <returns>true</returns>
        public bool AddRowsObject(List<object> _list, string _worksheetName = "", bool _append = false)
        {
            foreach (object obj in _list)
            {
                if (obj == null)
                    continue;
                //
                var values = enumerazioneCampi(obj);

                string[] strings = values.Item2;

                AddRow(strings, _worksheetName);
            }

            return true;
        }

        /// <summary>
        /// Metodo che apre un workbook esistente
        /// </summary>
        /// <param name="_filename">path del file Excel</param>
        /// <returns>true</returns>
        public ExcelLibrary.SpreadSheet.Workbook Open(string _filename = "")
        {
            if (!String.IsNullOrEmpty(_filename))
                FilePathAndName = _filename;

            workbook = ExcelLibrary.SpreadSheet.Workbook.Load(FilePathAndName);

            return workbook;
        }


        /// <summary>
        /// Metodo che restituisce una array con i nomi dei sheet
        /// </summary>
        /// <param name="_workbook">Workbook</param>
        /// <returns>string[]</returns>
        public string[] GetSheetNames(ExcelLibrary.SpreadSheet.Workbook _workbook)
        {

            workbook = _workbook;

            string[] SheetNames = new string[workbook.Worksheets.Count];

            for (int s = 0; s < workbook.Worksheets.Count; s++)
            {
                SheetNames[s] = workbook.Worksheets[s].Name;
            }

            return SheetNames;
        }

        /// <summary>
        /// Metodo che restituisce il numero di sheet presenti in un workbook
        /// </summary>
        /// <param name="_workbook">Workbook</param>
        /// <returns>int</returns>
        public int GetTotalSheet(ExcelLibrary.SpreadSheet.Workbook _workbook)
        {
            workbook = _workbook;

            return workbook.Worksheets.Count;
        }


        /// <summary>
        /// Populate all data (all converted into String) in all worksheets 
        /// from a given Excel workbook.
        /// </summary>
        /// <param name="_filename">File path of the Excel workbook</param>
        /// <returns>DataSet with all worksheet populate into DataTable</returns>
        public DataSet CreateDataSet(string _filename = "")
        {
            if (!String.IsNullOrEmpty(_filename))
                FilePathAndName = _filename;

            DataSet ds = ExcelLibrary.DataSetHelper.CreateDataSet(FilePathAndName);

            return ds;
        }

        /// <summary>
        /// Populate data (all converted into String) from a given Excel 
        /// workbook and also work sheet name into a new instance of DataTable.
        /// Returns null if given work sheet is not found. ATTENZIONE: LA PRIMA RIGA DEVE CONTENERE I NOMI DI COLONNA!
        /// </summary>
        /// <param name="_filename">File path of the Excel workbook</param>
        /// <param name="_sheetName">Worksheet name in workbook</param>
        /// <returns>DataTable with populate data</returns>
        public DataTable CreateDataTable(String _filename, String _sheetName)
        {
            if (!String.IsNullOrEmpty(_filename))
                FilePathAndName = _filename;

            if (!String.IsNullOrEmpty(_sheetName))
                worksheetName = _sheetName;

            DataTable dt = ExcelLibrary.DataSetHelper.CreateDataTable(FilePathAndName, worksheetName);

            return dt;
        }


        /// <summary>
        /// solo di test!!!!
        /// </summary>
        /// <param name="_filename">File path of the Excel workbook</param>
        /// <param name="_sheetName">Worksheet name in workbook</param>
        /// <returns>DataTable with populate data</returns>
        public string[] GetCellType(String _filename, String _sheetName)
        {
            if (!String.IsNullOrEmpty(_filename))
                FilePathAndName = _filename;

            if (!String.IsNullOrEmpty(_sheetName))
                worksheetName = _sheetName;

            string[] cellType = new string[3];

            workbook = Open(FilePathAndName);

            for (int i = 0; i < workbook.Worksheets[0].Cells.LastColIndex + 1; i++)
            {
                cellType[i] = workbook.Worksheets[0].Cells[0, i].Format.FormatString.ToString();
                if (String.IsNullOrEmpty(cellType[i]))
                    cellType[i] = "||";
            }
            return cellType;
        }


        private Tuple<string[], string[]> enumerazioneCampi(object obj)
        {

            List<string> names = new List<string>();
            List<string> values = new List<string>();

            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(ø => ø.CanRead);
            // .Where(ø => ø.PropertyType == typeof(string));

            foreach (var p in properties)
            {
                //p.Name
                object value = p.GetValue(obj, null);

                names.Add(p.Name == null ? "" : p.Name);
                values.Add(value == null ? "" : value.ToString());
            }

            //
            var members = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            // .Where(ø => ø.FieldType == typeof(string));
            foreach (var p in members)
            {
                //p.Name
                object value = p.GetValue(obj);

                names.Add(p.Name == null ? "" : p.Name);
                values.Add(value == null ? "" : value.ToString());
            }

            return Tuple.Create(names.ToArray(), values.ToArray()); ;
        }

        private Tuple<string[], string[]> enumerazioneCampi(object obj, string[] _columns)
        {

            List<string> names = new List<string>();
            List<string> values = new List<string>();
            List<string> names2 = new List<string>();
            List<string> values2 = new List<string>();

            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(ø => ø.CanRead);
            // .Where(ø => ø.PropertyType == typeof(string));

            foreach (var p in properties)
            {
                //p.Name                
                if (Array.IndexOf(_columns, p.Name) >= 0)
                {
                    object value = p.GetValue(obj, null);
                    names.Add(p.Name == null ? "" : p.Name);
                    values.Add(value == null ? "" : value.ToString());
                }
            }

            //
            var members = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            // .Where(ø => ø.FieldType == typeof(string));
            foreach (var p in members)
            {
                object value = p.GetValue(obj);
                if (Array.IndexOf(_columns, p.Name) >= 0)
                {
                    names.Add(p.Name == null ? "" : p.Name);
                    values.Add(value == null ? "" : value.ToString());
                }
            }
            int j = 0;

            for (int i = 0; i < _columns.Length; i++)
            {
                String colonna = _columns[i];
                int indice = names.IndexOf(colonna);
                if (indice >= 0)
                {
                    names2.Insert(j, names[indice]);
                    values2.Insert(j, values[indice]);
                }
                else //skip campo non trovato e riposiziono indice degli array names2 e values2 per non lasciare buchi
                {
                    j = j - 1;
                }
                j++;
            }

            return Tuple.Create(names2.ToArray(), values2.ToArray());
        }

        public int GetSheetNumber(string _worksheetName)
        {
            int s = 0;
            bool bOK = true;

            for (s = 0; s < workbook.Worksheets.Count; s++)
            {
                if (workbook.Worksheets[s].Name == _worksheetName)
                {
                    bExist = true;
                    bOK = false;
                    break;
                }
                else
                {
                    bOK = true;
                }
            }

            if (bOK)
                return -1;
            else
            {
                worksheet = workbook.Worksheets[s];
                return s;
            }
        }


        #endregion
    }
}
