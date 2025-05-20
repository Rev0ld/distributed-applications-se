using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.FormatsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class FormatsAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        private Shared.FormatsModels.FormatsIM Format = new Shared.FormatsModels.FormatsIM();
        private async Task AddToDataBase()
        {
            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Formats", Format);

            if (response.IsSuccessStatusCode)
            {
                Format.Type = null;
                Format.Extension = null;
                Format.IsPhysical = false;
            }
            else
            { }
        }
    }
}