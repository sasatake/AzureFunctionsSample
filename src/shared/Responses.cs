using System.Net;
using System.Net.Http;
using System.Text;

namespace AzureFunctionsSample.src.shared
{
    public class Responses
    {
        private static string contentType = "application/json";
        private static string failedContentType = "application/problem+json";

        public static HttpResponseMessage Success (string body)
        {
            return new HttpResponseMessage (HttpStatusCode.OK)
            {
                Content = new StringContent (body, Encoding.UTF8, contentType)
            };
        }

        public static HttpResponseMessage BadRequest (string body)
        {
            return new HttpResponseMessage (HttpStatusCode.BadRequest)
            {
                Content = new StringContent (body, Encoding.UTF8, failedContentType)
            };
        }

    }
}