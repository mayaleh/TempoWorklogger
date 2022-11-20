using Maya.Ext;
using Maya.Ext.Func.Rop;
using TempoWorklogger.Contract.UI.ViewModels.Template;

namespace TempoWorklogger.ViewModels.Template
{
    using ColumnDefinition = Model.Db.ColumnDefinition;
    using unitResult = Maya.Ext.Rop.Result<Unit, Exception>;
    internal class TemplateActions : ITemplateActions
    {
        private readonly TemplateViewModel viewModel;

        private bool isEditing = false;

        public TemplateActions(TemplateViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public Task<Unit> AddAttribute()
        {
            viewModel.AttributesModel.Add(new Model.Db.ColumnDefinition());
            return Task.FromResult(Unit.Default);
        }

        public async Task<Unit> Load(int? id = null)
        {
            viewModel.ImportMapModel = new();
            viewModel.AttributesModel = new();

            if (id == null)
            {
                await viewModel.Mediator.Send(new CQRS.ColumnDefinitions.Queries.GetAvailableColumnsQuery())
                    .EitherAsync(
                        success =>
                        {
                            viewModel.ImportMapModel.ColumnDefinitions = success;
                            return Task.FromResult(unitResult.Succeeded(Unit.Default));
                        },
                        async failure =>
                        {
                            await viewModel.NotificationService.ShowError($"Failled to load columns. Message: {failure.Message}");
                            return unitResult.Failed(failure);
                        }
                    );
                return Unit.Default;
            }

            return await viewModel.Mediator.Send(new CQRS.Template.Queries.GetImportMapByIdQuery(id.Value))
                .EitherAsync(
                    success =>
                    {
                        this.isEditing = true;
                        viewModel.ImportMapModel = success;
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

        public Task<Unit> RemoveAttribute(Model.Db.ColumnDefinition columnDefinition)
        {
            viewModel.AttributesModel.Remove(columnDefinition);
            return Task.FromResult(Unit.Default);
        }

        public async Task<Unit> Save()
        {
            return await ExecuteSaveRequest()
                .EitherAsync(
                    async success =>
                    {
                        await viewModel.NotificationService.ShowSuccess($"Successfully saved the changes.");
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
                ? viewModel.Mediator.Send(new CQRS.Template.Commands.UpdateImportMapCommand(viewModel.ImportMapModel, viewModel.AttributesModel))
                : viewModel.Mediator.Send(new CQRS.Template.Commands.CreateImportMapCommand(viewModel.ImportMapModel, viewModel.AttributesModel)));
        }
    }
}
