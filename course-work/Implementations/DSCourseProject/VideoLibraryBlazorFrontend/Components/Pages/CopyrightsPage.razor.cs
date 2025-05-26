using Common.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using Microsoft.JSInterop;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.CopyrightsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class CopyrightsPage
    {
        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        private Pager pager = new();
        private CopyrightsFilter filter = new();
        private List<Copyrights> Items = new();

        private int[] ItemsPerPageOptions = new[] { 1, 5, 10, 20, 50 };
        private Dictionary<string, string> OrderByOptions = new()
        {
            { "id", "Default (ID)"},
            { "name", "Name" },
            { "shortname", "Short Name" },
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
            await LoadCopyrights();
        }

        private async Task LoadCopyrights() 
        {
            var request = new IndexRequestModel<CopyrightsFilter>
            {
                Pager = pager,
                Filter = filter
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Copyrights/get", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Copyrights, CopyrightsFilter>>>();

                if (result?.Success == true)
                {
                    Items = result.Data.Items;
                    pager = result.Data.Pager;
                    filter = result.Data.Filter;
                }
            }
        }

        private async Task DeleteCopyright(int id) 
        {
            var confirm = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this author?");

            if (!confirm)
            {
                return;
            }
            try
            {
                var response = await HttpClient.DeleteAsync($"https://localhost:7209/api/Copyrights/{id}");
                if (response.IsSuccessStatusCode)
                {
                    await LoadCopyrights();
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private void NavigateToEdit(int id)
        {
            NavManager.NavigateTo($"/copyrights/edit/{id}");
        }

        private async Task ApplyFilter()
        {
            pager.Page = 1;
            await LoadCopyrights();
        }
        private async Task FirstPage()
        {
            pager.Page = 1;
            await LoadCopyrights();

        }
        private async Task PrevPage()
        {
            if (CanPrev)
            {
                pager.Page--;
                await LoadCopyrights();
            }
        }

        private async Task NextPage()
        {
            if (CanNext)
            {
                pager.Page++;
                await LoadCopyrights();
            }
        }
        private async Task LastPage()
        {
            pager.Page = pager.PagesCount;
            await LoadCopyrights();

        }

        private async Task ItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newCount))
            {
                pager.ItemsPerPage = newCount;
                pager.Page = 1;
                await LoadCopyrights();
            }
        }
    }
}