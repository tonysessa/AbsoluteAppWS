using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;

namespace Support.Library
{
    /// <summary>
    /// Un generico BinaryObject
    /// </summary>
    public interface IBinaryObject
    {
        string FullName
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }

        string Ext
        {
            get;
            set;
        }

        string ContentType
        {
            get;
            set;
        }

        byte[] Buffer
        {
            get;
            set;
        }

        Stream DataStream
        {
            get;
            set;
        }

        string FieldName
        {
            get;
            set;
        }

        long Size
        {
            get;
        }

        byte[] cache();

        void autoComplete(Boolean includeName);
        void autoComplete();
    }

    /// <summary>
    /// BinaryObject
    /// </summary>
    public class BinaryObject : IBinaryObject
    {
        private byte[] _Buffer = null;
        private Stream _DataStream = null;
        private long _size = 0;

        public BinaryObject()
        {
        }

        public static long CopyStream(Stream input, Stream output)
        {
            long letti = 0;
            byte[] buffer = new byte[0x1000];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                letti += read;
                output.Write(buffer, 0, read);
            }
            return letti;
        }

        public static String NewGuid()
        {
            return System.Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string Safe(String str)
        {
            return (str == null) ? "" : str;
        }

        public BinaryObject(IBinaryObject data)
        {
            this._Buffer = (data as BinaryObject)._Buffer;
            this._DataStream = (data as BinaryObject)._DataStream;
            this._size = data.Size;
            this.FullName = data.FullName;
            this.ContentType = data.ContentType;
            //_size = data.Size;
        }

        public BinaryObject(byte[] data)
        {
            _Buffer = data;
            _size = data.Length;
        }

        public BinaryObject(Stream data, long size)
        {
            _DataStream = data;
            _size = size;
        }

        public BinaryObject(Stream data)
        {
            _DataStream = data;
            bool ns = false;
            try
            {
                _size = data.Length;
            }
            catch (NotSupportedException ex)
            {
                ns = true;
            }
            if (ns)
            {
                cache();
            }
        }

        public BinaryObject(String str)
        {
            _Buffer = str != null ? Encoding.UTF8.GetBytes(str) : null;
            _size = _Buffer.Length;
        }

        // TODO: da verificare
        public byte[] cache()
        {
            try
            {
                if (_Buffer == null && _DataStream != null)
                {
                    MemoryStream ms = new MemoryStream();
                    _size = CopyStream(_DataStream, ms);
                    // ms.SetLength(_size);
                    _Buffer = ms.GetBuffer();
                    Array.Resize<byte>(ref _Buffer, (int)_size);
                    _DataStream = null;
                }
                else if (_DataStream != null)
                {
                    _DataStream = null;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(HttpContext.Current.Server.MapPath("~//storage/public/log/err.txt"), true))
                {
                    w.WriteLine("cache " + ex.Message); // Write the text
                    w.WriteLine("cache " + ex.InnerException); // Write the text
                }
            }
            return _Buffer;
        }

        public byte[] Buffer
        {
            get
            {
                return cache();
            }
            set
            {
                _Buffer = value;
            }
        }

        public Stream DataStream
        {
            get
            {
                if (_DataStream != null)
                    return _DataStream;
                if (_Buffer == null)
                    return null;
                MemoryStream ms = new MemoryStream(_Buffer);
                ms.Seek(0, SeekOrigin.Begin);
                //
                return ms;
            }
            set
            {
                _DataStream = value;
            }
        }

        public long Size
        {
            get
            {
                return _size;
            }
        }

        public string FullName
        {
            get
            {
                return Name + (Ext != "" ? "." : "") + Ext;
            }
            set
            {
                String[] tks = Safe(value).Split("\\/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                String str = tks.Length > 0 ? tks[tks.Length - 1] : "";
                int idx = str.LastIndexOf('.');
                Name = idx > 0 ? str.Substring(0, idx) : str;
                Ext = idx != -1 ? str.Substring(idx + 1) : "";
            }
        }

        private string _Name = "";
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = Safe(value);
            }
        }

        private string _Ext = "";
        public string Ext
        {
            get
            {
                return _Ext;
            }
            set
            {
                _Ext = Safe(value).ToLower();
                if (_Ext.StartsWith("."))
                    _Ext = _Ext.Substring(1);
            }
        }

        private string _ContentType = "";
        public string ContentType
        {
            get
            {
                return _ContentType;
            }
            set
            {
                _ContentType = Safe(value).ToLower();
            }
        }

        private string _FieldName = "";
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = Safe(value);
            }
        }

        // TODO: da sistemare
        public void autoComplete(Boolean includeName)
        {
            // Name
            if (includeName && Name == "")
                Name = NewGuid();
            // Da Ext cerco di ricavare il content type
            if (ContentType == "" && Ext != "")
                ContentType = MimeTypes.GetMimeType(Ext, "application/octet-stream");
            // da ct cerco di ricavare ext
            if (Ext == "" && ContentType != "")
                Ext = MimeTypes.GetExtension(ContentType, "");
            //
            if (this.FieldName == "")
                this.FieldName = "file";
        }

        public void autoComplete()
        {
            autoComplete(true);
        }
    }

    // ************************************************************************************************************* //
    // Manager lettura/scrittura
    // ************************************************************************************************************* //

    /// <summary>
    /// The Manager
    /// </summary>
    public static class BinaryManager
    {
        public static IBinaryObject ReadFromUri(Uri resourceUri, bool asStreamAsPossible = false)
        {
            using (StreamWriter w = new StreamWriter(HttpContext.Current.Server.MapPath("~//storage/public/log/err.txt"), true))
            {
                w.WriteLine("cache " + resourceUri.IsFile); // Write the text
                w.WriteLine("cache " + resourceUri.AbsoluteUri); // Write the text
            }
            if (resourceUri.IsFile)
            {
                try
                {
                    IBinaryObject ret = !asStreamAsPossible ? new BinaryObject(File.ReadAllBytes(resourceUri.AbsolutePath))
                                                            : new BinaryObject(File.OpenRead(resourceUri.AbsolutePath), new FileInfo(resourceUri.AbsolutePath).Length);
                    ret.FullName = resourceUri.AbsolutePath;
                    ret.autoComplete(false);
                    return ret;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                try
                {
                    HttpWebRequest hwrRequest = (HttpWebRequest)WebRequest.Create(resourceUri.AbsoluteUri);
                    hwrRequest.Method = "GET";
                    HttpWebResponse hwrResponse = (HttpWebResponse)hwrRequest.GetResponse();
                    Stream str = hwrResponse.GetResponseStream();
                    IBinaryObject ret = null;
                    if (hwrResponse.ContentLength > 0)
                        ret = new BinaryObject(hwrResponse.GetResponseStream(), hwrResponse.ContentLength);
                    else
                        ret = new BinaryObject(hwrResponse.GetResponseStream());
                    ret.FullName = resourceUri.AbsolutePath;
                    ret.ContentType = hwrResponse.ContentType;
                    ret.autoComplete(false);
                    //
                    return ret;
                }
                catch (Exception ex)
                {
                    using (StreamWriter w = new StreamWriter(HttpContext.Current.Server.MapPath("~//storage/public/log/err.txt"), true))
                    {
                        w.WriteLine("ReadFromUri catch"); // Write the text
                        w.WriteLine("ReadFromUri catch" + "" + ex.InnerException); // Write the text
                        w.WriteLine("ReadFromUri catch" + "" + ex.Message); // Write the text
                    }

                    return null;
                }
            }
        }

        // ExistUri
        public static bool ExistUri(Uri resourceUri)
        {
            if (resourceUri.IsFile)
            {
                try
                {
                    return File.Exists(resourceUri.AbsolutePath);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    HttpWebRequest hwrRequest = (HttpWebRequest)WebRequest.Create(resourceUri);
                    hwrRequest.Method = "HEAD";
                    HttpWebResponse hwrResponse = (HttpWebResponse)hwrRequest.GetResponse();
                    return (hwrResponse.StatusCode == HttpStatusCode.OK || hwrResponse.StatusCode == HttpStatusCode.Redirect);
                }
                catch
                {
                    return false;
                }
            }
        }

    }

    // ************************************************************************************************************* //
    // Form Uploader
    // ************************************************************************************************************* //

    #region PRIVATE
    public class FormUploader
    {
        public static Encoding encoding = Encoding.UTF8;

        private static string NewDataBoundary()
        {
            Random rnd = new Random();
            string formDataBoundary = "";
            while (formDataBoundary.Length < 15)
            {
                formDataBoundary = formDataBoundary + rnd.Next();
            }
            formDataBoundary = formDataBoundary.Substring(0, 15);
            formDataBoundary = "-----------------------------" + formDataBoundary;
            return formDataBoundary;
        }

        
    }

    #endregion

    // ************************************************************************************************************* //
    // WebBinaryObject
    // ************************************************************************************************************* //

    /// <summary>
    /// Un oggetto Binary web 
    /// </summary>
    public class WebBinaryObject : BinaryObject
    {
        // il percorso relativo rispetto allo storage root
        // esempio /public/xxx/yyy/aaa.jpg
        private String relativeUrl = "";

        /*
        // se applicabile il path completo (asoluto) sul disco dopo il salvatggio
        //private String diskPath = "";
        */

        // l'url http completa  il salvatggio
        // esempio http://www.xxx.com/social/public/xxx/aaa.jpg
        private String httpUrl = "";

        public WebBinaryObject()
            : base()
        {
        }

        public WebBinaryObject(IBinaryObject data)
            : base(data)
        {
        }

        public WebBinaryObject(byte[] data)
            : base(data)
        {
        }

        public WebBinaryObject(Stream data)
            : base(data)
        {
        }

        public WebBinaryObject(Stream data, long size)
            : base(data, size)
        {
        }

        public WebBinaryObject(String data)
            : base(data)
        {
        }

        public WebBinaryObject(HttpPostedFile pf)
            : base(pf.InputStream)
        {
            this.FullName = pf.FileName;
            //bin.Name = ""; // annullo per evitare overwrite
            this.ContentType = pf.ContentType;
        }

        public WebBinaryObject(HttpPostedFile pf, long size)
            : base(pf.InputStream, size)
        {
            this.FullName = pf.FileName;
            //bin.Name = ""; // annullo per evitare overwrite
            this.ContentType = pf.ContentType;
        }

        public String RelativeUrl
        {
            get { return relativeUrl; }
            set { relativeUrl = value; }
        }

        public String HttpUrl
        {
            get { return httpUrl; }
            set { httpUrl = value; }
        }

        /*
        public String DiskPath
        {
            get { return diskPath; }
            set { diskPath = value; }
        }
        */

    }

    // ************************************************************************************************************* //
    // Listing
    // ************************************************************************************************************* //

   
    public class WebObjectItem
    {
   
        public String RelativeUrl = "";

   
        public String HttpUrl = "";

   
        public String ContentType = "";

   
        
    }


}

