using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace Support.Library
{
    public class FileUtil
	{
        public FileUtil() { }

        #region fileUtil Function

        /// <summary>
        /// Metodo che restituisce la dimensione del file in KB o MB se superiore al mega
        /// </summary>
        /// <param name="_path">path del file</param>
        /// <returns>restituisce la dimensione del file</returns>
        public String GetFileDimension(String _path)
        {
            String sReturn = String.Empty;
            float fLengthKB = 0;
            float fLengthMB = 0;
            if (!String.IsNullOrEmpty(_path))
            {
                FileInfo fleMembers = new FileInfo(_path);
                if (fleMembers.Exists)
                {
                    fLengthKB = (float)(fleMembers.Length / 1024);
                    fLengthMB = (float)(fleMembers.Length / 1024 / 1024);
                    if (fLengthMB > 0)
                        sReturn = fLengthMB + " MB";
                    else
                        sReturn = fLengthKB + " KB";
                }
            }

            return sReturn;
        }

        /// <summary>
        /// Metodo che restituisce il nome del file normalizzato
        /// </summary>
        /// <param name="_path">path del file</param>
        /// <returns>restituisce la dimensione del file</returns>
        public String ClearFileName(String filename)
        {
            String sReturn = String.Empty;
            if (filename.Length > 0)
            {
                String replacement = "_";
                sReturn = Regex.Replace(filename, @"[^\w\.-]", replacement);
                sReturn = sReturn.Replace("à", "a");
                sReturn = sReturn.Replace("è", "e");
                sReturn = sReturn.Replace("i", "i");
                sReturn = sReturn.Replace("ò", "o");
                sReturn = sReturn.Replace("ù", "u");
            }
            else
                sReturn = "";

            return sReturn;
        }

        /// <summary>
        /// Metodo che restituisce il nome del file normalizzato
        /// </summary>
        /// <param name="_path">path del file</param>
        /// <returns>restituisce la dimensione del file</returns>
        public void CompressFolder(String _pathStorage, List<String> _listFiles, ZipOutputStream zipStream)
        {
            try
            {
                foreach (string filename in _listFiles)
                {
                    String sFileName = _pathStorage + filename.Replace("/", "\\");
                    FileInfo fi = new FileInfo(sFileName);

                    string entryName = sFileName.Substring(sFileName.LastIndexOf("\\"), sFileName.Length - sFileName.LastIndexOf("\\"));
                    entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                    ZipEntry newEntry = new ZipEntry(entryName);
                    newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                    // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                    // A password on the ZipOutputStream is required if using AES.
                    //   newEntry.AESKeySize = 256;

                    // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                    // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                    // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                    // but the zip will be in Zip64 format which not all utilities can understand.
                    //   zipStream.UseZip64 = UseZip64.Off;
                    newEntry.Size = fi.Length;

                    zipStream.PutNextEntry(newEntry);

                    // Zip the file in buffered chunks
                    // the "using" will close the stream even if an exception occurs
                    byte[] buffer = new byte[4096];
                    using (FileStream streamReader = File.OpenRead(sFileName))
                    {
                        StreamUtils.Copy(streamReader, zipStream, buffer);
                    }
                    zipStream.CloseEntry();
                }
            }
            catch(Exception ex){
            }
            

        }

        /// <summary>
        /// Metodo che restituisce il nome del file rinominato
        /// </summary>
        /// <param name="_path">path del file</param>
        /// <returns>restituisce la dimensione del file</returns>
        public String GetNewFileIfExist(String _path, String _fileName)
        {
            String sFileNameTemp = String.Empty;
            String sFileNameExtension = String.Empty;
            String[] sSplit;

            sSplit = _fileName.Split('\\');
            _fileName = sSplit[sSplit.Length - 1];
            sFileNameExtension = System.IO.Path.GetExtension(_fileName).ToLower();

            // Rinomino il file se esiste gia'
            if (File.Exists(_path + _fileName))
            {
                for (int i = 1; ; i++)
                {
                    String sFileNameNew = _fileName.Substring(0, _fileName.Length - sFileNameExtension.Length);
                    sFileNameNew = sFileNameNew + "_" + i.ToString().PadLeft(3, '0') + sFileNameExtension;

                    if (!File.Exists(_path + sFileNameNew))
                    {
                        _fileName = sFileNameNew;
                        break;
                    }

                }
            }

            return _fileName;
        }
        #endregion

        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[0x1000];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, read);
        }
    }
}
