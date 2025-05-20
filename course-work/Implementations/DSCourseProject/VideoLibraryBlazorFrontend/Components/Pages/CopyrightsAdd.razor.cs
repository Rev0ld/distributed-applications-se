using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.CopyrightsModels;
using static VideoLibraryBlazorFrontend.Components.Pages.Home;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class CopyrightsAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        private CopyrightsIM Copyright = new CopyrightsIM();

        private async Task AddToDataBase() 
        {
            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Copyrights", Copyright);

            if (response.IsSuccessStatusCode)
            {
                Copyright.Name = null;
                Copyright.ShortName = null;
                Copyright.Description = null;
            }
            else { }
        }
    
    }
}