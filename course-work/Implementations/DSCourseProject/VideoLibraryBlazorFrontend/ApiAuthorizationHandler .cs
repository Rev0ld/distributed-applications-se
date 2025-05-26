using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

public class ApiAuthorizationHandler : DelegatingHandler
{
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ApiAuthorizationHandler> _logger;

    // Define your scopes here
    private readonly string[] _scopes = new[] { "api://e6de894a-7857-4ac4-babe-bb5731887f9f/Weather.Get" };

    public ApiAuthorizationHandler(
        ITokenAcquisition tokenAcquisition,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ApiAuthorizationHandler> logger)
    {
        _tokenAcquisition = tokenAcquisition;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        string? test = user.GetTenantId();

        if (user == null || !(user.Identity?.IsAuthenticated ?? false))
        {
            _logger.LogWarning("User is not authenticated. Cannot acquire token.");
            var msalException = new MsalUiRequiredException("user_null", "User is not authenticated.");
            throw new MicrosoftIdentityWebChallengeUserException(msalException, _scopes);
        }

        try
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        catch (MsalUiRequiredException ex)
        {
            _logger.LogWarning(ex, "Access token could not be acquired silently; user interaction required.");
            // Throw with the original exception and the scopes to trigger a challenge
            throw new MicrosoftIdentityWebChallengeUserException(ex, _scopes);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

