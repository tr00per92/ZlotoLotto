using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using ZlotoLotto.Services;

namespace ZlotoLotto.ViewModels
{
    public class UnlockAccountPageViewModel : ViewModelBase
    {
        private readonly IAccountService accountService;

        public UnlockAccountPageViewModel(INavigationService navigationService, IAccountService accountService) 
            : base (navigationService)
        {
            this.accountService = accountService;
            this.Title = "Unlock Account";
            this.UnlockAccountCommand = new DelegateCommand(this.UnlockAccount);
        }

        private string accountPassword;
        public string AccountPassword
        {
            get => this.accountPassword;
            set => this.SetProperty(ref this.accountPassword, value);
        }

        public ICommand UnlockAccountCommand { get; set; }
        private async void UnlockAccount()
        {
            this.accountService.UnlockAccount(this.accountPassword);
            await this.NavigationService.NavigateToMainAsync();
        }
    }
}
