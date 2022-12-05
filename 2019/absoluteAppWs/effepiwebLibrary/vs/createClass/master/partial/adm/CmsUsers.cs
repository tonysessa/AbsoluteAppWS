using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel;
using System.Runtime.Serialization;

namespace dataLibs
{
    
    public partial class CmsUsers
    {

        [DataMember]
        public String Descrizione
        {
            get
            {
                if ((!String.IsNullOrEmpty(this.Surname)) || (!String.IsNullOrEmpty(this.Name)))
                {
                    return (this.Surname + " " + this.Name).Trim();
                }
                else
                {
                    return this.Username;
                }
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public String Title
        {
            get
            {
                return this.Descrizione;
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }


        [DataMember]
        public Boolean isEnable
        {
            get
            {
                if (this.StatusFlag.Equals(EnumCmsUserStatus.Enabled))
                    return true;
                return false;
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }

        [DataMember]
        public List<CmsUsers_Acl> iCmsUsers_Acl
        {
            get
            {

                return this.CmsUsers_Acl.Where(t => t.StatusFlag == (Int32)EnumCmsContent.Enabled).OrderBy(t => t.CmsNlsContext.Title).ToList();
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

        [DataMember]
        public List<CmsNlsContext> EnableCmsNlsContext
        {
            get
            {
                List<CmsNlsContext> objReturn = new List<CmsNlsContext>();
                if (this.CmsRoles.Uriname.Equals("ADMIN"))
                {
                    using (MyEntityContext ctx = new MyEntityContext())
                        objReturn = ctx.Context.CmsNlsContext.Where(t => t.StatusFlag == (int)EnumCmsContent.Enabled).OrderBy(t => t.Title).ToList();
                    return objReturn;
                }
                else
                {                    
                    foreach (CmsUsers_Acl obj in this.iCmsUsers_Acl)
                        objReturn.Add(obj.CmsNlsContext);

                    return objReturn;
                }
            }
            set
            {
                // NOTIMPLEMENTED
            }
        }


    }
}
