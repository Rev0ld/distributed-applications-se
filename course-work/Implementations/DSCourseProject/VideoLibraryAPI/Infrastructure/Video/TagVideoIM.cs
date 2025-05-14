using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Video
{
    public class TagVideoIM
    {
        [Required(ErrorMessage = "This field is Required")]
        public int TagId { get; set; }
    }
}
