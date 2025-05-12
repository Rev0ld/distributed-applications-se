using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities.M2MEntities
{
    public class TagVideo : BaseEntity
    {
        public int TagId { get; set; }

        public int VideoId { get; set; }

        public virtual Tags Tag { get; set; } = null!;

        public virtual Videos Video { get; set; } = null!;
    }
}
