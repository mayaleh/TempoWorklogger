using Maya.Ext.Rop;
using TempoWorklogger.Contract.Services;
using TempoWorklogger.Dto;
using TempoWorklogger.Dto.Storage;
using TempoWorklogger.Dto.Tempo;

namespace TempoWorklogger.Service
{
    public class FileReaderService : IFileReaderService
    {
        public async Task<List<Result<Worklog, (Exception Exception, int RowNr)>>> ReadWorklogFileAsync(MemoryStream fileStream, ImportMap importMap, Action<int> onProgressChanged)
        {
            // maybe redundant - should be in CQRS
            return importMap.FileType switch
            {
                FileTypeKinds.Xlsx => await new ExcelReaderService().ReadWorklogFileAsync(fileStream, importMap, onProgressChanged),
                _ => throw new NotImplementedException()
            };
        }
    }
}
