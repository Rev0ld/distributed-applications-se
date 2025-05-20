using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared;
using VideoLibraryBlazorFrontend.Shared.FormatsModels;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class FormatsEdit
    {
        [Parameter]
        public int FormatId { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        private FormatsIM? Format;

        protected override async Task OnInitializedAsync()
        {
            var response = await HttpClient.GetFromJsonAsync<ApiResponse<FormatsIM>>($"https://localhost:7209/api/Formats/{FormatId}");
            if (response != null && response.Success)
            {
                Format = response.Data;
            }
        }

        private async Task HandleValidSubmit()
        {
            var response = await HttpClient.PutAsJsonAsync($"https://localhost:7209/api/Formats/{FormatId}", Format);

            if (response.IsSuccessStatusCode)
            {
                NavManager.NavigateTo("/formats");
            }

        }

        private void Cancel()
        {
            NavManager.NavigateTo("/formats");
        }
    }
}