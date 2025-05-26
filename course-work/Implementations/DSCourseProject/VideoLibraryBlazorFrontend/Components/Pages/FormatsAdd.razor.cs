using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.FormatsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class FormatsAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        [Inject]
        NavigationManager NavManager { get; set; }

        private FormatsIM Format = new FormatsIM();


        private async Task AddToDataBase()
        {
            try
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
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
    }
}