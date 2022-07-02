using Maya.Ext.Rop;
using TempoWorklogger.Library.Model;
using TempoWorklogger.Library.Model.Tempo;

namespace TempoWorklogger.Library.Service
{
    internal class OpenXmlService : IFileReaderService
    {
        public Task<List<Result<Worklog, (Exception Exception, int RowNr)>>> ReadWorklogFileAsync(MemoryStream fileStream, ImportMap importMap, Action<int> onProgressChanged)
        {
            throw new NotImplementedException();
        }
    }
}
