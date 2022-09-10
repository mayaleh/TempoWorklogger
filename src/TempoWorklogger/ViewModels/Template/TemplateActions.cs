using Maya.Ext;
using Maya.Ext.Func.Rop;
using TempoWorklogger.Contract.UI.ViewModels.Template;

namespace TempoWorklogger.ViewModels.Template
{
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
                return Unit.Default;
            }

            return await viewModel.Mediator.Send(new CQRS.Template.Queries.GetImportMapByIdQuery(id.Value))
                .HandleAsync(
                    success => {
                        this.isEditing = true;
                        viewModel.ImportMapModel = success;
                        return Task.FromResult(Unit.Default);
                    },
                    failure =>
                    {
                        Console.WriteLine(failure.Message);
                        // TODO: notify UI
                        return Unit.Default;
                    }
                );
        }

        public Task<Unit> RemoveAttribute(Model.Db.ColumnDefinition columnDefinition)
        {
            viewModel.AttributesModel.Remove(columnDefinition);
            return Task.FromResult(Unit.Default);
        }

        public async Task<Unit> Save()
        {
            return await ExecuteSaveRequest()
                .HandleAsync(
                    success => {
                        // TODO: Notify UI
                        return Task.FromResult(Unit.Default);
                    },
                    failure =>
                    {
                        Console.WriteLine(failure.Message);
                        // TODO: notify UI
                        return Unit.Default;
                    }
                );
        }

        private async Task<unitResult> ExecuteSaveRequest()
        {
            return await (this.isEditing
                ?  viewModel.Mediator.Send(new CQRS.Template.Commands.UpdateImportMapCommand(viewModel.ImportMapModel, viewModel.AttributesModel))
                :  viewModel.Mediator.Send(new CQRS.Template.Commands.CreateImportMapCommand(viewModel.ImportMapModel, viewModel.AttributesModel)));
        }
    }
}
