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
        [Length(3, 50)]
        public string? Name { get; set; }

        [Length(2, 50)]
        public string? ShortName { get; set; }

        [Length(3, 50)]
        public string? Description { get; set; }

    }
}
