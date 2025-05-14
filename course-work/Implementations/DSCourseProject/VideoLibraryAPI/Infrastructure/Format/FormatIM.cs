using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Format
{
    public class FormatIM
    {

        [Required(ErrorMessage = "This field is Required")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public string? Extension { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public bool IsPhysical { get; set; }
    }
}
