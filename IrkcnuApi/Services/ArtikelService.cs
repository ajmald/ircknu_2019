using IrkcnuApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace IrkcnuApi.Services
{
    public class ArtikelService
    {
        private readonly IMongoCollection<Artikel> _artikels;

        public ArtikelService(IIrckcnuDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _artikels= database.GetCollection<Artikel>(settings.ArtikelCollectionName);
        }

        public List<Artikel> Get() =>
            _artikels.Find(artikel => true).ToList();

        public Artikel Get(string id) =>
            _artikels.Find<Artikel>(artikel => artikel.Id == id).FirstOrDefault();

        public Artikel Create(Artikel artikel)
        {
            _artikels.InsertOne(artikel);
            return artikel;
        }
        public string CreateArtikels(List<Artikel> artikels)
        {
            try
            {
                _artikels.InsertManyAsync(artikels).GetAwaiter().GetResult();
            }
            catch(MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey) 
                {
                    return "Duplicate key : KO";
                }
            }
            return "Ok";
        }
        public void Update(string id, Artikel artikelIn) =>
            _artikels.ReplaceOne(artikel => artikel.Id == id, artikelIn);

        public void Remove(Artikel artikelIn) =>
            _artikels.DeleteOne(artikel => artikel.Id == artikelIn.Id);

        public void Remove(string id) => 
            _artikels.DeleteOne(artikel => artikel.Id == id);
    }
}