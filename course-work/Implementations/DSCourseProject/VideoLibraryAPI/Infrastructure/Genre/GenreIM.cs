using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Genre
{
    public class GenreIM
    {

        [Required(ErrorMessage = "This field is Required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public string? Description { get; set; }
    }
}
