using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZlotoLotto.Models;
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
            this.ScratchCommand = new DelegateCommand(this.Scratch, this.CanScratch);
            this.OpenAddressCommand = new DelegateCommand(this.OpeAddress);
            this.GoToExchangeCommand = new DelegateCommand(this.GoToExchange);            
        }

        public DelegateCommand ScratchCommand { get; }
        private async void Scratch()
        {
            this.IsBusy = true;
            this.Message = null;
            try
            {
                this.TokensCount--;
                var result = await this.web3Service.ScratchToken();
                switch (result)
                {
                    case ScratchResult.Lose:
                        this.Message = "You lose this time, but every second ticket wins. Try again.";
                        break;
                    case ScratchResult.WinOne:
                        this.Message = "Congratulations. You won a ticket.";
                        break;
                    case ScratchResult.WinTwo:
                    case ScratchResult.WinFour:
                        this.Message = $"Congratulations. You won {(int)result} tickets.";
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            catch
            {
                this.HasError = true;
            }
            
            this.UpdateTokensCount();
            this.IsBusy = false;
        }
        private bool CanScratch()
        {
            return !this.IsBusy;
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

        protected override void RaiseCommandsCanExecuteChanged()
        {
            this.ScratchCommand.RaiseCanExecuteChanged();
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            this.UpdateTokensCount();
        }
    }
}
