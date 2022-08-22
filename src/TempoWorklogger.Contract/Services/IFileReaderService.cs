using Maya.Ext.Rop;
using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.Contract.Services
{
    public interface IFileReaderService
    {
        Task<List<Result<Worklog, (Exception Exception, int RowNr)>>> ReadWorklogFileAsync(MemoryStream fileStream, ImportMap importMap, Action<int> onProgressChanged);
    }
}
