using Maya.Ext.Rop;
using Microsoft.AspNetCore.Components;
using System.Collections;
using TempoWorklogger.Library.Model;
using TempoWorklogger.Library.Model.Tempo;
using TempoWorklogger.Library.Service;

namespace TempoWorklogger.Pages.Templates
{
    public partial class TemplateForm
    {
        [Parameter]
        public string Name { get; set; } = string.Empty;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IStorageService StorageService { get; set; }

        private ImportMap Model { get; set; } = new ImportMap();

        private List<TempoWorklogger.Library.Model.ColumnDefinition> attributes = new List<TempoWorklogger.Library.Model.ColumnDefinition>();

        private string errorMessage = string.Empty;

        private bool isReady = false;

        protected override void OnInitialized()
        {

            if (string.IsNullOrWhiteSpace(Name) == false)
            {
                StorageService.ImportMapTemplate.Read()
                    .Handle(
                        success =>
                        {
                            var m = success.FirstOrDefault(x => x.Name == Name);
                            if (m == null)
                            {
                                errorMessage = "Not found";
                                return;
                            }
                            Model = m;
                            Model.ColumnDefinitions = m.ColumnDefinitions.Where(x => x.Name.StartsWith(nameof(AttributeKeyVal)) == false)
                                .ToList();
                            this.attributes = m.ColumnDefinitions.Where(x => x.Name.StartsWith(nameof(AttributeKeyVal)))
                                .Select(x =>
                                {
                                    x.Name = x.Name.Substring(nameof(AttributeKeyVal).Length);
                                    return x;
                                })
                                    .Select(x =>
                                {
                                    x.Name = x.Name.Substring(nameof(AttributeKeyVal).Length);
                                    return x;
                                })
                                .ToList();
                        },
                        fail => errorMessage = fail.Message
                    );

                this.isReady = true;

                return;
            }

            foreach (var property in typeof(Worklog).GetProperties())
            {
                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    continue;
                }

                Model.ColumnDefinitions.Add(new Library.Model.ColumnDefinition { Name = property.Name });
            }

            this.isReady = true;

            base.OnInitialized();
        }

        private void OnAddAttributeClicked()
        {
            attributes.Add(new Library.Model.ColumnDefinition());
        }

        private void OnRemoveAttributeClicked(Library.Model.ColumnDefinition colDef)
        {
            this.attributes.Remove(colDef);
        }

        private void OnSaveClicked()
        {
            this.isReady = false;
            attributes.ForEach(a =>
            {
                a.Name = nameof(AttributeKeyVal) + a.Name;
                Model.ColumnDefinitions.Add(a);
            });
            if (string.IsNullOrWhiteSpace(Name))
            {
                // create new
                StorageService.ImportMapTemplate.Read()
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
                return;
            }

            // update
            StorageService.ImportMapTemplate.Read()
                .Bind(templates =>
                { // lazy to update properties. Possible faiulure after add, this may cause loosing of the data
                    var remove = templates.First(x => x.Name == Name);
                    templates.Remove(remove);
                    return StorageService.ImportMapTemplate.Save(templates);
                })
                .Bind(_ => StorageService.ImportMapTemplate.Read())
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

            this.isReady = true;
        }
    }
}
