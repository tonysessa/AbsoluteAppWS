using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DbModel
{
    [DataContract]
    public class Paging
    {
        [DataMember]
        public int PageNumber = 0;

        [DataMember]
        public int PageSize = 20;

        [DataMember]
        public int TotalPages = 0;

        [DataMember]
        public int TotalRows = 0;
    }
}
