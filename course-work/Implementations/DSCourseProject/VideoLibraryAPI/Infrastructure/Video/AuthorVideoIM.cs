using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Video
{
    public class AuthorVideoIM
    {
        [Required(ErrorMessage = "This field is Required")]
        public int AuthorId { get; set; }
    }
}
