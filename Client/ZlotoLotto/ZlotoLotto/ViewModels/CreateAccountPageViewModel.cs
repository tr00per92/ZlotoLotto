using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using ZlotoLotto.Services;
using ZlotoLotto.Views;

namespace ZlotoLotto.ViewModels
{
    public class CreateAccountPageViewModel : ViewModelBase
    {
        private readonly IAccountService accountService;

        public CreateAccountPageViewModel(INavigationService navigationService, IAccountService accountService) 
            : base (navigationService)
        {
            this.accountService = accountService;
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

        public ICommand CreateAccountCommand { get; set; }
        private async void CreateAccount()
        {
            var newAccount = this.accountService.CreateNew(this.NewAccountPassword);
            await this.NavigationService.NavigateToMainAsync();
        }

        public ICommand RestoreAccountCommand { get; set; }
        private void RestoreAccount()
        {
            if (!string.IsNullOrEmpty(this.PrivateKey))
            {
                this.accountService.RestoreAccountByKey(this.PrivateKey, this.RestoreAccountPassword);
            }
            else
            {
                this.accountService.RestoreAccountByMnemonic(this.Mnemonic, this.RestoreAccountPassword);
            }
        }
    }
}
