﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VelocityMap.Utilities
{
    class INIVariable
    {
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }

        public INIVariable(string name = null, string type = null, string value = null)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }
    }
}
