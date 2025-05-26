using Common.Entities.M2MEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Genres : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(128, MinimumLength = 3)]
        public string? Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<GenreVideo> Videos { get; set; } = new List<GenreVideo>();

    }
}
