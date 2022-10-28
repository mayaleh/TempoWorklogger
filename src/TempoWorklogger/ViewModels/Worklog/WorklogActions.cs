using Maya.Ext;
using Maya.Ext.Func.Rop;
using TempoWorklogger.Contract.UI.ViewModels.Worklog;
using TempoWorklogger.Model.Db;

namespace TempoWorklogger.ViewModels.Worklog
{
    using unitResult = Maya.Ext.Rop.Result<Unit, Exception>;

    public class WorklogActions : IWorklogActions
    {
        private readonly WorklogViewModel viewModel;
        private bool isEditing = false;

        public WorklogActions(WorklogViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public Task<Unit> AddAttribute(CustomAttributeKeyVal customAttributeKeyVal)
        {
            viewModel.WorklogModel.Attributes.Add(customAttributeKeyVal);
            return Task.FromResult(Unit.Default);
        }

        public async Task<Unit> Load(long? id)
        {
            if (id == null)
            {
                viewModel.WorklogModel = new Model.Db.Worklog()
                {
                    StartTime = DateTime.Now.AddMinutes(0),
                    EndTime = DateTime.Now.AddMinutes(15)
                };
                return Unit.Default;
            }

            return await viewModel.Mediator.Send(new CQRS.Worklogs.Queries.GetWorklogByIdQuery(id.Value))
                .EitherAsync(
                    success =>
                    {
                        this.isEditing = true;
                        viewModel.WorklogModel = success;
                        return Task.FromResult(unitResult.Succeeded(Unit.Default));
                    },
                    async failure =>
                    {
                        Console.WriteLine(failure.Message);
                        await viewModel.NotificationService.ShowError($"Failled to load import template with ID: {id}. Message: {failure.Message}");
                        return unitResult.Failed(failure);
                    }
                ).ValueOrAsync(Unit.Default);
        }

        public Task<Unit> RemoveAttribute(CustomAttributeKeyVal attributeKeyVal)
        {
            viewModel.WorklogModel.Attributes.Remove(attributeKeyVal);
            return Task.FromResult(Unit.Default);
        }

        public async Task<Unit> Save()
        {
            return await ExecuteSaveRequest()
                .EitherAsync(
                    async success =>
                    {
                        await viewModel.NotificationService.ShowSuccess($"Successfully saved the changes.");
                        if(this.isEditing == false) viewModel.GoToEditWorklog(viewModel.WorklogModel.Id);
                        return unitResult.Succeeded(Unit.Default);
                    },
                    async failure =>
                    {
                        Console.WriteLine(failure.Message);
                        await viewModel.NotificationService.ShowError($"Failled to save changes. Message: {failure.Message}");
                        return unitResult.Failed(failure);
                    }
                ).ValueOrAsync(Unit.Default);
        }

        private async Task<unitResult> ExecuteSaveRequest()
        {
            return await (this.isEditing
                ? viewModel.Mediator.Send(new CQRS.Worklogs.Commands.UpdateWorklogCommand(viewModel.WorklogModel))
                : viewModel.Mediator.Send(new CQRS.Worklogs.Commands.CreateWorklogCommand(viewModel.WorklogModel)));
        }
    }
}
