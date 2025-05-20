namespace VideoLibraryBlazorFrontend.Shared.FormatsModels
{
    public class FormatsFilter : Filter
    {
        public string? Type { get; set; }

        public string? Extension { get; set; }

        public bool? IsPhysical { get; set; }
    }
}
