namespace TempoWorklogger.CQRS.Worklogs.Commands
{
    public record SendToTempoCommand(ImportMap ImportMap, Worklog Worklog) : IRequest<unitResult>;

    public class SendToTempoCommandHandler : IRequestHandler<SendToTempoCommand, unitResult>
    {
        public async Task<unitResult> Handle(SendToTempoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Map DB.Worklog to Tempo.Worklog based on ImportMap
                // send to Tempo API
                // Create log
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return unitResult.Failed(e);
            }
        }

        private async Task<int> CreateSendLog()
        {
            // Create log to Worklog DB model about the send to tempo attempt
            throw new NotImplementedException();
        }
    }
}
