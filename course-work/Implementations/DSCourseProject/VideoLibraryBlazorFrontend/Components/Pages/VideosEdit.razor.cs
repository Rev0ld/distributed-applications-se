using Common.Entities;
using Common.Entities.M2MEntities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Text;
using System.Text.Json;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using VideoLibraryBlazorFrontend.Shared.CopyrightsModels;
using VideoLibraryBlazorFrontend.Shared.FormatsModels;
using VideoLibraryBlazorFrontend.Shared.GenresModels;
using VideoLibraryBlazorFrontend.Shared.TagsModels;
using VideoLibraryBlazorFrontend.Shared.VideosModels;
using static VideoLibraryBlazorFrontend.Components.Pages.Home;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class VideosEdit
    {
        [Parameter]
        public int VideoId { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }



        private VideosIM? Video;
        private EditContext editContext;

        private Pager formatsPager = new();
        private FormatsFilter formatsFilter = new();
        private List<Formats> formatsItems = new();

        private Pager copyrightPager = new();
        private CopyrightsFilter copyrightFilter = new();
        private List<Copyrights> copyrightItems = new();


        private List<int> authorsUpdate = new();

        private Pager authorPager = new();
        private AuthorsFilter authorFilter = new();
        private List<Authors> authorsItems = new();

        private Pager authorExistPager = new();
        private AuthorsFilter authorExistFilter = new();
        private List<Authors> authorsExistItems = new();

        private List<int> genresUpdate = new();

        private Pager genrePager = new();
        private GenresFilter genreFilter = new();
        private List<Genres> genresItems = new();

        private Pager genreExistPager = new();
        private GenresFilter genreExistFilter = new();
        private List<Genres> genresExistItems = new();

        private List<int> tagsUpdate = new();

        private Pager tagPager = new();
        private TagsFilter tagFilter = new();
        private List<Tags> tagsItems = new();

        private Pager tagExistPager = new();
        private TagsFilter tagExistFilter = new();
        private List<Tags> tagsExistItems = new();



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
        private Dictionary<string, string> OrderByAuthorOptions = new()
        {
            { "id", "Default (ID)"},
            { "FirstName", "First Name" },
            { "MiddleName", "Middle Name" },
            { "LastName", "Last Name" },
            { "Biography", "Biography" }
        };
        private Dictionary<string, string> OrderByGenreOptions = new()
        {
            { "id", "Default (ID)"},
            { "name", "Name" },
            { "description", "Description" }
        };
        private Dictionary<string, string> OrderByTagOptions = new()
        {
            { "id", "Default (ID)"},
            { "name", "Name" },
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

        protected override async Task OnInitializedAsync()
        {
            await LoadFormats();
            await LoadCopyrights();
            await LoadAuthors();
            await LoadExistAuthors();
            await LoadGenres();
            await LoadExistGenres();
            await LoadTags();
            await LoadExistTags();


            var response = await HttpClient.GetFromJsonAsync<ApiResponse<VideosIM>>($"https://localhost:7209/api/Video/{VideoId}");
            if (response != null && response.Success) 
            {
                Video = response.Data;

                editContext = new EditContext(Video);
            }
        }

        private async Task HandleValidSubmit()
        {
            var response = await HttpClient.PutAsJsonAsync($"https://localhost:7209/api/Video/{VideoId}", Video);


            var responseUpdateAuthors = await HttpClient.PostAsync($"https://localhost:7209/api/Video/author/update/{VideoId}", new StringContent(JsonSerializer.Serialize(authorsUpdate), Encoding.UTF8, "application/json"));
            var responseUpdateGenres = await HttpClient.PostAsync($"https://localhost:7209/api/Video/genre/update/{VideoId}", new StringContent(JsonSerializer.Serialize(genresUpdate), Encoding.UTF8, "application/json"));
            var responseUpdateTags = await HttpClient.PostAsync($"https://localhost:7209/api/Video/tag/update/{VideoId}", new StringContent(JsonSerializer.Serialize(tagsUpdate), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode && responseUpdateAuthors.IsSuccessStatusCode && responseUpdateGenres.IsSuccessStatusCode && responseUpdateTags.IsSuccessStatusCode)
            {
                await OnInitializedAsync();
            }
        }

        private void Cancel()
        {
            NavManager.NavigateTo("/videos");
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

        private async Task LoadAuthors() 
        {
            var request = new IndexRequestModel<AuthorsFilter>
            {
                Pager = authorPager,
                Filter = authorFilter
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Authors/get", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Authors, AuthorsFilter>>>();
                if (result?.Success == true)
                {
                    authorsItems = result.Data.Items;
                    authorPager = result.Data.Pager;
                    authorFilter = result.Data.Filter;
                }
            }
        }
        private async Task LoadExistAuthors() 
        {
            var request = new IndexRequestModel<AuthorsFilter>
            {
                Pager = authorExistPager,
                Filter = authorExistFilter
            };

            var response = await HttpClient.PostAsJsonAsync($"https://localhost:7209/api/Video/get/author/{VideoId}", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Authors, AuthorsFilter>>>();
                if (result?.Success == true)
                {
                    authorsExistItems = result.Data.Items;
                    authorExistPager = result.Data.Pager;
                    authorExistFilter = result.Data.Filter;

                    foreach (var item in authorsExistItems)
                    {
                        authorsUpdate.Add(item.Id);
                    }
                }
            }
        }

        private async Task LoadGenres() 
        {
            var request = new IndexRequestModel<GenresFilter>
            {
                Pager = genrePager,
                Filter = genreFilter
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Genres/get", request);
            if (response.IsSuccessStatusCode) 
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Genres, GenresFilter>>>();
                if (result?.Success == true) 
                {
                    genresItems = result.Data.Items;
                    genrePager = result.Data.Pager;
                    genreFilter = result.Data.Filter;
                }
            }

        }
        private async Task LoadExistGenres() 
        {
            var request = new IndexRequestModel<GenresFilter>
            {
                Pager = genreExistPager,
                Filter = genreExistFilter
            };

            var response = await HttpClient.PostAsJsonAsync($"https://localhost:7209/api/Video/get/genre/{VideoId}", request);
            if (response.IsSuccessStatusCode) 
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Genres, GenresFilter>>>();
                if (result?.Success == true) 
                {
                    genresExistItems = result.Data.Items;
                    genreExistPager = result.Data.Pager;
                    genreExistFilter = result.Data.Filter;

                    foreach (var item in genresExistItems)
                    {
                        genresUpdate.Add(item.Id);
                    }

                }


            }



        }

        private async Task LoadTags()
        {
            var request = new IndexRequestModel<TagsFilter>
            {
                Pager = tagPager,
                Filter = tagFilter
            };

            var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Tags/get", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Tags, TagsFilter>>>();
                if (result?.Success == true)
                {
                    tagsItems = result.Data.Items;
                    tagPager = result.Data.Pager;
                    tagFilter = result.Data.Filter;
                }
            }

        }
        private async Task LoadExistTags()
        {
            var request = new IndexRequestModel<TagsFilter>
            {
                Pager = tagExistPager,
                Filter = tagExistFilter
            };

            var response = await HttpClient.PostAsJsonAsync($"https://localhost:7209/api/Video/get/Tag/{VideoId}", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IndexResponseModel<Tags, TagsFilter>>>();
                if (result?.Success == true)
                {
                    tagsExistItems = result.Data.Items;
                    tagExistPager = result.Data.Pager;
                    tagExistFilter = result.Data.Filter;

                    foreach (var item in tagsExistItems)
                    {
                        tagsUpdate.Add(item.Id);
                    }

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



        private async Task ApplyAuthorFilter()
        {
            authorPager.Page = 1;
            await LoadAuthors();
        }
        private async Task ApplyExistAuthorFilter()
        {
            authorExistPager.Page = 1;
            await LoadExistAuthors();
        }

        private async Task ApplyGenreFilter() 
        {
            genrePager.Page = 1;
            await LoadGenres();
        }
        private async Task ApplyExistGenreFilter() 
        {
            genreExistPager.Page = 1;
            await LoadExistGenres();
        }

        private async Task ApplyTagFilter()
        {
            tagPager.Page = 1;
            await LoadTags();
        }
        private async Task ApplyExistTagFilter()
        {
            tagExistPager.Page = 1;
            await LoadExistTags();
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

        private async Task AuthorItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount))
            {
                authorPager.ItemsPerPage = newCount;
                authorPager.Page = 1;
                await LoadAuthors();
            }
        }
        private async Task AuthorExistItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount))
            {
                authorExistPager.ItemsPerPage = newCount;
                authorExistPager.Page = 1;
                await LoadExistAuthors();
            }
        }

        private async Task GenreItemsPerPageChanged(ChangeEventArgs e) 
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount)) 
            {
                genrePager.ItemsPerPage = newCount;
                genrePager.Page = 1;
                await LoadGenres();
            }

        }
        private async Task GenreExistItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount)) 
            {
                genreExistPager.ItemsPerPage = newCount;
                genreExistPager.Page = 1;
                await LoadExistGenres();
            }
        }

        private async Task TagItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount))
            {
                tagPager.ItemsPerPage = newCount;
                tagPager.Page = 1;
                await LoadTags();
            }

        }
        private async Task TagExistItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount))
            {
                tagExistPager.ItemsPerPage = newCount;
                tagExistPager.Page = 1;
                await LoadExistTags();
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


        private async Task AuthorsFirstPage() { authorPager.Page = 1; await LoadAuthors(); }
        private async Task AuthorsPrevPage() { if (authorPager.Page > 1) { authorPager.Page--; await LoadAuthors(); } }
        private async Task AuthorsNextPage() { if (authorPager.Page < authorPager.PagesCount) { authorPager.Page++; await LoadAuthors(); } }
        private async Task AuthorsLastPage() { authorPager.Page = authorPager.PagesCount; await LoadAuthors(); }

        private async Task AuthorsExistFirstPage() { authorExistPager.Page = 1; await LoadExistAuthors(); }
        private async Task AuthorsExistPrevPage() { if (authorExistPager.Page > 1) { authorExistPager.Page--; await LoadExistAuthors(); } }
        private async Task AuthorsExistNextPage() { if (authorExistPager.Page < authorExistPager.PagesCount) { authorExistPager.Page++; await LoadExistAuthors(); } }
        private async Task AuthorsExistLastPage() { authorExistPager.Page = authorExistPager.PagesCount; await LoadExistAuthors(); }

        private async Task GenresFirstPage() { genrePager.Page = 1; await LoadGenres(); }
        private async Task GenresPrevPage() { if (genrePager.Page > 1) { genrePager.Page--; await LoadGenres(); } }
        private async Task GenresNextPage() { if (genrePager.Page < genrePager.PagesCount) { genrePager.Page++; await LoadGenres(); } }
        private async Task GenresLastPage() { genrePager.Page = genrePager.PagesCount; await LoadGenres(); }

        private async Task GenresExistFirstPage() { genreExistPager.Page = 1; await LoadExistGenres(); }
        private async Task GenresExistPrevPage() { if (genreExistPager.Page > 1) { genreExistPager.Page--; await LoadExistGenres(); } }
        private async Task GenresExistNextPage() { if (genreExistPager.Page < genreExistPager.PagesCount) { genreExistPager.Page++; await LoadExistGenres(); } }
        private async Task GenresExistLastPage() { genreExistPager.Page = genreExistPager.PagesCount; await LoadExistGenres(); }

        private async Task TagsFirstPage() { tagPager.Page = 1; await LoadTags(); }
        private async Task TagsPrevPage() { if (tagPager.Page > 1) { tagPager.Page--; await LoadTags(); } }
        private async Task TagsNextPage() { if (tagPager.Page < tagPager.PagesCount) { tagPager.Page++; await LoadTags(); } }
        private async Task TagsLastPage() { tagPager.Page = tagPager.PagesCount; await LoadTags(); }

        private async Task TagsExistFirstPage() { tagExistPager.Page = 1; await LoadExistTags(); }
        private async Task TagsExistPrevPage() { if (tagExistPager.Page > 1) { tagExistPager.Page--; await LoadExistTags(); } }
        private async Task TagsExistNextPage() { if (tagExistPager.Page < tagExistPager.PagesCount) { tagExistPager.Page++; await LoadExistTags(); } }
        private async Task TagsExistLastPage() { tagExistPager.Page = tagExistPager.PagesCount; await LoadExistTags(); }

        private void SelectFormat(int id)
        {
            Video.FormatId = id;
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => Video.FormatId));
        }
        private void SelectCopyright(int id)
        {
            Video.CopyrightId = id;
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => Video.CopyrightId));
        }

        private void AddAuthor(int id) 
        {
            if (!authorsUpdate.Contains(id)) 
            {
                authorsUpdate.Add(id);
            }
        }
        private void RemoveAuthor(int id) 
        {
            if (authorsUpdate.Contains(id))
            {
                authorsUpdate.Remove(id);
            }
        }

        private void AddGenre(int id) 
        {
            if (!genresUpdate.Contains(id)) 
            {
                genresUpdate.Add(id);
            }
        }
        private void RemoveGenre(int id) 
        {
            if (genresUpdate.Contains(id)) 
            {
                genresUpdate.Remove(id);
            }
        }

        private void AddTag(int id) 
        {
            if (!tagsUpdate.Contains(id))
            {
                tagsUpdate.Add(id);
            }
        }
        private void RemoveTag(int id) 
        {
            if (tagsUpdate.Contains(id))
            {
                tagsUpdate.Remove(id);
            }
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