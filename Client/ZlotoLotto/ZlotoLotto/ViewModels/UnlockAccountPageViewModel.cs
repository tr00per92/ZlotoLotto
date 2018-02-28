using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZlotoLotto.Services;
using ZlotoLotto.Views;

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
            this.GoToCreateCommand = new DelegateCommand(this.GoToCreate);
        }

        public IEnumerable<KeyValuePair<string, string>> Accounts { get; } = Settings.Accounts.ToList();
        public KeyValuePair<string, string> SelectedAccount { get; set; } = Settings.Accounts.FirstOrDefault();

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
            this.HasError = false;
            try
            {
                await this.accountService.UnlockAccount(this.SelectedAccount.Value, this.accountPassword);
                this.web3Service.Initialize(this.accountService.Account);
                await this.NavigationService.NavigateToMainAsync(this.accountService.Account.Address);
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

        public ICommand GoToCreateCommand { get; }
        private async void GoToCreate()
        {
            await this.NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(CreateAccountPage)}");
        }
    }
}
