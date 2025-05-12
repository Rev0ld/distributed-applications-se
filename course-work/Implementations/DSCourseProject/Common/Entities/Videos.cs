using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Videos : BaseEntity
    {
        public string? Title { get; set; }

        public string? FileId { get; set; }

        public int FormatId { get; set; }
        public virtual Formats Format { get; set; }

        public int? Size { get; set; }

        public string? Description { get; set; }

        public DateTime? YearOfPublishing { get; set; }

        public int CopyrightId { get; set; }
        public virtual Copyrights Copyright { get; set; }


    }
}
