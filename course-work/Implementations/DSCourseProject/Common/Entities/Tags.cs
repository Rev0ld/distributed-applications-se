﻿using Common.Entities.M2MEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Tags : BaseEntity
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<TagVideo> Videos { get; set; } = new List<TagVideo>();
    }
}
