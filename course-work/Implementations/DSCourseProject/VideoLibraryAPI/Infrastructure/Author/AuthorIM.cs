using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace VideoLibraryAPI.Infrastructure.Author
{
    public class AuthorIM
    {   
        
        /// <example>FirstName</example>
        [Required(ErrorMessage = "FirstName field is Required")]        
        public string? FirstName { get; set; }

        /// <example>MiddleName</example>
        [Required(ErrorMessage = "MiddleName field is Required")]
        public string? MiddleName { get; set; }

        /// <example>LastName</example>
        [Required(ErrorMessage = "LastName field is Required")]
        public string? LastName { get; set; }

        /// <example>Short Bio: Lorem ipsum dolor sit amet...I</example>
        [Required(ErrorMessage = "Biography field is Required")]
        public string? Biography { get; set; }
    }
}
