using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.FormatsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class FormatsEdit
    {
        [Parameter]
        public int FormatId { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        private FormatsIM? Format;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await HttpClient.GetFromJsonAsync<ApiResponse<FormatsIM>>($"https://localhost:7209/api/Formats/{FormatId}");
                if (response != null && response.Success)
                {
                    Format = response.Data;
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
                var response = await HttpClient.PutAsJsonAsync($"https://localhost:7209/api/Formats/{FormatId}", Format);

                if (response.IsSuccessStatusCode)
                {
                    NavManager.NavigateTo("/formats");
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            

        }

        private void Cancel()
        {
            NavManager.NavigateTo("/formats");
        }
    }
}