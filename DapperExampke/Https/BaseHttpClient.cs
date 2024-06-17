using System.Text;
using DapperHttps.Services;
using Newtonsoft.Json;

namespace DapperHttps.Https;

public class BaseHttpClient
{
    public virtual HttpClient Client { get; protected set; }
    public readonly ILoggerService _logService;

    public BaseHttpClient (HttpClient client, ILoggerService logService)
    {
        this.Client = client;
        this._logService = logService;
    }


    public async Task<TOut> PostAsync<T, TOut>(string relativeAddress, HttpMethod method, List<KeyValuePair<string, string>> postBody = null) where T : class
    {
        FormUrlEncodedContent content = new FormUrlEncodedContent(postBody);

        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(Client.BaseAddress + relativeAddress),
            Content = content
        };

        return await Send<TOut>(request);
    }


    public async Task<TOut> PostWithLogAsync<T, TOut>(int? userId, string apiName, string apiDesc, string relativeAddress, HttpMethod method, List<KeyValuePair<string, string>> postBody = null) where T : class
    {
        FormUrlEncodedContent content = new FormUrlEncodedContent(postBody);

        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(Client.BaseAddress + relativeAddress),
            Content = content
        };

        return await Send<TOut>(request, userId, apiName, apiDesc, JsonConvert.SerializeObject(postBody));
    }


    public async Task<TOut> MakeRequestAsync<T, TOut>(string relativeAddress, HttpMethod method, T postBody = null) where T : class
    {
        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(Client.BaseAddress + relativeAddress),
            Content = new StringContent(JsonConvert.SerializeObject(postBody == null ? (object)string.Empty : postBody), Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = Client.DefaultRequestHeaders.Authorization;
        
        return await Send<TOut>(request);
    }


    public async Task<TOut> MakeRequestAsync<T, TOut>(int? userId, string apiName, string apiDesc, string relativeAddress, HttpMethod method, T postBody = null) where T : class
    {
        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(Client.BaseAddress + relativeAddress),
            Content = new StringContent(JsonConvert.SerializeObject(postBody == null ? (object)string.Empty : postBody), Encoding.UTF8, "application/json")
        };

        return await Send<TOut>(request, userId, apiName, apiDesc, JsonConvert.SerializeObject(postBody));
    }


    private async Task<T> Send<T>(HttpRequestMessage request)
    {
        var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            var ex = await GetException(request, response);
            throw ex;
        }

        var result = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(result);
    }


    private async Task<T> Send<T>(HttpRequestMessage request, int? userId, string apiName, string apiDesc, string postBody)
    {
        try
        {
            var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
            {
                var ex = await GetException(request, response);
                
                await _logService.AddApiLog(userId, apiName, apiDesc, request.RequestUri.AbsoluteUri, postBody, (int)response.StatusCode, $"{ex.Message} {ex.InnerException?.Message}");
                
                throw ex;
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);

            var result = new JsonSerializer().Deserialize<T>(jsonReader);

            await _logService.AddApiLog(userId, apiName, apiDesc, request.RequestUri.AbsoluteUri, postBody, (int)response.StatusCode, JsonConvert.SerializeObject(result));

            return result;
        }
        catch (Exception ex)
        {
            await _logService.AddApiLog(userId, apiName, apiDesc, request.RequestUri.AbsoluteUri, postBody, 500, $"{ex.Message} {ex.InnerException?.Message}");
            
            throw ex;
        }
    }


    private async Task<HttpRequestException> GetException(HttpRequestMessage request, HttpResponseMessage response)
    {
        // throw an exception that has better info
        request.Headers.TryGetValues("Connection", out var headers);
        var content = response.Content != null ?
            await response.Content.ReadAsStringAsync() : "";

        var url = request.RequestUri.ToString();
        var statusCode = (int)response.StatusCode;
        var reasonPhrase = response.ReasonPhrase;

        var message =
            $"Url: {url}; ConnectionClose: {headers?.FirstOrDefault().ToString()}; " +
            $"Status: {statusCode} {reasonPhrase}; Content: {content}";

        var ex = new HttpRequestException(message);
        ex.Data.Add("Url", url);
        ex.Data.Add("StatusCode", statusCode);
        ex.Data.Add("ReasonPhrase", reasonPhrase);
        ex.Data.Add("Content", content);

        return ex;
    }
}