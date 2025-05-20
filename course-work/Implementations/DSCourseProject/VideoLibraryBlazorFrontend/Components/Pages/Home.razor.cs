
using Common.Entities;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace VideoLibraryBlazorFrontend.Components.Pages
{
    public partial class Home
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        private List<Author> authors = new();

        public class Author
        {
            [JsonPropertyName("firstName")]
            public string FirstName { get; set; }

            [JsonPropertyName("middleName")]
            public string MiddleName { get; set; }

            [JsonPropertyName("lastName")]
            public string LastName { get; set; }

            [JsonPropertyName("biography")]
            public string Biography { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            //var requestBody = new
            //{
            //    pager = new
            //    {
            //        page = 0,
            //        itemsPerPage = 0
            //    },
            //    filter = new
            //    {
            //        firstName = "",
            //        middleName = "",
            //        lastName = "",
            //        biography = "",
            //        orderBy = "",
            //        orderDir = ""
            //    }
            //};

            //var jsonContent = new StringContent(
            //    JsonSerializer.Serialize(requestBody),
            //    Encoding.UTF8,
            //    "application/json"
            //);

            //var response = await HttpClient.PostAsync("https://localhost:5000/api/Authors/get", jsonContent);

            //if (response.IsSuccessStatusCode)
            //{
            //    var json = await response.Content.ReadAsStringAsync();
            //    using var doc = JsonDocument.Parse(json);

            //    var itemsElement = doc.RootElement
            //                          .GetProperty("data")
            //                          .GetProperty("items");

            //    authors = JsonSerializer.Deserialize<List<Author>>(itemsElement);
            //}
            //else
            //{
            //    var error = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine($"Error: {error}");
            //}
        }

    }
}