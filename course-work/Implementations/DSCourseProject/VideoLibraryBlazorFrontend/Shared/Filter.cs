using System.Text.Json.Serialization;

namespace VideoLibraryBlazorFrontend.Shared
{
    abstract public class Filter
    {
        public string? OrderBy { get; set; }

        public string? OrderDir { get; set; }
    }
}
