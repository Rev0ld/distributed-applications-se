using Common.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.VideosModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class VideosPage
    {
        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        private Pager pager = new();
        private VideosFilter filter = new();
        private List<Videos> Items= new();

        private int[] ItemsPerPageOptions = new[] { 1, 5, 10, 20, 50 };
        private Dictionary<string, string> OrderByOptions = new()
        {
            { "id", "Default (ID)"}
        };
        private Dictionary<string, string> orderDirOptions = new()
        {
            { "desc", "Descending" },
            { "asc", "Ascending" }
        };
        private Dictionary<string, string> IsPhysicalOptions = new()
        {
            { "", "All" },
            { "true", "Physical" },
            { "false", "Digital" }
        };

        private bool CanPrev => pager.Page > 1;
        private bool CanNext => pager.Page < pager.PagesCount;

        protected override async Task OnInitializedAsync()
        {
            await LoadVideos();
        }

        private async Task LoadVideos()
        {
            var request = new IndexRequestModel<VideosFilter>
            {
                Pager = pager,
                Filter = filter
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Video/get", request);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Videos, VideosFilter>>>();

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

        private async Task DeleteVideo(int id)
        {
            var confirm = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this author?");

            if (!confirm)
            {
                return;
            }
            var response = await HttpClient.DeleteAsync($"https://localhost:7209/api/Video/{id}");
            if (response.IsSuccessStatusCode)
            {
                await LoadVideos();
            }
        }
        private void NavigateToEdit(int id)
        {
            NavManager.NavigateTo($"/videos/edit/{id}");
        }

        private async Task ApplyFilter()
        {
            pager.Page = 1;
            await LoadVideos();
        }
        private async Task FirstPage()
        {
            pager.Page = 1;
            await LoadVideos();

        }
        private async Task PrevPage()
        {
            if (CanPrev)
            {
                pager.Page--;
                await LoadVideos();
            }
        }

        private async Task NextPage()
        {
            if (CanNext)
            {
                pager.Page++;
                await LoadVideos();
            }
        }
        private async Task LastPage()
        {
            pager.Page = pager.PagesCount;
            await LoadVideos();

        }
        private string? IsPhysicalSelection
        {
            get => filter.FormatIsPhysical?.ToString().ToLower();
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    filter.FormatIsPhysical = null;
                }
                else if (bool.TryParse(value, out var result))
                {
                    filter.FormatIsPhysical = result;
                }
            }
        }

        private async Task ItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newCount))
            {
                pager.ItemsPerPage = newCount;
                pager.Page = 1;
                await LoadVideos();
            }
        }
    }
}