using Maya.Ext.Rop;
using TempoWorklogger.Dto.Storage;
using TempoWorklogger.Dto.Tempo;

namespace TempoWorklogger.Contract.Services
{
    public interface IFileReaderService
    {
        Task<List<Result<Worklog, (Exception Exception, int RowNr)>>> ReadWorklogFileAsync(MemoryStream fileStream, ImportMap importMap, Action<int> onProgressChanged);
    }
}
