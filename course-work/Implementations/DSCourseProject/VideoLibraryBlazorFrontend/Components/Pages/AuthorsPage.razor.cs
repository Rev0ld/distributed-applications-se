using Common.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using Microsoft.JSInterop;
using System.Text.Json.Serialization;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using static System.Net.WebRequestMethods;
using static VideoLibraryBlazorFrontend.Components.Pages.Home;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class AuthorsPage
    {
        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }


        private Pager pager = new();
        private AuthorsFilter filter = new();
        private List<Authors> Items = new();

        private int[] ItemsPerPageOptions = new[] { 1,5, 10, 20, 50 };
        private Dictionary<string, string> OrderByOptions = new() 
        {
            { "id", "Default (ID)"},
            { "FirstName", "First Name" },
            { "MiddleName", "Middle Name" },
            { "LastName", "Last Name" },
            { "Biography", "Biography" }
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
            await LoadAuthors();
        }

        private async Task LoadAuthors()
        {
            try
            {
                var request = new IndexRequestModel<AuthorsFilter>
                {
                    Pager = pager,
                    Filter = filter
                };

                var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Authors/get", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Authors, AuthorsFilter>>>();

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
            var confirm = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this author?");

            if (!confirm) 
            {
                return;
            }
            try
            {
                var response = await HttpClient.DeleteAsync($"https://localhost:7209/api/Authors/{id}");
                if (response.IsSuccessStatusCode)
                {
                    await LoadAuthors();
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private void NavigateToEdit(int id)
        {
            NavManager.NavigateTo($"/authors/edit/{id}");
        }

        private async Task ApplyFilter()
        {
            pager.Page = 1;
            await LoadAuthors();
        }
        private async Task FirstPage()
        {
                pager.Page = 1;
                await LoadAuthors();
            
        }
        private async Task PrevPage()
        {
            if (CanPrev)
            {
                pager.Page--;
                await LoadAuthors();
            }
        }

        private async Task NextPage()
        {
            if (CanNext)
            {
                pager.Page++;
                await LoadAuthors();
            }
        }
        private async Task LastPage()
        {
            pager.Page = pager.PagesCount;
            await LoadAuthors();

        }

        private async Task ItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newCount))
            {
                pager.ItemsPerPage = newCount;
                pager.Page = 1;
                await LoadAuthors();
            }
        }


    }
}