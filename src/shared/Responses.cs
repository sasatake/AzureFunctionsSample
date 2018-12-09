using System.Net;
using System.Net.Http;
using System.Text;

namespace shared
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

        public static HttpResponseMessage Created (string body)
        {
            return new HttpResponseMessage (HttpStatusCode.Created)
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