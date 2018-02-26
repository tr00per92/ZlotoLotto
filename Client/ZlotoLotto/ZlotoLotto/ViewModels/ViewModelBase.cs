using Prism.Mvvm;
using Prism.Navigation;

namespace ZlotoLotto.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected ViewModelBase(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        protected INavigationService NavigationService { get; }

        private string title;
        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                this.SetProperty(ref this.isBusy, value);
                this.RaiseCommandsCanExecuteChanged();
            }
        }

        private string message;
        public string Message
        {
            get => this.message;
            set => this.SetProperty(ref this.message, value);
        }

        private bool hasError;
        public bool HasError
        {
            get => this.hasError;
            set => this.SetProperty(ref this.hasError, value);
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        protected virtual void RaiseCommandsCanExecuteChanged()
        {
        }
    }
}
