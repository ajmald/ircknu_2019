using CsvHelper;
using IrkcnuApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System;

namespace IrkcnuApi.Services
{
    public class ImportService
    {

        private readonly IMongoCollection<Artikel> _artikels;
        private readonly IMongoDatabase _database;
        private readonly MongoClient _mongoClient;
        public ImportService(IIrckcnuDatabaseSettings settings)
        {
            _mongoClient = new MongoClient(settings.ConnectionString);
            _database = _mongoClient.GetDatabase(settings.DatabaseName);
            _artikels= _database.GetCollection<Artikel>(settings.ArtikelCollectionName);
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
                return JsonConvert.SerializeObject(listObjResult); 
        }


        //Read the contents of the json file, then deserialize it and map to a BsonDocument
        public void InsertDocumentsInCollection(string jsonFile)
        {
            string text = System.IO.File.ReadAllText(jsonFile);
            IEnumerable<BsonDocument> doc = BsonSerializer.Deserialize<BsonArray>(text).Select(p => p.AsBsonDocument);
            var collection = _database.GetCollection<BsonDocument>("Artikels");
            try
            {
                collection.InsertManyAsync(doc).GetAwaiter().GetResult();
            }
            catch(MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey) 
                {
                }
            }
        }

        

    }
}