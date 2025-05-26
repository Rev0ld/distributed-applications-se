using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared;
using Microsoft.AspNetCore.Razor.TagHelpers;
using VideoLibraryBlazorFrontend.Shared.TagsModels;
using Microsoft.Identity.Web;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class TagsEdit
    {
        [Parameter]
        public int TagId { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        private TagsIM Tag;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await HttpClient.GetFromJsonAsync<ApiResponse<TagsIM>>($"https://localhost:7209/api/Tags/{TagId}");
                if (response != null && response.Success)
                {
                    Tag = response.Data;
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"https://localhost:7209/api/Tags/{TagId}", Tag);

                if (response.IsSuccessStatusCode)
                {
                    NavManager.NavigateTo("/tags");
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            

        }

        private void Cancel()
        {
            NavManager.NavigateTo("/tags");
        }
    }
}