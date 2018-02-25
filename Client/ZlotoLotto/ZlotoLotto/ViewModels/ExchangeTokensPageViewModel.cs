using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using ZlotoLotto.Services;

namespace ZlotoLotto.ViewModels
{
    public class ExchangeTokensPageViewModel : ViewModelBase
    {
        private readonly IWeb3Service web3Service;

        public ExchangeTokensPageViewModel(INavigationService navigationService, IWeb3Service web3Service) 
            : base(navigationService)
        {
            this.web3Service = web3Service;
            this.Title = "Exchange tickets";
            this.Refresh();
            this.BuyTokensCommand = new DelegateCommand(this.BuyTokens);
            this.SellTokensCommand = new DelegateCommand(this.SellTokens);
        }

        private void Refresh()
        {
            this.UpdateTokensCount();
            this.UpdateCurrenPrice();
            this.UpdateBalance();
        }

        public ICommand BuyTokensCommand { get; }
        private async void BuyTokens()
        {
            await this.web3Service.BuyTokens(this.BuyTokensCount, this.BuyTotalPrice);
            this.Refresh();
            this.BuyTokensCount = 0;
        }

        public ICommand SellTokensCommand { get; }
        private async void SellTokens()
        {
            await this.web3Service.SellTokens(this.SellTokensCount);
            this.Refresh();
            this.SellTokensCount = 0;
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
            set
            {
                this.SetProperty(ref this.sellTokensCount, value);
                this.RaisePropertyChanged(nameof(this.SellTotalPrice));
            }
        }
        public decimal SellTotalPrice => this.SellTokensCount * this.CurrentPrice;

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
    }
}
