using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.TagsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class TagsAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        private TagsIM Tag = new TagsIM();
        private async Task AddToDataBase()
        {
            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Tags", Tag);

            if (response.IsSuccessStatusCode)
            {
                Tag.Name = null;
                Tag.Description = null;
            }
            else
            { }
        }
    }
}