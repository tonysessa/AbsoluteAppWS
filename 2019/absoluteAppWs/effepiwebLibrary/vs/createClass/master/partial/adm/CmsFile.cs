using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;

namespace dataLibs
{
    public partial class CmsFile : ICmsRepositoryItem
    {
        public CmsFile()
        {
            CreationDate = DateTime.Now;
        }

        /**
        * Repository di appartenenza
        **/
        [DataMember]
        public CmsRepository Repository
        {
            get
            {
                return this.CmsRepository;
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        /**
        * E' un file
        **/
        [DataMember(Order = 999)]
        public EnumCmsFileType FileType
        {
            get
            {
                return (EnumCmsFileType)FileTypeFlag;
            }
            set
            {
                if (value == EnumCmsFileType.Directory || value == EnumCmsFileType.Stream)
                    FileTypeFlag = (int)value;
            }
        }


        /**
        * E' un file
        **/
        [DataMember]
        public Boolean IsStream
        {
            get
            {
                return FileType == EnumCmsFileType.Stream;
            }
            set
            {
                FileType = value ? EnumCmsFileType.Stream : EnumCmsFileType.Directory;
            }
        }

        /**
        * E' una directory
        **/
        [DataMember]
        public Boolean IsDirectory
        {
            get
            {
                return FileType == EnumCmsFileType.Directory;
            }
            set
            {
                FileType = value ? EnumCmsFileType.Directory : EnumCmsFileType.Stream;
            }
        }

        /**
         * Status
         **/
        [DataMember(Order = 999)]
        public EnumCmsFileStatus Status
        {
            get
            {
                return (EnumCmsFileStatus)StatusFlag;
            }
            set
            {
                StatusFlag = (int)value;
            }
        }

        /**
         * Utente che ha creato
         **/
        [DataMember]
        public CmsUsers CreationUser
        {
            get
            {
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    return ctx.Context.CmsUsers.Where(t => t.Uid == Uid_CreationUser).FirstOrDefault();
                }
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
        public CmsUsers DeletionUser
        {
            get
            {
                if (Status != EnumCmsFileStatus.Deleted)
                    return null;
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    return ctx.Context.CmsUsers.Where(t => t.Uid == Uid_UpdateUser).FirstOrDefault();
                }
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
        public CmsUsers UpdateUser
        {
            get
            {
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    return ctx.Context.CmsUsers.Where(t => t.Uid == Uid_UpdateUser).FirstOrDefault();
                }
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        /**
        * Data di sistema (server)
        **/
        [DataMember]
        public DateTime? DeletionDate
        {
            get
            {
                if (Status == EnumCmsFileStatus.Deleted)
                    return this.UpdateDate;
                return null;
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }


        // Parent directory
        public ICmsRepositoryItem ParentItem
        {
            get
            {
                if (this.Uid_Parent == null)
                    return (ICmsRepositoryItem)Repository;
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    // return ctx.db.CmsFile.Where(t => t.Uid == this.Uid_Parent).FirstOrDefault();
                    return (from t in ctx.Context.CmsFile.Include("CmsRepository")
                            where t.Uid == this.Uid_Parent
                            select t).FirstOrDefault();
                }
            }
        }

        public Boolean Browsable
        {
            get
            {
                return this.IsDirectory;
            }
        }

        /**Relative Path**/
        [DataMember]
        public String RelativePath
        {
            get
            {
                if (ParentItem != null)
                    return ParentItem.RelativePath + Name + (this.IsDirectory ? "/" : "");
                else
                    return this.CmsRepository.RelativePath + Name + (this.IsDirectory ? "/" : "");

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
        public String AbsolutePath
        {
            get
            {
                if (ParentItem != null)
                    return ParentItem.AbsolutePath + Name + (this.IsDirectory ? "\\" : "");
                else
                    return this.CmsRepository.AbsolutePath + Name + (this.IsDirectory ? "\\" : "");

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
                if (ParentItem != null)
                    return ParentItem.AbsoluteUrl + Name + (this.IsDirectory ? "/" : "");
                else
                    return this.CmsRepository.AbsoluteUrl + Name + (this.IsDirectory ? "/" : "");

            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public String AbsolutePathIco
        {
            get
            {
                if (ParentItem != null)
                    return ParentItem.AbsolutePath + ImageIco + (this.IsDirectory ? "\\" : "");
                else
                    return this.CmsRepository.AbsolutePath + ImageIco + (this.IsDirectory ? "\\" : "");

            }
            set
            {
                // NOTIMPLEMENTED
            }
        }
        [DataMember]
        public String AbsoluteUrlIco
        {
            get
            {
                if (ParentItem != null)
                    return ParentItem.AbsoluteUrl + ImageIco + (this.IsDirectory ? "/" : "");
                else
                    return this.CmsRepository.AbsoluteUrl + ImageIco + (this.IsDirectory ? "/" : "");
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public String AbsolutePathPrev
        {
            get
            {
                if (ParentItem != null)
                    return ParentItem.AbsolutePath + ImagePrev + (this.IsDirectory ? "\\" : "");
                else
                    return ImagePrev + (this.IsDirectory ? "\\" : "");

            }
            set
            {
                // NOTIMPLEMENTED
            }
        }
        [DataMember]
        public String AbsoluteUrlPrev
        {
            get
            {
                if (ParentItem != null)
                    return ParentItem.AbsoluteUrl + ImagePrev + (this.IsDirectory ? "/" : "");
                else
                    return ImagePrev + (this.IsDirectory ? "/" : "");
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        /*                   
        *  Lista degli item diretti
        **/
        public List<CmsFile> ListFiles
        {
            get
            {
                if (!this.IsDirectory)
                    return null;
                List<CmsFile> ret = new List<CmsFile>();
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    ret = (from t in ctx.Context.CmsFile.Include("CmsRepository")
                           where t.Uid_Parent == this.Uid && t.StatusFlag != (int)EnumCmsFileStatus.Deleted
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
                if (!this.IsDirectory)
                    return null;
                List<CmsFile> ret = new List<CmsFile>();
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    ret = (from t in ctx.Context.CmsFile.Include("CmsRepository")
                           where t.Uid_Parent == this.Uid && t.StatusFlag != (int)EnumCmsFileStatus.Deleted && t.FileTypeFlag == (int)EnumCmsFileType.Stream
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
                if (!this.IsDirectory)
                    return null;
                List<CmsFile> ret = new List<CmsFile>();
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    ret = (from t in ctx.Context.CmsFile.Include("CmsRepository")
                           where t.Uid_Parent == this.Uid && t.StatusFlag != (int)EnumCmsFileStatus.Deleted && t.FileTypeFlag == (int)EnumCmsFileType.Directory
                           select t).ToList();
                }
                return ret;
            }
        }

    }
}
