using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.CopyrightsModels;
using static VideoLibraryBlazorFrontend.Components.Pages.Home;
using Common.Entities;

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
            var response = await HttpClient.GetFromJsonAsync<ApiResponse<CopyrightsIM>>($"https://localhost:7209/api/Copyrights/{CopyrightId}");
            if (response != null && response.Success)
            {
                Copyright = response.Data;
            }
        }
        private async Task HandleValidSubmit()
        {
            var response = await HttpClient.PutAsJsonAsync($"https://localhost:7209/api/Copyrights/{CopyrightId}", Copyright);

            if (response.IsSuccessStatusCode)
            {
                NavManager.NavigateTo("/copyrights");
            }

        }

        private void Cancel()
        {
            NavManager.NavigateTo("/copyrights");
        }
    }
}