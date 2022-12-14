using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Support.jQueryFileUpload
{
    public class FilesStatus
    {
        public const string HandlerPath = "/";

        public string group { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string progress { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_url { get; set; }
        public string delete_type { get; set; }
        public string error { get; set; }

        public FilesStatus() { }

        public FilesStatus(FileInfo fileInfo) { SetValues(fileInfo.Name, (int)fileInfo.Length); }

        public FilesStatus(string fileName, int fileLength) { SetValues(fileName, fileLength); }

        private void SetValues(string fileName, int fileLength)
        {
            name = fileName;
            type = "image/png";
            size = fileLength;
            progress = "1.0";
            url = HandlerPath + "handler/Upload.ashx?f=" + fileName;
            thumbnail_url = HandlerPath + "handler/Upload.ashx?f=" + fileName;
            delete_url = HandlerPath + "handler/Upload.ashx?f=" + fileName;
            delete_type = "DELETE";
        }
    }
}