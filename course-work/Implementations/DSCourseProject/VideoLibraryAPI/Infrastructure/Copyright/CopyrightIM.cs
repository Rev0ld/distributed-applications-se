using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Copyright
{
    public class CopyrightIM
    {
        /// <example>Creative Commons</example>
        [Required(ErrorMessage = "Name field is Required")]
        public string? Name { get; set; }

        /// <example>CC</example>
        [Required(ErrorMessage = "ShortName field is Required")]
        public string? ShortName { get; set; }

        /// <example>Used when ...</example>
        [Required(ErrorMessage = "Description field is Required")]
        public string? Description { get; set; }
    }
}
