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
        [StringLength(50, MinimumLength = 3)]
        public string? Type { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public string? Extension { get; set; }

        public bool IsPhysical { get; set; }
    }
}
