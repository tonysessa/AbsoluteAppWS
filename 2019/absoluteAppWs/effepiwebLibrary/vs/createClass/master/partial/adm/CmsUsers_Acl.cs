using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;

namespace dataLibs
{
    
    public partial class CmsUsers_Acl
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
    }
}
