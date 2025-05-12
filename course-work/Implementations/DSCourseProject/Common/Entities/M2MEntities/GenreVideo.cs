using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities.M2MEntities
{
    class GenreVideo : BaseEntity
    {
        public int GenreId { get; set; }

        public int VideoId { get; set; }
        public virtual Genres Genre { get; set; } = null!;

        public virtual Videos Video { get; set; } = null!;
    }
}
