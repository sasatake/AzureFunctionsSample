using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace shared
{
    public static class DocumentDBRepository<T> where T : class
    {
        private static string databaseId;
        private static string collectionId;
        private static DocumentClient client;

        public static async Task<T> GetItemAsync (string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync (UriFactory.CreateDocumentUri (databaseId, collectionId, id));
                return (T) (dynamic) document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<IEnumerable<Document>> ReadDocumentFeedAsync ()
        {
            string continuation = string.Empty;
            List<Document> results = new List<Document> ();
            do
            {

                FeedResponse<dynamic> response = await client.ReadDocumentFeedAsync (UriFactory.CreateDocumentCollectionUri (databaseId, collectionId),
                    new FeedOptions
                    {
                        MaxItemCount = 10, RequestContinuation = continuation
                    });

                foreach (var d in response)
                {
                    results.Add (d);
                }

                continuation = response.ResponseContinuation;

            }
            while (!string.IsNullOrEmpty (continuation));

            return results;
        }

        public static async Task<IEnumerable<T>> GetItemsAsync (Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T> (
                    UriFactory.CreateDocumentCollectionUri (databaseId, collectionId),
                    new FeedOptions { MaxItemCount = -1 })
                .Where (predicate)
                .AsDocumentQuery ();

            List<T> results = new List<T> ();
            while (query.HasMoreResults)
            {
                results.AddRange (await query.ExecuteNextAsync<T> ());
            }

            return results;
        }

        public static async Task<Document> CreateItemAsync (T item)
        {
            return await client.CreateDocumentAsync (UriFactory.CreateDocumentCollectionUri (databaseId, collectionId), item);
        }

        public static async Task<Document> UpdateItemAsync (string id, T item)
        {
            return await client.ReplaceDocumentAsync (UriFactory.CreateDocumentUri (databaseId, collectionId, id), item);
        }

        public static async Task DeleteItemAsync (string id)
        {
            await client.DeleteDocumentAsync (UriFactory.CreateDocumentUri (databaseId, collectionId, id));
        }

        public static void Initialize (Configuration config)
        {
            client = new DocumentClient (new Uri (config.endpoint), config.authKey);
            databaseId = config.databaseId;
            collectionId = config.collectionId;

            CreateDatabaseIfNotExistsAsync ().Wait ();
            CreateCollectionIfNotExistsAsync ().Wait ();
        }

        private static async Task CreateDatabaseIfNotExistsAsync ()
        {
            try
            {
                await client.ReadDatabaseAsync (UriFactory.CreateDatabaseUri (databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync (new Database { Id = databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync ()
        {
            try
            {
                await client.ReadDocumentCollectionAsync (UriFactory.CreateDocumentCollectionUri (databaseId, collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync (
                        UriFactory.CreateDatabaseUri (databaseId),
                        new DocumentCollection { Id = collectionId },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}