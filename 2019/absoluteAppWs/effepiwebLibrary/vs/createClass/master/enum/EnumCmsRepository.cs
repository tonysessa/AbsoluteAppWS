using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbModel
{
    public enum EnumCmsFileStatus : int
    {
        Disabled    = 0,
        Enabled     = 1,
        Deleted     = 2
    }

    public enum EnumCmsFileType : int
    {
        Stream      = 0,
        Directory   = 1,
        Repository  = 2
    }
}
