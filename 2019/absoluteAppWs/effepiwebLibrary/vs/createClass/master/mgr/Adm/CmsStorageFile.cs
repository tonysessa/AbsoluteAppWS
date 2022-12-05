using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using Support.Web;

namespace DbModel
{
    [DataContract(IsReference = true)]
    public class CmsStorageFile
    {
        private String _RelativePath = "";

        public CmsStorageFile()
        {
            
        }

        /**
         * Relative Path
        **/
        [DataMember]
        public String FileName
        {
            get
            {
                if (String.IsNullOrEmpty(_RelativePath))
                    return _RelativePath;
                int idx = _RelativePath.LastIndexOfAny("\\/".ToCharArray());
                if (idx == -1)
                    return _RelativePath;
                return _RelativePath.Substring(idx + 1);
            }
            set
            {
                // NOT IMPLEMENTED
            }
        }

        /**
         * Relative Path
        **/
        [DataMember]
        public String RelativePath
        {
            get
            {
                return _RelativePath;
            }
            set
            {
                _RelativePath = value;
            }
        }

        /**
        * Utente che ha creato
        **/
        [DataMember]
        public String AbsolutePath
        {
            get
            {
                Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
                String basePath = WebContext.getConfig("%.storagePublicBasePath").ToString();
                return objLibString.CombinePath(basePath, _RelativePath);
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }
        /**
        * Utente che ha creato
        **/
        [DataMember]
        public String AbsoluteUrl
        {
            get
            {
                Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
                String baseUrl = WebContext.getConfig("%.storagePublicBasePath").ToString();
                return objLibString.CombinePath(baseUrl, _RelativePath);
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

       
 
    }
}
