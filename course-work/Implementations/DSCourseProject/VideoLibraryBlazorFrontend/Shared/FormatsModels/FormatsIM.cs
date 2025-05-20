using System.ComponentModel.DataAnnotations;

namespace VideoLibraryBlazorFrontend.Shared.FormatsModels
{
    public class FormatsIM
    {
        [Required(ErrorMessage = "This field is Required")]
        [StringLength(50, ErrorMessage = "Type cannot be longer than 50 characters")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [StringLength(10, ErrorMessage = "Extension cannot be longer than 10 characters")]
        public string? Extension { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public bool IsPhysical { get; set; }
    }
}
