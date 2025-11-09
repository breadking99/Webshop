using System.Net.Http.Headers;

namespace Web.Blazor.Services;

public class BaseService
{
    #region # Fields
    private readonly HttpClient httpClient;
    private static string token = string.Empty;
    #endregion

    #region Constructors
    public BaseService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        InitAuthHeader();
    }
    #endregion

    #region Methods (Private)
    private void InitAuthHeader()
    {
        string scheme = "bearer";
        AuthenticationHeaderValue authHeader = new(scheme, token);
        httpClient.DefaultRequestHeaders.Authorization = authHeader;
    }
    #endregion
}