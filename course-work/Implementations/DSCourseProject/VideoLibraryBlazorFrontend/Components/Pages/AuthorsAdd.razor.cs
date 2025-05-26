using Microsoft.AspNetCore.Components;
using VideoLibraryBlazorFrontend.Shared.AuthorsModels;
using System.Text.Json.Serialization;
using Azure.Core;
using Common.Entities;
using VideoLibraryBlazorFrontend.Shared;
using Microsoft.Identity.Web;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class AuthorsAdd
    {
        [Inject]
        HttpClient HttpClient { get; set; }
        [Inject]
        NavigationManager NavManager { get; set; }

        private AuthorsIM Author = new AuthorsIM();
        private async Task AddToDataBase() 
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("https://localhost:7209/api/Authors", Author);

                if (response.IsSuccessStatusCode)
                {
                    Author.FirstName = null;
                    Author.MiddleName = null;
                    Author.LastName = null;
                    Author.Biography = null;
                }
                else
                { }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavManager.NavigateTo("authentication/login", forceLoad: true);
            }
            
        }
    }
}