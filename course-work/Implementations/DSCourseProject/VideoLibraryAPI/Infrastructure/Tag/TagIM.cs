using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Tag
{
    public class TagIM
    {

        [Required(ErrorMessage = "This field is Required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public string? Description { get; set; }
    }
}
