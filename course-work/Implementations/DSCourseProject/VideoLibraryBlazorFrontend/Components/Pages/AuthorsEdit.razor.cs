using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class AuthorsEdit
    {
        [Parameter]
        public int AuthorId { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        private AuthorsIM? Author;

        protected override async Task OnInitializedAsync()
        {
            var response = await HttpClient.GetFromJsonAsync<ApiResponse<AuthorsIM>>($"https://localhost:7209/api/Authors/{AuthorId}");
            if (response != null && response.Success)
            {
                Author = response.Data;
            }
        }

        private async Task HandleValidSubmit()
        {
            var response = await HttpClient.PutAsJsonAsync($"https://localhost:7209/api/Authors/{AuthorId}", Author);

            if (response.IsSuccessStatusCode)
            {
                NavManager.NavigateTo("/authors");
            }
            
        }

        private void Cancel()
        {
            NavManager.NavigateTo("/authors");
        }
    }
}