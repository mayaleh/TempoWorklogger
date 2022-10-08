namespace TempoWorklogger.Library.Service
{
    public class StorageService : IStorageService
    {
        public Storage.ImportMapTemplateService ImportMapTemplate { get; }

        public StorageService()
        {
            this.ImportMapTemplate = new Storage.ImportMapTemplateService();
        }
    }
}
