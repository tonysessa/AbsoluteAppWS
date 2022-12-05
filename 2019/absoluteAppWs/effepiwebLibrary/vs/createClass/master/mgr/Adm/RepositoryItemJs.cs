using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dataLibs
{
    public class RepositoryItemJs
    {
        public class items
        {
            public String UidRepository { get; set; }
            public String Uid { get; set; }
            public String typo { get; set; }
            public String path { get; set; }
            public String name { get; set; }            
            public String thumb { get; set; }
            public String original { get; set; }
            public String extension { get; set; }

            public List<itemDetails> details = new List<itemDetails>();
        }

        public class itemDetails
        {
            public String label { get; set; }
            public String value { get; set; }
        }

        public class Error
        {
            public String msg { get; set; }

        }
    }
}
