using Microsoft.Identity.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

public class ApiAuthorizationHandler : DelegatingHandler
{
    private readonly ITokenAcquisition _tokenAcquisition;

    public ApiAuthorizationHandler(ITokenAcquisition tokenAcquisition)
    {
        _tokenAcquisition = tokenAcquisition;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Specify the scopes required by the downstream API
        string[] scopes = new string[] { "api://e6de894a-7857-4ac4-babe-bb5731887f9f/Weather.Get" };

        // Acquire the access token
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        // Attach the token to the request
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
