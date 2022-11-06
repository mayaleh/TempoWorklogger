using SQLite;

namespace TempoWorklogger.Model.Db
{
    public class IntegrationSettings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Name of the import
        /// </summary>
        [MaxLength(255)]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Access token of the Tempo importer account
        /// </summary>
        [MaxLength(512)]
        public string AccessToken { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(512)]
        public string AuthorAccountToken { get; set; } = null!;

        /// <summary>
        /// Endpoint.
        /// </summary>
        [MaxLength(512)]
        public string Endpoint { get; set; } = null!;
    }
}
