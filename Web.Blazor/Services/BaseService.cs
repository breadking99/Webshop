using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Enums;
using Shared.Responses;
using System;
using System.Net.Http.Headers;
using System.Text;
using Web.Blazor.Extensions;

namespace Web.Blazor.Services;

public class BaseService
{

    #region Constructors
    public BaseService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        InitAuthHeader();
    }
    #endregion

    #region Properties
    public static bool IsLoggedIn => !string.IsNullOrEmpty(Token);
    protected StringBuilder ServiceAddress => GetServiceAddress();
    protected static string Token { get; set; } = string.Empty;
    #endregion

    #region Fields
    private readonly HttpClient httpClient;
    #endregion

    //? Later add Request Settings

    #region Methods (Protected, GET)
    // No generic
    protected async Task<Response> GetAsync(string uri)
        => await DoRequest(() => httpClient.GetAsync(uri));
    protected async Task<Response> GetAsync(object[]? parameters = null)
        => await GetAsync<object>(parameters);
    protected async Task<Response> GetAsync<TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
        => await GetAsync(GetUri(parameters, query));

    // TValue
    protected async Task<Response<TValue>> GetAsync<TValue>(string uri)
        => await DoRequest<TValue>(() => httpClient.GetAsync(uri));
    protected async Task<Response<TValue>> GetAsync<TValue>(object[]? parameters = null)
        => await GetAsync<TValue, object>(parameters);
    protected async Task<Response<TValue>> GetAsync<TValue, TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
        => await GetAsync<TValue>(GetUri(parameters, query));
    #endregion

    #region Methods (Protected, POST)
    // No generic
    protected async Task<Response> PostAsync(string uri)
        => await DoRequest(() => httpClient.PostAsync(uri, content: null));
    protected async Task<Response> PostAsync(object[]? parameters = null)
        => await PostAsync(GetUri<object>(parameters, null));
    protected async Task<Response> PostAsync<TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
        => await PostAsync(GetUri(parameters, query));

    // TRequest
    protected async Task<Response> PostAsync<TRequest>(string uri, TRequest? body) where TRequest : class
        => await DoRequest(() => httpClient.PostAsync(uri, GetContent(body)));
    protected async Task<Response> PostAsync<TRequest>(TRequest? body = null, object[]? parameters = null) where TRequest : class
        => await PostAsync(GetUri<object>(parameters, null), body);
    protected async Task<Response> PostAsync<TRequest, TQuery>(TRequest? body = null, object[]? parameters = null, TQuery? query = null)
        where TRequest : class where TQuery : class
        => await PostAsync(GetUri(parameters, query), body);

    // TValue
    protected async Task<Response<TValue>> PostAsync<TValue>(string uri)
        => await DoRequest<TValue>(() => httpClient.PostAsync(uri, content: null));
    protected async Task<Response<TValue>> PostAsync<TValue>(object[]? parameters = null)
        => await PostAsync<TValue, object>(parameters);
    protected async Task<Response<TValue>> PostAsync<TValue, TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
        => await PostAsync<TValue>(GetUri(parameters, query));

    // TRequest & TValue
    protected async Task<Response<TValue>> PostAsync<TRequest, TValue>(string uri, TRequest? body) where TRequest : class
        => await DoRequest<TValue>(() => httpClient.PostAsync(uri, GetContent(body)));
    protected async Task<Response<TValue>> PostAsync<TRequest, TValue>(TRequest? body = null, object[]? parameters = null) where TRequest : class
        => await PostAsync<TRequest, TValue>(GetUri<object>(parameters, null), body);
    protected async Task<Response<TValue>> PostAsync<TRequest, TValue, TQuery>(TRequest? body = null, object[]? parameters = null, TQuery? query = null)
        where TRequest : class where TQuery : class
        => await PostAsync<TRequest, TValue>(GetUri(parameters, query), body);
    #endregion

    #region Methods (Protected, PUT)
    // No generic
    protected async Task<Response> PutAsync(string uri)
        => await DoRequest(() => httpClient.PutAsync(uri, content: null));
    protected async Task<Response> PutAsync(object[]? parameters = null)
        => await PutAsync(GetUri<object>(parameters, null));
    protected async Task<Response> PutAsync<TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
        => await PutAsync(GetUri(parameters, query));

    // TRequest
    protected async Task<Response> PutAsync<TRequest>(string uri, TRequest? body) where TRequest : class
        => await DoRequest(() => httpClient.PutAsync(uri, GetContent(body)));
    protected async Task<Response> PutAsync<TRequest>(TRequest? body = null, object[]? parameters = null) where TRequest : class
        => await PutAsync(GetUri<object>(parameters, null), body);
    protected async Task<Response> PutAsync<TRequest, TQuery>(TRequest? body = null, object[]? parameters = null, TQuery? query = null)
        where TRequest : class where TQuery : class
        => await PutAsync(GetUri(parameters, query), body);

    // TValue
    protected async Task<Response<TValue>> PutAsync<TValue>(string uri)
        => await DoRequest<TValue>(() => httpClient.PutAsync(uri, content: null));
    protected async Task<Response<TValue>> PutAsync<TValue>(object[]? parameters = null)
        => await PutAsync<TValue, object>(parameters);
    protected async Task<Response<TValue>> PutAsync<TValue, TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
        => await PutAsync<TValue>(GetUri(parameters, query));

    // TRequest & TValue
    protected async Task<Response<TValue>> PutAsync<TRequest, TValue>(string uri, TRequest? body) where TRequest : class
        => await DoRequest<TValue>(() => httpClient.PutAsync(uri, GetContent(body)));
    protected async Task<Response<TValue>> PutAsync<TRequest, TValue>(TRequest? body = null, object[]? parameters = null) where TRequest : class
        => await PutAsync<TRequest, TValue>(GetUri<object>(parameters, null), body);
    protected async Task<Response<TValue>> PutAsync<TRequest, TValue, TQuery>(TRequest? body = null, object[]? parameters = null, TQuery? query = null)
        where TRequest : class where TQuery : class
        => await PutAsync<TRequest, TValue>(GetUri(parameters, query), body);
    #endregion

    #region Methods (Protected, DELETE)
    // No generic
    protected async Task<Response> DeleteAsync(string uri)
        => await DoRequest(() => httpClient.DeleteAsync(uri));
    protected async Task<Response> DeleteAsync(object[]? parameters = null)
        => await DeleteAsync(GetUri<object>(parameters, null));
    protected async Task<Response> DeleteAsync<TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
        => await DeleteAsync(GetUri(parameters, query));

    // TValue
    protected async Task<Response<TValue>> DeleteAsync<TValue>(string uri)
        => await DoRequest<TValue>(() => httpClient.DeleteAsync(uri));
    protected async Task<Response<TValue>> DeleteAsync<TValue>(object[]? parameters = null)
        => await DeleteAsync<TValue, object>(parameters);
    protected async Task<Response<TValue>> DeleteAsync<TValue, TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
        => await DeleteAsync<TValue>(GetUri(parameters, query));
    #endregion

    #region Methods (Protected, Helpers)
    protected virtual StringBuilder GetServiceAddress(params object[] parameters) => new StringBuilder().AppendJoin('/', parameters);
    protected StringBuilder GetRequestAddress(params object[] parameters) => ServiceAddress.AppendJoin('/', parameters);
    protected string GetUri<TQuery>(object[]? parameters = null, TQuery? query = null) where TQuery : class
    {
        StringBuilder sb;

        if (parameters == null || parameters.Length == 0) sb = ServiceAddress;
        else sb = ServiceAddress
            .Append('/')
            .AppendJoin('/', parameters);

        sb.AddQuery(query);
        string uri = sb.ToString();

        return uri;
    }
    #endregion

    #region Methods (Private)
    private void InitAuthHeader()
    {
        string scheme = "bearer";
        AuthenticationHeaderValue authHeader = new(scheme, Token);
        httpClient.DefaultRequestHeaders.Authorization = authHeader;
    }

    private static async Task<Response<TValue>> DoRequest<TValue>(Func<Task<HttpResponseMessage>> request)
    {
        try
        {
            HttpResponseMessage result = await request();

            return await GetContent<TValue>(result);
        }
        catch
        {
            return new(EResponseStatus.TimeOut);
        }
    }

    private static async Task<Response> DoRequest(Func<Task<HttpResponseMessage>> request)
    {
        try
        {
            HttpResponseMessage result = await request();

            return await GetContent(result);
        }
        catch
        {
            return new(EResponseStatus.TimeOut);
        }
    }

    private static async Task<Response<TValue>> GetContent<TValue>(HttpResponseMessage result)
    {
        if (result.IsSuccessStatusCode)
        {
            Response<TValue>? responseTypes = await HandleDifferentTypes<TValue>(result);
            if (responseTypes != null) return responseTypes;

            string json = await result.Content.ReadAsStringAsync();
            TValue? value = JsonConvert.DeserializeObject<TValue>(json);
            if (value == null) return new(EResponseStatus.DeserializeError);

            return new(value);
        }

        Response responseMessage = await GetContent(result);

        return new(responseMessage);
    }

    private static async Task<Response> GetContent(HttpResponseMessage result)
    {
        int statusCode = (int)result.StatusCode;
        string message = await result.Content.ReadAsStringAsync();

        return new(statusCode, message);
    }

    private static StringContent? GetContent<TRequest>(TRequest? request)
    {
        if (request == null) return null;
        string json = JsonConvert.SerializeObject(request, Formatting.Indented);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static async Task<Response<TValue>?> HandleDifferentTypes<TValue>(HttpResponseMessage result)
    {
        if (typeof(TValue) == typeof(string))
        {
            string str = await result.Content.ReadAsStringAsync();
            object? value = str as object;

            return new((TValue)value!);
        }

        return null;
    }
    #endregion
}