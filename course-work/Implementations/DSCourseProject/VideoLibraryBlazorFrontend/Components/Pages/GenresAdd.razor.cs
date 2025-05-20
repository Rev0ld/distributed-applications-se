using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.GenresModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class GenresAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        private GenresIM Genre = new GenresIM();
        private async Task AddToDataBase()
        {
            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Genres", Genre);

            if (response.IsSuccessStatusCode)
            {
                Genre.Name = null;
                Genre.Description = null;
            }
            else
            { }
        }
    }
}