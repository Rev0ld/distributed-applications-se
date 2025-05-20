using VideoLibraryBlazorFrontend.Shared;

namespace VideoLibraryBlazorFrontend.Shared.AuthorsModels
{
    public class AuthorsFilter : Filter
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; }
    }
}
