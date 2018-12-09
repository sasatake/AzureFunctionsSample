using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using shared;

namespace functions
{
    public class UserController
    {
        public static Configuration config;

        [FunctionName ("createUser")]
        public static async Task<HttpResponseMessage> createUser (
            [HttpTrigger (AuthorizationLevel.Function, "post", Route = "users")] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {
            init (context);

            User user = new User ()
            {
                Name = "test User",
                Description = "first User"
            };

            var document = await DocumentDBRepository<User>.CreateItemAsync (user);

            log.LogInformation (document.ToString ());

            return Responses.Success (document.ToString ());
        }

        [FunctionName ("getUsers")]
        public static async Task<HttpResponseMessage> getUsers (
            [HttpTrigger (AuthorizationLevel.Function, "get", Route = "users")] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {

            init (context);

            var documentList = await DocumentDBRepository<User>.ReadDocumentFeedAsync ();

            return Responses.Success (string.Join (",", documentList));
        }

        [FunctionName ("getUser")]
        public static async Task<HttpResponseMessage> getUser (
            [HttpTrigger (AuthorizationLevel.Function, "get", Route = "users/{id}")] HttpRequest req,
            String id,
            ExecutionContext context,
            ILogger log)
        {

            init (context);

            log.LogInformation ("id: " + id);

            User user = await DocumentDBRepository<User>.GetItemAsync (id);

            return Responses.Success (JsonConvert.SerializeObject (user));
        }

        private static void init (ExecutionContext context)
        {
            if (config == null)
            {
                config = ConfigurationManager.GetConfiguration (context);
            }
            DocumentDBRepository<User>.Initialize (config);
        }

    }
}