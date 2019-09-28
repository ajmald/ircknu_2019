using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IrkcnuApi.Models
{
    public class Artikel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Key")]
        public string Key { get; set; }

        [BsonElement("ArtikelCode")]
        public int ArtikelCode { get; set; }
        
        [BsonElement("ColorCode")]
        public string ColorCode { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("DiscountedPrice")]
        public string DiscountedPrice { get; set; }

        [BsonElement("DeliveredIn")]
        public string DeliveredIn { get; set; }

        [BsonElement("Q1")]
        public string Q1 { get; set; }

        [BsonElement("Size")]
        public int Size { get; set; }

        [BsonElement("Color")]
        public string Color { get; set; }








        public string Category { get; set; }

        public string Author { get; set; }
    }
}