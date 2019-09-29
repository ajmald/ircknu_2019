using CsvHelper;
using IrkcnuApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IrkcnuApi.Services
{
    public class ImportService
    {

        private readonly IMongoCollection<Artikel> _artikels;
        private readonly IMongoDatabase _database;
        private readonly MongoClient _mongoClient;

        private IIrckcnuDatabaseSettings settings;
        public ImportService(IIrckcnuDatabaseSettings settings)
        {
            _mongoClient = new MongoClient(settings.ConnectionString);
            _database = _mongoClient.GetDatabase(settings.DatabaseName);
            _artikels= _database.GetCollection<Artikel>(settings.ArtikelCollectionName);
        }

        public async void ImportFile(string fileName)
        {
            IrckcnuDatabaseSettings settings = new IrckcnuDatabaseSettings();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            var transformedFile = ConvertCsvFileToJsonObject(fileName);
            string text = System.IO.File.ReadAllText(transformedFile);
            var document = BsonSerializer.Deserialize<BsonDocument>(text);
            var collection = database.GetCollection<BsonDocument>("Artikel");
            await collection.InsertOneAsync(document);
    
        }
      
        public string ConvertCsvFileToJsonObject(string path) 
        {
                var csv = new List<string[]>();
                var lines = File.ReadAllLines(path);

                foreach (string line in lines)
                    csv.Add(line.Split(','));

                var properties = lines[0].Split(',');

                var listObjResult = new List<Dictionary<string, string>>();

                for (int i = 1; i < lines.Length; i++)
                {
                    var objResult = new Dictionary<string, string>();
                    for (int j = 0; j < properties.Length; j++)
                        objResult.Add(properties[j], csv[i][j]);

                    listObjResult.Add(objResult);
                }
                
                //var data = (from row in path.Split('\n') select row.Split(',')).ToList();
                return JsonConvert.SerializeObject(listObjResult,Formatting.Indented); 
        }

        public async void InsertDocumentsInCollection(string jsonFile)
        {
            string text = System.IO.File.ReadAllText(jsonFile);
            //var document = BsonSerializer.Deserialize<BsonDocument>(text);
            IEnumerable<BsonDocument> doc = BsonSerializer.Deserialize<BsonArray>(text).Select(p => p.AsBsonDocument);
            var collection = _database.GetCollection<BsonDocument>("Artikels");
            await collection.InsertManyAsync(doc);  
        }

    }
}