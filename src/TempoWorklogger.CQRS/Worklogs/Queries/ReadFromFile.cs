using Maya.Ext.Rop;

namespace TempoWorklogger.CQRS.Worklogs.Queries
{
    using worklogsFileResult = Result<List<Result<Worklog, (Exception Exception, int RowNr)>>, Exception>;

    public record ReadFromFileQuery(
        Model.UI.FileInfo FileInfo,
        ImportMap ImportMap,
        Action<int> OnProgressChanged) : IRequest<worklogsFileResult>;
    public class ReadFromFileQueryHandler : IRequestHandler<ReadFromFileQuery, worklogsFileResult>
    {
        private readonly IFileReaderService fileReaderService;

        public ReadFromFileQueryHandler(IFileReaderService fileReaderService)
        {
            this.fileReaderService = fileReaderService;
        }

        public async Task<worklogsFileResult> Handle(ReadFromFileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var data = await fileReaderService.ReadWorklogFileAsync(
                    request.FileInfo.Content,
                    request.ImportMap,
                    request.OnProgressChanged);

                return worklogsFileResult.Succeeded(data);
            }
            catch (Exception e)
            {
                return worklogsFileResult.Failed(e);
            }
        }
    }
}
