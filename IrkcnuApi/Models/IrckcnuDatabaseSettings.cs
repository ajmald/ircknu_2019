namespace IrkcnuApi.Models
{
    public class IrckcnuDatabaseSettings : IIrckcnuDatabaseSettings
    {
        public string ArtikelCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IIrckcnuDatabaseSettings
    {
        string ArtikelCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}