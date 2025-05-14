using VideoLibraryAPI.Infrastructure.Shared;

namespace VideoLibraryAPI.Infrastructure.Author
{
    public class AuthorFilterIM : FilterIM
    {
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Biography { get; set; }
    
    }
}
