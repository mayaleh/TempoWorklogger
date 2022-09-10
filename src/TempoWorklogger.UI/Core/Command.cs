using TempoWorklogger.Contract.UI;

namespace TempoWorklogger.UI.Core
{
    public class Command : BaseCommand, ICommand
    {
        private readonly Action action;

        public Command(Action action, IObservable<bool>? onCanExecute = null) : base()
        {
            this.action = action;
            CanExecute = true;

            onCanExecute?.Subscribe((v) =>
            {
                Console.WriteLine("COMMAND onCanExecute change: {0}", v);
                if (Executing) return;

                CanExecute = v;
            });
        }

        public void Execute()
        {
            if (!CanExecute) return;

            CanExecute = false;
            Executing = true;

            if (this.action != null)
            {
                action.Invoke();
            }

            Executing = false;
            CanExecute = true;
        }
    }

    public class Command<TParam> : BaseCommand, ICommand<TParam>
    {
        private readonly Maya.Ext.Func.Action<TParam> action;

        public Command(Maya.Ext.Func.Action<TParam> action, IObservable<bool>? onCanExecute = null) : base()
        {
            this.action = action;
            CanExecute = true;

            onCanExecute?.Subscribe((v) =>
            {
                Console.WriteLine("COMMAND onCanExecute change: {0}", v);
                if (Executing) return;

                CanExecute = v;
            });
        }

        public void Execute(TParam input)
        {
            if (!CanExecute) return;

            CanExecute = false;
            Executing = true;

            if (this.action != null)
            {
                action.Invoke(input);
            }

            Executing = false;
            CanExecute = true;
        }
    }
}
