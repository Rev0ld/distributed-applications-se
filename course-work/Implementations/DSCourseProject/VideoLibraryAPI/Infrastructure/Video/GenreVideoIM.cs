using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Video
{
    public class GenreVideoIM
    {
        [Required(ErrorMessage = "This field is Required")]
        public int GenreId { get; set; }
    }
}
