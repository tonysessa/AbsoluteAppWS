using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;

namespace dataLibs
{
    
    public partial class CmsRoles_Acl
    {      
        

        [DataMember]
        public String Title
        {
            get
            {
                try
                {
                    return this.Uid.ToString();
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public CmsSubSections iCmsSubSections
        {
            get
            {
                try
                {
                    if (this.CmsSubSections != null)
                        return this.CmsSubSections;
                    return null;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public CmsRoles iCmsRoles
        {
            get
            {
                try
                {
                    if (this.CmsRoles != null)
                        return this.CmsRoles;
                    return null;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }
    }
}
