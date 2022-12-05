using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DbModel
{
    public enum EnumCmsContentStatus : int
    {
        Draft       = 0,
        Stage       = 1,
        Deleted     = 2
    }

    [JsonObject(MemberSerialization.OptIn)]
    public interface ICmsVersionable
    {
        String Key
        {
            get;
            //set;
        }

        bool ToAdd
        {
            get;
            set;
        }

        bool IsFe
        {
            get;
            set;
        }

        int StatusFlag
        {
            get;
            set;
        }

        int SuperStatusFlag
        {
            get;
            set;
        }
    }
}
