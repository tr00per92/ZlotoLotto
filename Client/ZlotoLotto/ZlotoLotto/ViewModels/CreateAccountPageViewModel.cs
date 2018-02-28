using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using ZlotoLotto.Services;

namespace ZlotoLotto.ViewModels
{
    public class CreateAccountPageViewModel : ViewModelBase
    {
        private readonly IAccountService accountService;
        private readonly IWeb3Service web3Service;

        public CreateAccountPageViewModel(INavigationService navigationService, IAccountService accountService, IWeb3Service web3Service) 
            : base (navigationService)
        {
            this.accountService = accountService;
            this.web3Service = web3Service;
            this.Title = "Create Account";
            this.ContinueCommand = new DelegateCommand(this.Continue);
            this.CreateAccountCommand = new DelegateCommand(this.CreateAccount, this.CanCreateAccount);
            this.RestoreAccountCommand = new DelegateCommand(this.RestoreAccount, this.CanRestoreAccount);
        }

        public ICommand ContinueCommand { get; }
        private async void Continue()
        {
            await this.NavigationService.NavigateToMainAsync(this.accountService.Account.Address);
        }

        private string newAccountPassword;
        public string NewAccountPassword
        {
            get => this.newAccountPassword;
            set
            {
                this.SetProperty(ref this.newAccountPassword, value);
                this.CreateAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private string restoreAccountPassword;
        public string RestoreAccountPassword
        {
            get => this.restoreAccountPassword;
            set
            {
                this.SetProperty(ref this.restoreAccountPassword, value);
                this.RestoreAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private string privateKey;
        public string PrivateKey
        {
            get => this.privateKey;
            set
            {
                this.SetProperty(ref this.privateKey, value);
                this.RestoreAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private string mnemonic;
        public string Mnemonic
        {
            get => this.mnemonic;
            set
            {
                this.SetProperty(ref this.mnemonic, value);
                this.RestoreAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private string restoreError;
        public string RestoreError
        {
            get => this.restoreError;
            set => this.SetProperty(ref this.restoreError, value);
        }

        public DelegateCommand CreateAccountCommand { get; }
        private async void CreateAccount()
        {
            this.IsBusy = true;
            var newAccountData = await this.accountService.CreateNew(this.NewAccountPassword);
            this.web3Service.Initialize(this.accountService.Account);
            this.IsBusy = false;
            this.Message = $"Account created successfully. Your address is '{this.accountService.Account.Address}'. Please write down your private key '{newAccountData.PrivateKey}' and your mnemonic phrase '{newAccountData.Mnemonic}'";
            this.NewAccountPassword = null;
        }
        private bool CanCreateAccount()
        {
            return !string.IsNullOrWhiteSpace(this.NewAccountPassword);
        }

        public DelegateCommand RestoreAccountCommand { get; }
        private async void RestoreAccount()
        {
            this.RestoreError = null;
            this.Message = null;
            this.IsBusy = true;
            if (!string.IsNullOrWhiteSpace(this.PrivateKey))
            {
                try
                {
                    await this.accountService.RestoreAccountByKey(this.PrivateKey, this.RestoreAccountPassword);
                }
                catch
                {
                    this.RestoreError = "Invalid private key";
                }                
            }
            else
            {
                try
                {
                    await this.accountService.RestoreAccountByMnemonic(this.Mnemonic, this.RestoreAccountPassword);
                }
                catch
                {
                    this.RestoreError = "Invalid mnemonic phrase";
                }                
            }

            if (this.RestoreError == null)
            {
                this.web3Service.Initialize(this.accountService.Account);
                this.Message = $"Account restored successfully. Your address is '{this.accountService.Account.Address}'.";
            }

            this.IsBusy = false;
            this.RestoreAccountPassword = null;
            this.Mnemonic = null;
            this.PrivateKey = null;
        }
        private bool CanRestoreAccount()
        {
            return !string.IsNullOrWhiteSpace(this.RestoreAccountPassword)
                && (!string.IsNullOrWhiteSpace(this.PrivateKey) || !string.IsNullOrWhiteSpace(this.Mnemonic));
        }
    }
}
