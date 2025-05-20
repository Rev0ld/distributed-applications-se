using System.ComponentModel.DataAnnotations;

namespace VideoLibraryBlazorFrontend.Shared.AuthorsModels
{
    public class AuthorsIM
    {
        [Required(ErrorMessage = "First Name field is Required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "MiddleName field is Required")]
        [StringLength(50, ErrorMessage = "Middle Name cannot be longer than 50 characters")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "LastName field is Required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Biography field is Required")]
        [StringLength(255, ErrorMessage = "Biography cannot be longer than 255 characters")]
        public string? Biography { get; set; }
    }
}
