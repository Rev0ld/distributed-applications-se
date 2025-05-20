using Common.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.GenresModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class GenresPage
    {
        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        private Pager pager = new();
        private GenresFilter filter = new();
        private List<Genres> Items = new();

        private int[] ItemsPerPageOptions = new[] { 1, 5, 10, 20, 50 };
        private Dictionary<string, string> OrderByOptions = new()
        {
            { "id", "Default (ID)"},
            { "name", "Name" },
            { "description", "Description" }
        };
        private Dictionary<string, string> orderDirOptions = new()
        {
            { "desc", "Descending" },
            { "asc", "Ascending" }
        };

        private bool CanPrev => pager.Page > 1;
        private bool CanNext => pager.Page < pager.PagesCount;


        protected override async Task OnInitializedAsync()
        {
            await LoadGenres();
        }

        private async Task LoadGenres()
        {
            var request = new IndexRequestModel<GenresFilter>
            {
                Pager = pager,
                Filter = filter
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Genres/get", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Genres, GenresFilter>>>();

                if (result?.Success == true)
                {
                    Items = result.Data.Items;
                    pager = result.Data.Pager;
                    filter = result.Data.Filter;
                }
            }
            else
            {

            }
        }

        private async Task DeleteAuthor(int id)
        {
            var confirm = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this genre?");

            if (!confirm)
            {
                return;
            }
            var response = await HttpClient.DeleteAsync($"https://localhost:7209/api/Genres/{id}");
            if (response.IsSuccessStatusCode)
            {
                await LoadGenres();
            }
        }
        private void NavigateToEdit(int id)
        {
            NavManager.NavigateTo($"/genres/edit/{id}");
        }

        private async Task ApplyFilter()
        {
            pager.Page = 1;
            await LoadGenres();
        }
        private async Task FirstPage()
        {
            pager.Page = 1;
            await LoadGenres();

        }
        private async Task PrevPage()
        {
            if (CanPrev)
            {
                pager.Page--;
                await LoadGenres();
            }
        }

        private async Task NextPage()
        {
            if (CanNext)
            {
                pager.Page++;
                await LoadGenres();
            }
        }
        private async Task LastPage()
        {
            pager.Page = pager.PagesCount;
            await LoadGenres();

        }

        private async Task ItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newCount))
            {
                pager.ItemsPerPage = newCount;
                pager.Page = 1;
                await LoadGenres();
            }
        }




    }
}