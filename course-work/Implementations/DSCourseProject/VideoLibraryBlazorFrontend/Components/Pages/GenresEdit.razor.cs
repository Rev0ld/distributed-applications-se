using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.GenresModels;
using static VideoLibraryBlazorFrontend.Components.Pages.Home;
using Common.Entities;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class GenresEdit
    {
        [Parameter]
        public int GenreId { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        private GenresIM? Genre;

        protected override async Task OnInitializedAsync()
        {
            var response = await HttpClient.GetFromJsonAsync<ApiResponse<GenresIM>>($"https://localhost:7209/api/Genres/{GenreId}");
            if (response != null && response.Success)
            {
                Genre = response.Data;
            }
        }
        private async Task HandleValidSubmit()
        {
            var response = await HttpClient.PutAsJsonAsync($"https://localhost:7209/api/Genres/{GenreId}", Genre);

            if (response.IsSuccessStatusCode)
            {
                NavManager.NavigateTo("/genres");
            }

        }

        private void Cancel()
        {
            NavManager.NavigateTo("/genres");
        }

    }
}