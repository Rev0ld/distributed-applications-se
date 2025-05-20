using System.ComponentModel.DataAnnotations;

namespace VideoLibraryBlazorFrontend.Shared.TagsModels
{
    public class TagsIM
    {

        [Required(ErrorMessage = "This field is Required")]
        [StringLength(50, ErrorMessage = "Type cannot be longer than 50 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [StringLength(50, ErrorMessage = "Type cannot be longer than 50 characters")]
        public string? Description { get; set; }
    }
}
