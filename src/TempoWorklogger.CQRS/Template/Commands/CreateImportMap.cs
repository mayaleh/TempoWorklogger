using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.CQRS.Template.Commands
{
    public record CreateImportMapCommand(ImportMap ImportMap) : IRequest<unitResult>;

    public class CreateImportMapHandler : IRequestHandler<CreateImportMapCommand, unitResult>
    {
        public async Task<unitResult> Handle(CreateImportMapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var importMap = request.ImportMap;
                
                if (string.IsNullOrWhiteSpace(importMap.Name))
                {
                    return unitResult.Failed(new Exception("Name is required!"));
                }

                ModifyColumnsDefitionsName(importMap.ColumnDefinitions);
                // TODO
                // create new
                /*StorageService.ImportMapTemplate.Read()
                    .Bind(templates =>
                    {
                        templates.Add(Model);
                        return StorageService.ImportMapTemplate.Save(templates);
                    })
                    .Handle(
                        success =>
                        {
                            NavigationManager.NavigateTo("/templates");
                        },
                        fail => errorMessage = fail.Message
                    );
                return;*/
            }
            catch (Exception e)
            {
                return unitResult.Failed(e);
            }
        }

        private void ModifyColumnsDefitionsName(ICollection<ColumnDefinition> columnDefinitions)
        {
            if (columnDefinitions == null)
            {
                return;
            }

            foreach (var item in columnDefinitions)
            {
                item.Name = nameof(AttributeKeyVal) + item.Name;
            }
        }
    }
}
