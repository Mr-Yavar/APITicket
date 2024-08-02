
using System.Net;
using RestSharp;

namespace Services
{
    public static class RequestHelper
    {
        public static async Task<T> CallApi<T>(string apiUrl, object value,string accessToken ,bool isToken = false) where T : class
        {
            var url = new Uri(apiUrl);
            var baseHost = $"{url.Scheme}://{url.Authority}";
            var method = $"{url.AbsolutePath}";
            var _client = new RestClient(baseHost) ;
            var request = new RestRequest(method, RestSharp.Method.Post);
            if (isToken)
                request.AddObject(value);
            else
            {
                _client.AddDefaultHeader("Authorization", $"bearer {accessToken}");
                request.AddJsonBody(value);
            }
            var response = await _client.ExecuteAsync<T>(request);
            if (method.Contains("verify"))
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = new  { Message = "پرداخت شما با موفقیت انجام پذیرفت" };
                    return result as T;
                }
                else
                {
                    var result = new { Message = response.Content };
                    return result as T;
                }
            return response.Data;
        }
    }
}
