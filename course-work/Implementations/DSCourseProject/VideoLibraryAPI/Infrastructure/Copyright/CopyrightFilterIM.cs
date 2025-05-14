using VideoLibraryAPI.Infrastructure.Shared;

namespace VideoLibraryAPI.Infrastructure.Copyright
{
    public class CopyrightFilterIM : FilterIM
    {
        public string? Name { get; set; }

        public string? ShortName { get; set; }

        public string? Description { get; set; }
    }
}
