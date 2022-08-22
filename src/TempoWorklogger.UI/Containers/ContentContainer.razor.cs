using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace TempoWorklogger.UI.Containers
{
    public partial class ContentContainer
    {
        [Parameter]
        public bool IsLoading { get; set; }

        [Parameter]
        public bool IsBusy { get; set; }

        [Parameter]
        public RenderFragment? StaticContent { get; set; }

        [Parameter]
        public RenderFragment? Content { get; set; }

        //[Parameter]
        //public INotifyMessage NotifyMessage { get; set; } = null!;


        //private NotifyCenter? notifyCenter;

        protected override void OnParametersSet()
        {
            //if (NotifyMessage != null)
            //{
            //    this.notifyCenter = new Models.NotifyCenter(NotifyMessage, MessengerService);
            //}

            base.OnParametersSet();
        }
    }
}
