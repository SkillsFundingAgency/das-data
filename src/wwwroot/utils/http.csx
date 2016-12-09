using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Net;


public interface IHttp
{
    T Get<T>(string url) where T : class, new();
}


public class FunctionsHttp : IHttp
{
    string _keyName;

    public FunctionsHttp(string keyName)
    {
        _keyName = keyName;
    }

    public static string GetEnvironmentVariable(string name)
    {
        return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }

    public T Get<T>(string url) where T : class, new()
    {
        var h = new Http();
        return h.Get<T>(url, headers:new Dictionary<string, string> {{ "x-functions-key", GetEnvironmentVariable(_keyName) } });
    }
}

public class Http : IHttp
{

    public bool FollowRedirects { get; set; }
    public IDictionary<string, string> DefaultHeaders { get; set; }
    public IList<Cookie> DefaultCookies { get; set; }
    public IAuthenticator Authenticator { get; set; }

    public IRestResponse Get(string url, IDictionary<string, string> headers = null, IList<Cookie> cookies = null, IEnumerable<Parameter> parameters = null)
    {
        var result = CreateRequest(url, Method.GET, headers, cookies, parameters);
        var response = result.Item1.Execute(result.Item2);
        return response;
    }

    public T Get<T>(string url) where T : class, new()
    {
        return Get<T>(url, null, null, null);
    }

    public T Get<T>(string url, IDictionary<string, string> headers = null, IList<Cookie> cookies = null, IEnumerable<Parameter> parameters = null) where T : class, new()
    {
        var result = CreateRequest(url, Method.GET, headers, cookies, parameters);
        var response = result.Item1.Execute<T>(result.Item2);
        return response.Data;
    }

    public IRestResponse Delete(string url, IDictionary<string, string> headers = null, IList<Cookie> cookies = null, IEnumerable<Parameter> parameters = null)
    {
        var result = CreateRequest(url, Method.DELETE, headers, cookies, parameters);
        var response = result.Item1.Execute(result.Item2);
        return response;
    }

    public IRestResponse Put<T>(string url, T data, IDictionary<string, string> headers = null, IList<Cookie> cookies = null, IEnumerable<Parameter> parameters = null)
    {
        var result = CreateRequest(url, Method.PUT, headers, cookies, parameters);

        result.Item2.AddJsonBody(data);

        var response = result.Item1.Execute(result.Item2);

        return response;
    }

    public IRestResponse Post<T>(string url, T data, IDictionary<string, string> headers = null, IList<Cookie> cookies = null, IEnumerable<Parameter> parameters = null)
    {
        var result = CreateRequest(url, Method.POST, headers, cookies, parameters);

        result.Item2.AddJsonBody(data);

        var response = result.Item1.Execute(result.Item2);

        return response;
    }

    private Tuple<RestClient, RestRequest> CreateRequest(string url, Method method, IDictionary<string, string> headers = null, IList<Cookie> cookies = null, IEnumerable<Parameter> parameters = null)
    {
        var uri = new Uri(url);
        var host = uri.GetLeftPart(UriPartial.Authority);
        var resource = uri.PathAndQuery;

        var client = new RestClient(host);
        var cookieContainer = new CookieContainer();

        if (DefaultCookies != null)
        {
            foreach (var cookie in DefaultCookies)
            {
                cookieContainer.Add(cookie);
            }
        }

        if (cookies != null)
        {
            foreach (var cookie in cookies)
            {
                cookieContainer.Add(cookie);
            }
        }

        client.CookieContainer = cookieContainer;
        client.FollowRedirects = FollowRedirects;

        var request = new RestRequest(resource, method);

        if (parameters != null)
        {
            foreach (Parameter p in parameters)
            {
                request.AddParameter(p);
            }
        }

        if (DefaultHeaders != null)
        {
            foreach (var item in DefaultHeaders)
            {
                request.AddHeader(item.Key, item.Value);
            }
        }

        if (Authenticator != null)
        {
            client.Authenticator = Authenticator;
        }

        if (headers != null)
        {
            foreach (var item in headers)
            {
                request.AddHeader(item.Key, item.Value);
            }
        }

        return Tuple.Create(client, request);
    }
}