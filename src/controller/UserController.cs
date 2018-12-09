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
using usecase;

namespace controller
{
    public class UserController
    {
        public static Configuration config;

        [FunctionName ("createUser")]
        public static async Task<HttpResponseMessage> Create (
            [HttpTrigger (AuthorizationLevel.Function, "post", Route = "users")] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {
            init (context);

            log.LogInformation (req.ContentType);

            string requestBody = await new StreamReader (req.Body).ReadToEndAsync ();
            dynamic data = null;
            try
            {
                data = JsonConvert.DeserializeObject (requestBody);
            }
            catch (JsonReaderException)
            {
                return Responses.BadRequest ("invalid Body");
            }

            string name = data?.name;
            string description = data?.description;

            var request = new UserCreateRequest (name, description);
            UserCreateResponse response = await new UserCreateUseCase ().execute (request);

            return Responses.Created (JsonConvert.SerializeObject (response.User));
        }

        [FunctionName ("getUsers")]
        public static async Task<HttpResponseMessage> GetList (
            [HttpTrigger (AuthorizationLevel.Function, "get", Route = "users")] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {

            init (context);

            var documentList = await DocumentDBRepository<User>.ReadDocumentFeedAsync ();

            return Responses.Success (string.Join (",", documentList));
        }

        [FunctionName ("getUser")]
        public static async Task<HttpResponseMessage> Get (
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