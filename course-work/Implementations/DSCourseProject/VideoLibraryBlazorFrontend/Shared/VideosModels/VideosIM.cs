using System.ComponentModel.DataAnnotations;

namespace VideoLibraryBlazorFrontend.Shared.VideosModels
{
    public class VideosIM
    {
        [Required(ErrorMessage = "This field is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public string FileId { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [Range(0, int.MaxValue, ErrorMessage = "File size cannot be negative MBs")]
        public int Size { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a format.")]
        public int FormatId { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public DateTime YearOfPublishing { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a copyright.")]
        public int CopyrightId { get; set; }
    }
}
