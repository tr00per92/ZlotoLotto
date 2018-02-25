using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZlotoLotto.Services;
using ZlotoLotto.Views;

namespace ZlotoLotto.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IWeb3Service web3Service;

        public MainPageViewModel(INavigationService navigationService, IWeb3Service web3Service)
            : base(navigationService)
        {
            this.web3Service = web3Service;
            this.Title = "Zloto Lotto";
            this.Address = this.web3Service.Address;
            this.ScratchCommand = new DelegateCommand(this.Scratch);
            this.OpenAddressCommand = new DelegateCommand(this.OpeAddress);
            this.GoToExchangeCommand = new DelegateCommand(this.GoToExchange);
            this.UpdateTokensCount();
        }

        public ICommand ScratchCommand { get; }
        private async void Scratch()
        {
            var result = await this.web3Service.ScratchToken();
            this.UpdateTokensCount();
        }

        public ICommand OpenAddressCommand { get; }
        private void OpeAddress()
        {
            Device.OpenUri(new Uri("https://ropsten.etherscan.io/address/" + this.Address));
        }

        public ICommand GoToExchangeCommand { get; }
        private async void GoToExchange()
        {
            await this.NavigationService.NavigateAsync(nameof(ExchangeTokensPage));
        }

        private string address;
        public string Address
        {
            get => this.address;
            set => this.SetProperty(ref this.address, value);
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
    }
}
