using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.TagsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class TagsAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        [Inject]
        NavigationManager NavManager { get; set; }

        private TagsIM Tag = new TagsIM();
        private async Task AddToDataBase()
        {
            try
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
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }

        }
    }
}