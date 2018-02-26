using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using ZlotoLotto.Services;

namespace ZlotoLotto.ViewModels
{
    public class UnlockAccountPageViewModel : ViewModelBase
    {
        private readonly IAccountService accountService;
        private readonly IWeb3Service web3Service;

        public UnlockAccountPageViewModel(INavigationService navigationService, IAccountService accountService, IWeb3Service web3Service) 
            : base (navigationService)
        {
            this.accountService = accountService;
            this.web3Service = web3Service;
            this.Title = "Unlock Account";
            this.UnlockAccountCommand = new DelegateCommand(this.UnlockAccount, this.CanUnlockAccount);
        }

        private string accountPassword;
        public string AccountPassword
        {
            get => this.accountPassword;
            set
            {
                this.SetProperty(ref this.accountPassword, value);
                this.UnlockAccountCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand UnlockAccountCommand { get; }
        private async void UnlockAccount()
        {
            this.IsBusy = true;
            try
            {
                await this.accountService.UnlockAccount(this.accountPassword);
                this.web3Service.Initialize(this.accountService.Account);
                await this.NavigationService.NavigateToMainAsync();
            }
            catch
            {
                this.HasError = true;
                this.AccountPassword = null;
            }

            this.IsBusy = false;
        }
        private bool CanUnlockAccount()
        {
            return !string.IsNullOrWhiteSpace(this.AccountPassword);
        }
    }
}
