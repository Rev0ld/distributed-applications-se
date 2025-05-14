using Common.Entities.M2MEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Authors : BaseEntity
    {
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Biography { get; set; }

        [JsonIgnore]
        public virtual ICollection<AuthorVideo> Videos { get; set; } = new List<AuthorVideo>();
    }
}
