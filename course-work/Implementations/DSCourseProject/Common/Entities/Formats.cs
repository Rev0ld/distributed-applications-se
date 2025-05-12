using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Formats : BaseEntity
    {
        public string? Type { get; set; }

        public string? Extension { get; set; }

        public bool IsPhysical { get; set; }
    }
}
