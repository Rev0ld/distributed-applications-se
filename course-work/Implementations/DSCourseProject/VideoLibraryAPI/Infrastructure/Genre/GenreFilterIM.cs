using VideoLibraryAPI.Infrastructure.Shared;

namespace VideoLibraryAPI.Infrastructure.Genre
{
    public class GenreFilterIM : FilterIM
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
