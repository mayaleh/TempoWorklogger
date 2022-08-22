using Maya.Ext.Rop;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TempoWorklogger.Contract.Services;
using TempoWorklogger.Library.Mapper.Tempo;
using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.Service
{
    public class ExcelReaderService : IFileReaderService
    {
        public Task<List<Result<Worklog, (Exception Exception, int RowNr)>>> ReadWorklogFileAsync(MemoryStream fileStream, ImportMap importMap, Action<int> onProgressChanged)
        {
            try
            {
                fileStream.Position = 0;

                var xssWorkbook = new XSSFWorkbook(fileStream);
                var sheet = xssWorkbook.GetSheetAt(0);

                var worklogsResults = new List<Result<Worklog, (Exception, int)>>();

                var startFrom = importMap.StartFromRow;
                if (sheet.PhysicalNumberOfRows > sheet.LastRowNum)
                {
                    var diff = sheet.PhysicalNumberOfRows - sheet.LastRowNum;
                    startFrom -= diff;
                }

                var total = sheet.LastRowNum - importMap.StartFromRow;
                var precentageDone = 0;
                total = total == 0 ? 1 : total;
                for (int i = startFrom; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);

                    if (row == null) continue; // empty row

                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue; // all cells in row are empty

                    worklogsResults.Add(row.MapRowByImportToWorklog(importMap));

                    precentageDone = 100 * i / total;
                    onProgressChanged.Invoke(precentageDone);
                }

                return Task.FromResult(worklogsResults);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
