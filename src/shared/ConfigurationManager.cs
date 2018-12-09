using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace shared
{
    public static class ConfigurationManager
    {
        private static object Lock { get; } = new object ();
        private static Configuration Configuration { get; set; }
        public static Configuration GetConfiguration (ExecutionContext context)
        {
            if (Configuration == null)
            {
                lock (Lock)
                {
                    if (Configuration == null)
                    {
                        Configuration = new Configuration ();
                        new ConfigurationBuilder ()
                            .SetBasePath (context.FunctionAppDirectory)
                            .AddJsonFile ("local.settings.json", true)
                            .AddEnvironmentVariables ()
                            .Build ()
                            .Bind ("Values", Configuration);
                    }
                }

            }

            return Configuration;
        }
    }

    public class Configuration
    {
        public string endpoint { get; set; }
        public string authKey { get; set; }
        public string databaseId { get; set; }
        public string collectionId { get; set; }

        public override string ToString ()
        {
            return new { endpoint = endpoint, authKey = authKey, databaseId = databaseId, collectionId = collectionId }.ToString ();
        }

    }
}