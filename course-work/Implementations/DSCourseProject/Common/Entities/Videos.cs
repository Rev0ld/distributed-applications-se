﻿using Common.Entities.M2MEntities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Videos : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(36, MinimumLength = 36)]
        public string? FileId { get; set; }

        [Range(1, int.MaxValue)]
        public int FormatId { get; set; }
        public virtual Formats Format { get; set; }

        [Range(1, int.MaxValue)]
        public int? Size { get; set; }

        [StringLength(255, MinimumLength = 3)]
        public string? Description { get; set; }

        public DateTime? YearOfPublishing { get; set; }

        [Range(1, int.MaxValue)]
        public int CopyrightId { get; set; }
        public virtual Copyrights Copyright { get; set; }

        [JsonIgnore]
        public virtual ICollection<AuthorVideo> Authors { get; set; } = new List<AuthorVideo>();

        [JsonIgnore]
        public virtual ICollection<TagVideo> Tags { get; set; } = new List<TagVideo>();

        [JsonIgnore]
        public virtual ICollection<GenreVideo> Genres { get; set; } = new List<GenreVideo>();



    }
}
