using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using shared;

namespace functions
{
    public static class FirstFunction
    {
        [FunctionName ("FirstFunction")]
        public static HttpResponseMessage Run (
            [HttpTrigger (AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation ("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var myObj = new { name = name };
            var jsonToReturn = JsonConvert.SerializeObject (myObj);

            return (name != null && name != "") ? Responses.Success (jsonToReturn) : Responses.BadRequest ("Missing parameters");
        }
    }

}