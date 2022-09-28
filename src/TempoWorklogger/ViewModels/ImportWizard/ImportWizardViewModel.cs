﻿using Microsoft.AspNetCore.Components.Forms;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;
using TempoWorklogger.Model.UI;

namespace TempoWorklogger.ViewModels.ImportWizard
{
    public class ImportWizardViewModel : IImportWizardViewModel
    {
        public ImportWizardState ImportWizardState { get; }

        public IImportWizardActions Actions { get; }
        
        public IImportWizardCommands Commands { get; }

        public IBrowserFile SelectedFile { get; set;  }

        public ImportWizardViewModel()
        {
            ImportWizardState = new();
            Actions = new ImportWizardActions(this);
            Commands = new ImportWizardCommands(this);
        }
    }
}
