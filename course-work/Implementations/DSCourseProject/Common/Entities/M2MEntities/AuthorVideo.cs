using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities.M2MEntities
{
    public class AuthorVideo : BaseEntity
    {
        public int AuthorId { get; set; }

        public int VideoId { get; set; }

        public virtual Authors Author { get; set; } = null!;

        public virtual Videos Video { get; set; } = null!;
    }
}
