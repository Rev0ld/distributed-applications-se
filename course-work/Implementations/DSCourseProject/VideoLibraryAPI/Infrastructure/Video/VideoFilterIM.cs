using System.ComponentModel.DataAnnotations;
using VideoLibraryAPI.Infrastructure.Shared;

namespace VideoLibraryAPI.Infrastructure.Video
{
    public class VideoFilterIM : FilterIM
    {
        public string Title { get; set; }

        public string FileId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "File size cannot be negative MBs")]
        public int? Size { get; set; }

        [Range(typeof(DateTime), "1/1/1980", "31/12/2025")]
        public DateTime? YearOfPublishing { get; set; }

        public string? AuthorFirstName { get; set; }
        public string? AuthorMiddleName { get; set; }
        public string? AuthorLastName { get; set; }
    }
}
