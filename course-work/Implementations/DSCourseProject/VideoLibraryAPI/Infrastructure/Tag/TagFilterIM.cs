using VideoLibraryAPI.Infrastructure.Shared;

namespace VideoLibraryAPI.Infrastructure.Tag
{
    public class TagFilterIM : FilterIM
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
