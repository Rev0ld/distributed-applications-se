using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Copyright
{
    public class CopyrightIM
    {
        [Required(ErrorMessage = "Name field is Required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "ShortName field is Required")]
        public string? ShortName { get; set; }

        [Required(ErrorMessage = "Description field is Required")]
        public string? Description { get; set; }
    }
}
