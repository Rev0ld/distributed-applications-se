﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Copyrights : BaseEntity
    {
        public string? Name { get; set; }
     
        public string? ShortName { get; set; }

        public string? Description { get; set; }

    }
}
