using System.ComponentModel.DataAnnotations;

namespace VideoLibraryBlazorFrontend.Shared.VideosModels
{
    public class VideosFilter : Filter
    {
        public string Title { get; set; }

        [Required]
        public string FileId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "File size cannot be negative MBs")]
        public int? Size { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1980", "31/12/2025")]
        public DateTime? YearOfPublishing { get; set; }
        [Required]
        public string? FormatExtension { get; set; }
        [Required]
        public bool? FormatIsPhysical { get; set; }
        [Required]
        public string? AuthorFirstName { get; set; }
        [Required]
        public string? AuthorMiddleName { get; set; }
        [Required]
        public string? AuthorLastName { get; set; }

        [Required]
        public string? CopyrightShortName { get; set; }

        [Required]
        public string? GenreName { get; set; }
    }
    
}
