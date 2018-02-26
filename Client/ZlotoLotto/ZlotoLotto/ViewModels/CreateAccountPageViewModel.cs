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
            this.CreateAccountCommand = new DelegateCommand(this.CreateAccount);
            this.RestoreAccountCommand = new DelegateCommand(this.RestoreAccount);
        }

        private string newAccountPassword;
        public string NewAccountPassword
        {
            get => this.newAccountPassword;
            set => this.SetProperty(ref this.newAccountPassword, value);
        }

        private string restoreAccountPassword;
        public string RestoreAccountPassword
        {
            get => this.restoreAccountPassword;
            set => this.SetProperty(ref this.restoreAccountPassword, value);
        }

        private string privateKey;
        public string PrivateKey
        {
            get => this.privateKey;
            set => this.SetProperty(ref this.privateKey, value);
        }

        private string mnemonic;
        public string Mnemonic
        {
            get => this.mnemonic;
            set => this.SetProperty(ref this.mnemonic, value);
        }
        
        public ICommand CreateAccountCommand { get; }
        private async void CreateAccount()
        {
            var newAccountData = this.accountService.CreateNew(this.NewAccountPassword);
            this.web3Service.Initialize(this.accountService.Account);
            await this.NavigationService.NavigateToMainAsync();
        }

        public ICommand RestoreAccountCommand { get; }
        private async void RestoreAccount()
        {
            if (!string.IsNullOrWhiteSpace(this.PrivateKey))
            {
                try
                {
                    this.accountService.RestoreAccountByKey(this.PrivateKey, this.RestoreAccountPassword);
                    this.Message = null;
                }
                catch
                {
                    this.Message = "Invalid private key";
                }                
            }
            else if (!string.IsNullOrWhiteSpace(this.Mnemonic))
            {
                try
                {
                    this.accountService.RestoreAccountByMnemonic(this.Mnemonic, this.RestoreAccountPassword);
                    this.Message = null;
                }
                catch
                {
                    this.Message = "Invalid mnemonic phrase";
                }                
            }
            else
            {
                this.Message = "Please fill the private key or the mnemonic";
            }

            if (this.Message == null)
            {
                this.web3Service.Initialize(this.accountService.Account);
                await this.NavigationService.NavigateToMainAsync();
            }            
        }
    }
}
