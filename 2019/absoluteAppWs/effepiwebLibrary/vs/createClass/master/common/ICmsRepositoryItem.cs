using System;
using System.Collections.Generic;
using dataLibs;

namespace DbModel
{
    public interface ICmsRepositoryItem
    {
        Int32 Uid
        {
            get;
        }

        EnumCmsFileType FileType
        {
            get;
        }

        Boolean Browsable
        {
            get;
        }

        String RelativePath
        {
            get;
        }

        String AbsolutePath
        {
            get;
        }

        String AbsoluteUrl
        {
            get;
        }

        ICmsRepositoryItem ParentItem
        {
            get;
        }

        List<CmsFile> ListFiles
        {
            get;
        }

        List<CmsFile> ListStreams
        {
            get;
        }

        List<CmsFile> ListDirectories
        {
            get;
        }
    }
}
