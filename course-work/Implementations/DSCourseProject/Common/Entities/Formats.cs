using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Formats : BaseEntity
    {
        [Length(3, 50)]
        public string? Type { get; set; }

        [Length(1, 10)]
        public string? Extension { get; set; }

        public bool IsPhysical { get; set; }
    }
}
