using Maya.Ext.Rop;
using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Contract.Services
{
    public interface IFileReaderService
    {
        Task<List<Result<Worklog, (Exception Exception, int RowNr)>>> ReadWorklogFileAsync(MemoryStream fileStream, ImportMap importMap, Action<int> onProgressChanged);
    }
}
