using System.ComponentModel.DataAnnotations;

namespace VideoLibraryBlazorFrontend.Shared.CopyrightsModels
{
    public class CopyrightsIM
    {
        [Required(ErrorMessage = "Name field is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "ShortName field is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Short Name must be between 3 and 50 characters")]
        public string? ShortName { get; set; }

        [Required(ErrorMessage = "Description field is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 50 characters")]
        public string? Description { get; set; }
    }
}
