﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class BaseModel
    {
        public BaseModel()
        {
            UserDatabases = new List<Database>();
            IsPartial = false;
        }
        public bool IsPartial { get; set; }
        public string UserName { get; set; }
        public List<Database> UserDatabases { get; set; }
    }
}
