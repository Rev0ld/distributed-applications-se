using Common.Entities;
using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.FormatsModels;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.VideosModels;
using static VideoLibraryBlazorFrontend.Components.Pages.Home;
using VideoLibraryBlazorFrontend.Shared.CopyrightsModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using System.Runtime.CompilerServices;
using VideoLibraryBlazorFrontend.Shared.GenresModels;
using VideoLibraryBlazorFrontend.Shared.TagsModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Identity.Web;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class VideosAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        [Inject]
        NavigationManager NavManager { get; set; }


        private EditContext editContext;
        private bool clearVideo = true;
        public int? videoId = -1;

        private VideosIM Video = new VideosIM();

        private Pager formatsPager = new();
        private FormatsFilter formatsFilter = new();
        private List<Formats> formatsItems = new();

        private Pager copyrightPager = new();
        private CopyrightsFilter copyrightFilter = new();
        private List<Copyrights> copyrightItems = new();

        private Pager authorPager = new();
        private AuthorsFilter authorFilter = new();
        private List<Authors> authorsItems = new();
        private List<int> authorsToAdd = new();

        private Pager genrePager = new();
        private GenresFilter genreFilter = new();
        private List<Genres> genreItems = new();
        private List<int> genreToAdd = new();

        private Pager tagPager = new();
        private TagsFilter tagFilter = new();
        private List<Tags> tagItems = new();
        private List<int> tagToAdd = new();


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

        private async Task AddToDataBase()
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Video", Video);

                if (response.IsSuccessStatusCode)
                {
                    var createdVideo = await response.Content.ReadFromJsonAsync<Videos>();
                    videoId = createdVideo?.Id;

                    List<int> authorsToRemove = new();
                    List<int> genresToRemove = new();
                    List<int> tagsToRemove = new();

                    for (int i = 0; i < authorsToAdd.Count; i++)
                    {
                        var responseAddAuthors = await HttpClient.PostAsync($"https://localhost:7209/api/Video/author/{videoId}/{authorsToAdd[i]}", null);
                        if (!responseAddAuthors.IsSuccessStatusCode)
                        {
                            var errorContent = await responseAddAuthors.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error adding author {authorsToAdd[i]}: {errorContent}");
                            clearVideo = false;
                        }
                        else
                        {
                            authorsToRemove.Add(authorsToAdd[i]);
                        }
                    }
                    for (int i = 0; i < genreToAdd.Count; i++)
                    {
                        var responseAddGenres = await HttpClient.PostAsync($"https://localhost:7209/api/Video/Genre/{videoId}/{genreToAdd[i]}", null);
                        if (!responseAddGenres.IsSuccessStatusCode)
                        {
                            var errorContent = await responseAddGenres.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error adding author {authorsToAdd[i]}: {errorContent}");
                            clearVideo = false;
                        }
                        else
                        {
                            genresToRemove.Add(genreToAdd[i]);
                        }
                    }
                    for (int i = 0; i < tagToAdd.Count; i++)
                    {
                        var responseAddTag = await HttpClient.PostAsync($"https://localhost:7209/api/Video/Tag/{videoId}/{tagToAdd[i]}", null);
                        if (!responseAddTag.IsSuccessStatusCode)
                        {
                            var errorContent = await responseAddTag.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error adding author {authorsToAdd[i]}: {errorContent}");
                            clearVideo = false;
                        }
                        else
                        {
                            tagsToRemove.Add(tagToAdd[i]);
                        }
                    }
                    foreach (var item in authorsToRemove)
                    {
                        authorsToAdd.Remove(item);
                    }
                    foreach (var item in genresToRemove)
                    {
                        genreToAdd.Remove(item);
                    }
                    foreach (var item in tagsToRemove)
                    {
                        tagToAdd.Remove(item);
                    }

                    if (clearVideo)
                    {
                        Video = new();
                        videoId = -1;
                    }


                }
                else
                { }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private async Task TryAddConnections(int? id) 
        {
            try
            {
                if (id is not null)
                {
                    for (int i = 0; i < authorsToAdd.Count; i++)
                    {
                        var responseAddAuthors = await HttpClient.PostAsync($"https://localhost:7209/api/Video/author/{id}/{authorsToAdd[i]}", null);
                        if (!responseAddAuthors.IsSuccessStatusCode)
                        {
                            var errorContent = await responseAddAuthors.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error adding author {authorsToAdd[i]}: {errorContent}");
                            clearVideo = false;
                        }
                        else
                        {
                            authorsToAdd.Remove(authorsToAdd[i]);
                        }
                    }
                    for (int i = 0; i < genreToAdd.Count; i++)
                    {
                        var responseAddGenres = await HttpClient.PostAsync($"https://localhost:7209/api/Video/Genre/{id}/{genreToAdd[i]}", null);
                        if (!responseAddGenres.IsSuccessStatusCode)
                        {
                            var errorContent = await responseAddGenres.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error adding author {authorsToAdd[i]}: {errorContent}");
                            clearVideo = false;
                        }
                        else
                        {
                            genreToAdd.Remove(genreToAdd[i]);
                        }
                    }
                    for (int i = 0; i < tagToAdd.Count; i++)
                    {
                        var responseAddTag = await HttpClient.PostAsync($"https://localhost:7209/api/Video/Tag/{id}/{tagToAdd[i]}", null);
                        if (!responseAddTag.IsSuccessStatusCode)
                        {
                            var errorContent = await responseAddTag.Content.ReadAsStringAsync();
                            Console.WriteLine($"Error adding author {authorsToAdd[i]}: {errorContent}");
                            clearVideo = false;
                        }
                        else
                        {
                            tagToAdd.Remove(tagToAdd[i]);
                        }
                    }
                    if (clearVideo)
                    {
                        Video = new();
                        videoId = -1;
                    }
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            editContext = new EditContext(Video);
            await LoadFormats();
            await LoadCopyrights();
            await LoadAuthors();
            await LoadGenres();
            await LoadTags();
        }


        private async Task LoadFormats()
        {
            try
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
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private async Task LoadCopyrights()
        {
            try
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
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private async Task LoadAuthors()
        {
            try
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
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private async Task LoadGenres()
        {
            try
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
                        genreItems = result.Data.Items;
                        genrePager = result.Data.Pager;
                        genreFilter = result.Data.Filter;
                    }
                }

            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
        private async Task LoadTags()
        {
            try
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
                        tagItems = result.Data.Items;
                        tagPager = result.Data.Pager;
                        tagFilter = result.Data.Filter;
                    }
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
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
        private async Task ApplyGenreFilter()
        {
            genrePager.Page = 1;
            await LoadGenres();
        }
        private async Task ApplyTagFilter()
        {
            tagPager.Page = 1;
            await LoadTags();
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
        private async Task AuthorsItemsPerPageChanged(ChangeEventArgs e) 
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount)) 
            {
                authorPager.ItemsPerPage = newCount;
                authorPager.Page = 1;
                await LoadAuthors();
            }
        }
        private async Task GenresItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount))
            {
                genrePager.ItemsPerPage = newCount;
                genrePager.Page = 1;
                await LoadGenres();
            }
        }
        private async Task TagsItemsPerPageChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newCount))
            {
                tagPager.ItemsPerPage = newCount;
                tagPager.Page = 1;
                await LoadTags();
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

        private async Task GenresFirstPage() { genrePager.Page = 1; await LoadGenres(); }
        private async Task GenresPrevPage() { if (genrePager.Page > 1) { genrePager.Page--; await LoadGenres(); } }
        private async Task GenresNextPage() { if (genrePager.Page < genrePager.PagesCount) { genrePager.Page++; await LoadGenres(); } }
        private async Task GenresLastPage() { genrePager.Page = genrePager.PagesCount; await LoadGenres(); }

        private async Task TagsFirstPage() { tagPager.Page = 1; await LoadTags(); }
        private async Task TagsPrevPage() { if (tagPager.Page > 1) { tagPager.Page--; await LoadTags(); } }
        private async Task TagsNextPage() { if (tagPager.Page < tagPager.PagesCount) { tagPager.Page++; await LoadTags(); } }
        private async Task TagsLastPage() { tagPager.Page = tagPager.PagesCount; await LoadTags(); }



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

        private void SelectAuthor(int id) 
        {
            authorsToAdd.Add(id);
        }
        private void UnselectAuthor(int id) 
        {
            authorsToAdd.Remove(id);
        }
        private void SelectGenre(int id)
        {
            genreToAdd.Add(id);
        }
        private void UnselectGenre(int id)
        {
            genreToAdd.Remove(id);
        }
        private void SelectTag(int id)
        {
            tagToAdd.Add(id);
        }
        private void UnselectTag(int id)
        {
            tagToAdd.Remove(id);
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