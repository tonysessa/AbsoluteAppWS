using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Support;
//
using dataLibs;

namespace DbModel
{
    public class MyEntityContext : EntityContextWrapper<DbModelEntities>
    {
        public MyEntityContext()
            : base()
        {

        }
    }
}
