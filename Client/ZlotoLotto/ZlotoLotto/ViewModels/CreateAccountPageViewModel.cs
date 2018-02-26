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
            this.CreateAccountCommand = new DelegateCommand(this.CreateAccount, this.CanCreateAccount);
            this.RestoreAccountCommand = new DelegateCommand(this.RestoreAccount, this.CanRestoreAccount);
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

        public DelegateCommand CreateAccountCommand { get; }
        private async void CreateAccount()
        {
            this.IsBusy = true;
            var newAccountData = await this.accountService.CreateNew(this.NewAccountPassword);
            this.web3Service.Initialize(this.accountService.Account);
            await this.NavigationService.NavigateToMainAsync();
            this.IsBusy = false;
        }
        private bool CanCreateAccount()
        {
            return !string.IsNullOrWhiteSpace(this.NewAccountPassword);
        }

        public DelegateCommand RestoreAccountCommand { get; }
        private async void RestoreAccount()
        {
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
                    this.Message = "Invalid private key";
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
                    this.Message = "Invalid mnemonic phrase";
                }                
            }

            if (this.Message == null)
            {
                this.web3Service.Initialize(this.accountService.Account);
                await this.NavigationService.NavigateToMainAsync();
            }

            this.RestoreAccountPassword = null;
            this.IsBusy = false;
        }
        private bool CanRestoreAccount()
        {
            return !string.IsNullOrWhiteSpace(this.RestoreAccountPassword)
                && (!string.IsNullOrWhiteSpace(this.PrivateKey) || !string.IsNullOrWhiteSpace(this.Mnemonic));
        }
    }
}
