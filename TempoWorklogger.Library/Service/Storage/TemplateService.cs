using Maya.Ext;
using Maya.Ext.Rop;
using System.Text.Json;
using TempoWorklogger.Library.Model;

namespace TempoWorklogger.Library.Service.Storage
{
    public class ImportMapTemplateService : BaseStorageService
    {
        const string fileStorage = "templateMappingStorage.json";

        public Result<List<ImportMap>, Exception> Read()
        {
            try
            {
                var source = EnsureFileExist(fileStorage);
                var rawData = File.ReadAllText(source);
                return Result<List<ImportMap>, Exception>.Succeeded(JsonSerializer.Deserialize<List<ImportMap>>(rawData));
            }
            catch (Exception e)
            {
                return Result<List<ImportMap>, Exception>.Failed(e);
            }
        }

        public Result<Unit, Exception> Save(List<ImportMap> importMaps)
        {
            try
            {
                var source = EnsureFileExist(fileStorage);
                var serializedData = JsonSerializer.Serialize(importMaps);
                File.WriteAllText(source, serializedData);
                return Result<Unit, Exception>.Succeeded(Unit.Default);
            }
            catch (Exception e)
            {
                return Result<Unit, Exception>.Failed(e);
            }
        }

        public Result<Unit, Exception> DeleteStorage()
        {
            try
            {
                var storageFile = EnsureFileExist(fileStorage);
                File.Delete(storageFile);
                return Result<Unit, Exception>.Succeeded(Unit.Default);
            }
            catch (Exception e)
            {
                return Result<Unit, Exception>.Failed(e);
            }
        }
    }
}
