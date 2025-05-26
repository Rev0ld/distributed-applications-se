using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Copyrights : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(10, MinimumLength = 2)]
        public string? ShortName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string? Description { get; set; }

    }
}
