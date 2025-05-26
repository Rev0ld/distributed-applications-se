using System.Text.Json.Serialization;

namespace VideoLibraryBlazorFrontend.Shared
{
    public class Pager
    {
        public int Page { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 1;

        public int PagesCount { get; set; } = 1;
    }
}
