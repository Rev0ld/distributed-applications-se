using Common.Entities;
using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.FormatsModels;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.VideosModels;
using static VideoLibraryBlazorFrontend.Components.Pages.Home;
using VideoLibraryBlazorFrontend.Shared.CopyrightsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class VideosAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        private VideosIM Video = new VideosIM();

        private Pager formatsPager = new();
        private FormatsFilter formatsFilter = new();
        private List<Formats> formatsItems = new();

        private Pager copyrightPager = new();
        private CopyrightsFilter copyrightFilter = new();
        private List<Copyrights> copyrightItems = new();


        private int[] ItemsPerPageOptions = new[] { 1, 5, 10, 20, 50 };
        private Dictionary<string, string> OrderByFormatOptions = new()
        {
            { "id", "Default (ID)" },
            { "type", "Type" },
            { "extension", "Extension" },
            { "isPhysical", "Is Physical" }
        };
        private Dictionary<string, string> OrderByCopyrightOptions = new()
        {
            { "id", "Default (ID)"},
            { "name", "Name" },
            { "shortname", "Short Name" },
            { "description", "Description" }
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

        private async Task AddToDataBase()
        {
            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Video", Video);

            if (response.IsSuccessStatusCode)
            {
                Video = new VideosIM();
            }
            else
            { }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadFormats();
            await LoadCopyrights();
        }
        private async Task LoadFormats()
        {
            var request = new IndexRequestModel<FormatsFilter>
            {
                Pager = formatsPager,
                Filter = formatsFilter
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Formats/get", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Formats, FormatsFilter>>>();
                if (result?.Success == true)
                {
                    formatsItems = result.Data.Items;
                    formatsPager = result.Data.Pager;
                    formatsFilter = result.Data.Filter;
                }
            }
        }
        private async Task LoadCopyrights()
        {
            var request = new IndexRequestModel<CopyrightsFilter>
            {
                Pager = copyrightPager,
                Filter = copyrightFilter
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Copyrights/get", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Copyrights, CopyrightsFilter>>>();
                if (result?.Success == true)
                {
                    copyrightItems = result.Data.Items;
                    copyrightPager = result.Data.Pager;
                    copyrightFilter = result.Data.Filter;
                }
            }
        }

        private async Task ApplyFormatsFilter()
        {
            formatsPager.Page = 1;
            await LoadFormats();
        }
        private async Task ApplyCopyrightFilter()
        {
            copyrightPager.Page = 1;
            await LoadCopyrights();
        }

        private async Task FormatsItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount))
            {
                formatsPager.ItemsPerPage = newCount;
                formatsPager.Page = 1;
                await LoadFormats();
            }
        }
        private async Task CopyrightItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount))
            {
                copyrightPager.ItemsPerPage = newCount;
                copyrightPager.Page = 1;
                await LoadCopyrights();
            }
        }

        private async Task FormatsFirstPage() { formatsPager.Page = 1; await LoadFormats(); }
        private async Task FormatsPrevPage() { if (formatsPager.Page > 1) { formatsPager.Page--; await LoadFormats(); } }
        private async Task FormatsNextPage() { if (formatsPager.Page < formatsPager.PagesCount) { formatsPager.Page++; await LoadFormats(); } }
        private async Task FormatsLastPage() { formatsPager.Page = formatsPager.PagesCount; await LoadFormats(); }

        private async Task CopyrightFirstPage() { copyrightPager.Page = 1; await LoadCopyrights(); }
        private async Task CopyrightPrevPage() { if (copyrightPager.Page > 1) { copyrightPager.Page--; await LoadCopyrights(); } }
        private async Task CopyrightNextPage() { if (copyrightPager.Page < copyrightPager.PagesCount) { copyrightPager.Page++; await LoadCopyrights(); } }
        private async Task CopyrightLastPage() { copyrightPager.Page = copyrightPager.PagesCount; await LoadCopyrights(); }


        private void SelectFormat(int id)
        {
            Video.FormatId = id;
        }
        private void SelectCopyright(int id)
        {
            Video.CopyrightId = id;
        }


        private string? formatsIsPhysicalSelection
        {
            get => formatsFilter.IsPhysical?.ToString().ToLower();
            set
            {
                if (string.IsNullOrEmpty(value))
                    formatsFilter.IsPhysical = null;
                else if (bool.TryParse(value, out var result))
                    formatsFilter.IsPhysical = result;
            }
        }
    }
}