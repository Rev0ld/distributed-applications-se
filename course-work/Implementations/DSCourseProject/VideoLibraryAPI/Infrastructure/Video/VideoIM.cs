using System.ComponentModel.DataAnnotations;

namespace VideoLibraryAPI.Infrastructure.Video
{
    public class VideoIM
    {
        [Required(ErrorMessage = "This field is Required")]
        /// <summery>
        /// The title of the video
        /// </summery>
        /// <example>I have no idea how to title this video</example>
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        /// <summery>
        /// UUID that represents the video in the file storage
        /// </summery>
        /// <example>b27c8a98-47c9-4f7b-b8dc-66342a5ac46c</example>
        public string FileId { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [Range(0, int.MaxValue, ErrorMessage = "File size cannot be negative MBs")]
        /// <summery>
        /// The size of the video in MB, rounded to the nearest bigger value
        /// </summery>
        /// <example>43</example>
        public int Size { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [Range(1, 8)]
        /// <summery>
        /// the ID of the format
        /// </summery>
        /// <example>False</example>
        public int FormatId { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        /// <summery>
        /// A description of the video
        /// </summery>
        /// <example>This is a description of the video</example>
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        /// <summery>
        /// The year in which the video was published
        /// </summery>
        /// <example>False</example>
        public DateTime YearOfPublishing { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        [Range(1, 6)]
        public int CopyrightId { get; set; }
    }
}
