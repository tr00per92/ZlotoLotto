using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using ZlotoLotto.Services;

namespace ZlotoLotto.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IWeb3Service web3Service;

        public MainPageViewModel(INavigationService navigationService, IWeb3Service web3Service, IAccountService accountService)
            : base(navigationService)
        {
            this.web3Service = web3Service;
            this.Title = "Welcome to Zloto Lotto";
            this.Address = this.web3Service.Address;
            this.RefreshCommand = new DelegateCommand(this.Refresh);
            this.ScratchCommand = new DelegateCommand(this.Scratch);
            this.BuyTokensCommand = new DelegateCommand(this.BuyTokens);
            this.SellTokensCommand = new DelegateCommand(this.SellTokens);
            this.Refresh();
        }

        public ICommand RefreshCommand { get; }
        private void Refresh()
        {
            this.UpdateBalance();
            this.UpdateTokensCount();
            this.UpdateCurrenPrice();
        }

        public ICommand ScratchCommand { get; }
        public void Scratch()
        {
        }

        public ICommand BuyTokensCommand { get; }
        public void BuyTokens()
        {
        }

        public ICommand SellTokensCommand { get; }
        public void SellTokens()
        {
        }

        private string address;
        public string Address
        {
            get => this.address;
            set => this.SetProperty(ref this.address, value);
        }

        private decimal balance;
        public decimal Balance
        {
            get => this.balance;
            set => this.SetProperty(ref this.balance, value);
        }
        private async void UpdateBalance()
        {
            this.Balance = await this.web3Service.GetBalance();
        }

        private int tokensCount;
        public int TokensCount
        {
            get => this.tokensCount;
            set => this.SetProperty(ref this.tokensCount, value);
        }
        private async void UpdateTokensCount()
        {
            this.TokensCount = await this.web3Service.GetTokensCount();
        }

        private decimal currentPrice;
        public decimal CurrentPrice
        {
            get => this.currentPrice;
            set => this.SetProperty(ref this.currentPrice, value);
        }
        private async void UpdateCurrenPrice()
        {
            this.CurrentPrice = await this.web3Service.GetCurrentPrice();
        }

        private int buyTokensCount;
        public int BuyTokensCount
        {
            get => this.buyTokensCount;
            set
            {
                this.SetProperty(ref this.buyTokensCount, value);
                this.RaisePropertyChanged(nameof(this.BuyTotalPrice));
            }
        }
        public decimal BuyTotalPrice => this.BuyTokensCount * this.CurrentPrice;

        private int sellTokensCount;
        public int SellTokensCount
        {
            get => this.sellTokensCount;
            set => this.SetProperty(ref this.sellTokensCount, value);
        }
    }
}
