using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;
using Support.Web;

namespace dataLibs
{
    public partial class CmsRepository: ICmsRepositoryItem
    {        

        /**
        * Relative Path
        **/
        [DataMember]
        public String RelativePath
        {
            get
            {
                return Folder + "/";
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        public EnumCmsFileType FileType
        {
            get
            {
                return EnumCmsFileType.Repository;
            }
        }

        public ICmsRepositoryItem ParentItem
        {
            get
            {
                return null;
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
                String basePath = WebContext.getConfig("%.storageRepositoryBasePath");
                return CombinePath(basePath, Folder.Replace("/", "\\") + "\\");
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
                String baseUrl = WebContext.getConfig("%.storageRepositoryBaseUrl");
                return CombinePath(baseUrl, Folder + "/");
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        public Boolean Browsable
        {
            get
            {
                return true;
            }
        }

        /*                   
         *  Lista degli item diretti
         **/
        public List<CmsFile> ListFiles
        {
            get
            {
                List<CmsFile> ret = new List<CmsFile>();
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    ret = (from t in ctx.Context.CmsFile.Include("CmsRepository")
                           where t.Uid_CmsRepository == this.Uid && t.Uid_Parent == null && t.StatusFlag != (int)EnumCmsFileStatus.Deleted 
                           select t).ToList();
                }
                return ret;
            }
        }

        /*
        * Lista dei files
        **/
        public List<CmsFile> ListStreams
        {
            get
            {
                List<CmsFile> ret = new List<CmsFile>();
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    ret = (from t in ctx.Context.CmsFile.Include("CmsRepository")
                           where t.Uid_CmsRepository == this.Uid && t.Uid_Parent == null && t.StatusFlag != (int)EnumCmsFileStatus.Deleted && t.FileTypeFlag == (int)EnumCmsFileType.Stream
                           select t).ToList();
                }
                return ret;
            }
        }
        /*
        *  Lista delle directory
        **/
        public List<CmsFile> ListDirectories
        {
            get
            {
                List<CmsFile> ret = new List<CmsFile>();
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    ret = (from t in ctx.Context.CmsFile.Include("CmsRepository")
                           where t.Uid_CmsRepository == this.Uid && t.Uid_Parent == null && t.StatusFlag != (int)EnumCmsFileStatus.Deleted && t.FileTypeFlag == (int)EnumCmsFileType.Directory
                           select t).ToList();
                }
                return ret;
            }
        }

        private static String CombinePath(String baseUrl, String url1)
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

        private static string Safe(String str)
        {
            return (str == null) ? "" : str;
        }
    }
}
