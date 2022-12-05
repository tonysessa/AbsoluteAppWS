using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;

namespace dataLibs
{

    public partial class CmsRoles
    {

        [DataMember]
        public List<CmsRoles_Acl> iCmsRoles_Acl
        {
            get
            {

                return this.CmsRoles_Acl.Where(t => t.StatusFlag == (Int32)EnumCmsContent.Enabled).OrderBy(t => t.CmsSubSections.Title).ToList();
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public List<CmsSubSections> EnableCmsSubSections
        {
            get
            {
                List<CmsSubSections> objReturn = new List<CmsSubSections>();
                if (this.Uriname.Equals("ADMIN"))
                {
                    using (MyEntityContext ctx = new MyEntityContext())
                        objReturn = ctx.Context.CmsSubSections.Include("CmsSections").Where(t => t.StatusFlag == (int)EnumCmsContent.Enabled).OrderBy(t => t.CmsSections.Ord).OrderBy(t => t.Ord).ToList();
                    return objReturn;
                }
                else
                {
                    foreach (CmsRoles_Acl obj in this.iCmsRoles_Acl)
                        objReturn.Add(obj.CmsSubSections);

                    return objReturn;
                }
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public List<CmsSections> EnableCmsSections
        {
            get
            {
                List<CmsSections> objReturn = new List<CmsSections>();
                foreach (CmsSubSections obj in this.EnableCmsSubSections.Where(t=>t.CmsSections.StatusFlag == (int)EnumCmsContent.Enabled).OrderBy(t => t.CmsSections.Ord))
                    if (!objReturn.Exists(t => t.Uid == obj.CmsSections.Uid))
                        objReturn.Add(obj.CmsSections);

                return objReturn;
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

    }
}
