using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbModel
{
    [Flags]
    public enum EnumCmsContent : int
    {
        Disabled  = 0,
        Enabled   = 1,
        Deleted   = 2        
    }
}
