using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Linq;

namespace AzureDatabaseComparison
{
    public class CosmosRepository
    {
        private string _connectionUri;
        private string _databaseName;
        private string _collectionName;
        private string _key;
        private DocumentClient _client;
        private Uri _link;

        public CosmosRepository(string connUri, string dbName, string coll, string key)
        {
            _connectionUri = connUri;
            _databaseName = dbName;
            _collectionName = coll;
            _key = key;

            _client = new DocumentClient(new Uri(_connectionUri), _key);

            var db = GetOrCreateDatabaseAsync(_client, _databaseName);
            var collection = GetOrCreateCollectionAsync(_client, _databaseName, _collectionName);
            _link = UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName);
        }

        public async Task<Document> CreateDoc(object document)
        {
            var doc = await _client.CreateDocumentAsync(_link, document);
            return doc;
        }
        
        // ***********************************************************************
        // Below are functions swiped from CosmosDB C# sample (CosmosSample.cs)
        // ***********************************************************************
        
        private static async Task<DocumentCollection> GetOrCreateCollectionAsync(DocumentClient client, string databaseId, string collectionId)
        {
            var databaseUri = UriFactory.CreateDatabaseUri(databaseId);

            DocumentCollection collection = client.CreateDocumentCollectionQuery(databaseUri)
                .Where(c => c.Id == collectionId)
                .AsEnumerable()
                .FirstOrDefault();

            if (collection == null)
            {
                collection = await client.CreateDocumentCollectionAsync(databaseUri, new DocumentCollection { Id = collectionId });
            }

            return collection;
        }

        private static async Task<Database> GetOrCreateDatabaseAsync(DocumentClient client, string databaseId)
        {
            var databaseUri = UriFactory.CreateDatabaseUri(databaseId);

            Database database = client.CreateDatabaseQuery()
                .Where(db => db.Id == databaseId)
                .ToArray()
                .FirstOrDefault();

            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database { Id = databaseId });
            }

            return database;
        }
    }
}
