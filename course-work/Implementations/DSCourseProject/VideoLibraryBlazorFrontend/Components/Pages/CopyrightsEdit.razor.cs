using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.CopyrightsModels;
using static VideoLibraryBlazorFrontend.Components.Pages.Home;
using Common.Entities;
using Microsoft.Identity.Web;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class CopyrightsEdit
    {
        [Parameter]
        public int CopyrightId { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        private CopyrightsIM Copyright;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await HttpClient.GetFromJsonAsync<ApiResponse<CopyrightsIM>>($"https://localhost:7209/api/Copyrights/{CopyrightId}");
                if (response != null && response.Success)
                {
                    Copyright = response.Data;
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
                var response = await HttpClient.PutAsJsonAsync($"https://localhost:7209/api/Copyrights/{CopyrightId}", Copyright);

                if (response.IsSuccessStatusCode)
                {
                    NavManager.NavigateTo("/copyrights");
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }


        }

        private void Cancel()
        {
            NavManager.NavigateTo("/copyrights");
        }
    }
}