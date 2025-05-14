using VideoLibraryAPI.Infrastructure.Shared;

namespace VideoLibraryAPI.Infrastructure.Format
{
    public class FormatFilterIM : FilterIM
    {
        public string? Type { get; set; }

        public string? Extension { get; set; }

        public bool? IsPhysical { get; set; }
    }
}
