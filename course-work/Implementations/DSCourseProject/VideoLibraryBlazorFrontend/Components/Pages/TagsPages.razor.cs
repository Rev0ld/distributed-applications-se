using Common.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Microsoft.JSInterop;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.TagsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class TagsPages
    {
        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }
        
        [Inject]
        IJSRuntime JS { get; set; }

        private Pager pager = new();
        private TagsFilter filter = new();
        private List<Tags> Items = new();

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
            await LoadTags();
        }

        private async Task LoadTags()
        {
            try
            {
                var request = new IndexRequestModel<TagsFilter>
                {
                    Pager = pager,
                    Filter = filter
                };

                var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Tags/get", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Tags, TagsFilter>>>();

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
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }

        private async Task DeleteAuthor(int id)
        {
            var confirm = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this tag?");

            if (!confirm)
            {
                return;
            }
            try
            {
                var response = await HttpClient.DeleteAsync($"https://localhost:7209/api/Tags/{id}");
                if (response.IsSuccessStatusCode)
                {
                    await LoadTags();
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private void NavigateToEdit(int id)
        {
            NavManager.NavigateTo($"/tags/edit/{id}");
        }

        private async Task ApplyFilter()
        {
            pager.Page = 1;
            await LoadTags();
        }
        private async Task FirstPage()
        {
            pager.Page = 1;
            await LoadTags();

        }
        private async Task PrevPage()
        {
            if (CanPrev)
            {
                pager.Page--;
                await LoadTags();
            }
        }

        private async Task NextPage()
        {
            if (CanNext)
            {
                pager.Page++;
                await LoadTags();
            }
        }
        private async Task LastPage()
        {
            pager.Page = pager.PagesCount;
            await LoadTags();

        }

        private async Task ItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newCount))
            {
                pager.ItemsPerPage = newCount;
                pager.Page = 1;
                await LoadTags();
            }
        }
    }
}