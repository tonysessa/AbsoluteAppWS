using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;

namespace dataLibs
{
    
    public partial class CmsSubSections
    {
        [DataMember]
        public String FullTitle
        {
            get
            {
                if (this.CmsSections != null)
                {
                    return (this.CmsSections.Title + " - " + this.Title).Trim();
                }
                else
                {
                    return this.Title;
                }
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }


        [DataMember]
        public CmsSections iCmsSectionsParent
        {
            get
            {
                // TODO
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    CmsSections item;
                    item = ctx.Context.CmsSections.Where(t => t.Uid == this.Uid_CmsSections && t.StatusFlag == (int)EnumCmsContent.Enabled).FirstOrDefault();
                    return item;
                }
            }
            set
            {
                //
            }
        }      
    }
}
