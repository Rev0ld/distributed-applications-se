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
    public class Authors : BaseEntity
    {
        [StringLength(50, MinimumLength = 1)]
        public string? FirstName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string? MiddleName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string? LastName { get; set; }

        [StringLength(255, MinimumLength = 1)]
        public string? Biography { get; set; }

        [JsonIgnore]
        public virtual ICollection<AuthorVideo> Videos { get; set; } = new List<AuthorVideo>();
    }
}
