using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;

namespace dataLibs
{

    public partial class CmsNlsContext
    { 
        [DataMember]
        public List<CmsLabels> iCmsLabels
        {
            get
            {
                if (this.CmsLabels.Count() > 0)
                    return this.CmsLabels.Where(t => t.StatusFlag == (Int32)EnumCmsContent.Enabled).ToList();
                return new List<CmsLabels>();

            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public List<CmsResources> iCmsResources
        {
            get
            {
                if (this.CmsResources.Count() > 0)
                    return this.CmsResources.Where(t => t.StatusFlag == (Int32)EnumCmsContent.Enabled).ToList();
                return new List<CmsResources>();

            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public String RepositoryFolder
        {
            get
            {
                return this.Uid.ToString().PadLeft(3, '0');
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        public String StorageFolder
        {
            get
            {
                return this.Uid.ToString().PadLeft(3, '0');
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }
    }
}
