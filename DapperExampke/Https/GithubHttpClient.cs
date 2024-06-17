using DapperHttps.Services;

namespace DapperHttps.Https;


public class GithubHttpClient : BaseHttpClient
{
    public override HttpClient Client { get; protected set; }

    public GithubHttpClient (HttpClient client, ILoggerService logService) : base(client, logService) { }
}