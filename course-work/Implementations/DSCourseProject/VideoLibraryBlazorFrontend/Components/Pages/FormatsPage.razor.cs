using Common.Entities;
using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.FormatsModels;
using Microsoft.JSInterop;
using Microsoft.Identity.Web;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class FormatsPage
    {
        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        private Pager pager = new();
        private FormatsFilter filter = new();
        private List<Formats> Items = new();

        private int[] ItemsPerPageOptions = new[] { 1, 5, 10, 20, 50 };
        private Dictionary<string, string> OrderByOptions = new()
        {
            { "id", "Default (ID)"},
            { "type", "Type" },
            { "extension", "Extension" },
            { "isPhysical", "Is Physical" }
        };
        private Dictionary<string, string> OrderDirOptions = new()
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
            await LoadFormats();
        }

        private async Task LoadFormats()
        {
            try
            {
                var request = new IndexRequestModel<FormatsFilter>
                {
                    Pager = pager,
                    Filter = filter
                };

                var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Formats/get", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Formats, FormatsFilter>>>();

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
        private async Task DeleteFormat(int id)
        {
            var confirm = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this format?");

            if (!confirm)
            {
                return;
            }
            try
            {
                var response = await HttpClient.DeleteAsync($"https://localhost:7209/api/Formats/{id}");
                if (response.IsSuccessStatusCode)
                {
                    await LoadFormats();
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private void NavigateToEdit(int id)
        {
            NavManager.NavigateTo($"/formats/edit/{id}");
        }


        private async Task ApplyFilter()
        {
            pager.Page = 1;
            await LoadFormats();
        }
        private async Task FirstPage()
        {
            pager.Page = 1;
            await LoadFormats();

        }
        private async Task PrevPage()
        {
            if (CanPrev)
            {
                pager.Page--;
                await LoadFormats();
            }
        }

        private async Task NextPage()
        {
            if (CanNext)
            {
                pager.Page++;
                await LoadFormats();
            }
        }
        private async Task LastPage()
        {
            pager.Page = pager.PagesCount;
            await LoadFormats();

        }

        private string? IsPhysicalSelection
        {
            get => filter.IsPhysical?.ToString().ToLower();
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    filter.IsPhysical = null;
                }
                else if (bool.TryParse(value, out var result))
                {
                    filter.IsPhysical = result;
                }
            }
        }

        private async Task ItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newCount))
            {
                pager.ItemsPerPage = newCount;
                pager.Page = 1;
                await LoadFormats();
            }
        }
    }
}