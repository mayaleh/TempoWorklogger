using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TempoWorklogger.UI.Dialogs
{
    public partial class DialogView
    {
        [Parameter]
        public string Identifier { get; set; } = null!;

        [Parameter]
        public string Title { get; set; } = null!;

        [Parameter]
        public EventCallback<MouseEventArgs> OnConfirmClicked { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs>? OnCancelClicked { get; set; }

        [Parameter]
        public string Width { get; set; } = "500px";

        [Parameter]
        public string? ConfirmButtonText { get; set; }

        [Parameter]
        public RenderFragment? Content { get; set; }

        [Parameter]
        public RenderFragment? ButtonsContent { get; set; }

        [Parameter]
        public bool IsConfirmDisabled { get; set; } = false;

        [Parameter]
        public bool IsCancelDisabled { get; set; } = false;

        private async Task DefaultCancelClicked(MouseEventArgs args) 
        {
            System.Console.WriteLine($"Dialog {Identifier}: Cancel clicked.");
            if (OnCancelClicked != null)
            {
                await OnCancelClicked.Value.InvokeAsync(args);
            }
        }
    }
}
