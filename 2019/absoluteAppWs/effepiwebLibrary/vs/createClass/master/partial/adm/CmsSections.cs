using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;

namespace dataLibs
{
    
    public partial class CmsSections
    {       

        [DataMember]
        public List<CmsSubSections> iCmsSubSections
        {
            get
            {
                // TODO
                using (MyEntityContext ctx = new MyEntityContext())
                {
                    List<CmsSubSections> item;
                    item = ctx.Context.CmsSubSections.Where(t => t.Uid_CmsSections == this.Uid && t.StatusFlag == (int)EnumCmsContent.Enabled).OrderBy(t => t.Ord).ToList();
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
