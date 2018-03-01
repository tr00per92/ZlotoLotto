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
    public class AdminPageViewModel : ViewModelBase
    {
        private readonly IWeb3Service web3Service;

        public AdminPageViewModel(INavigationService navigationService, IWeb3Service web3Service)
            : base(navigationService)
        {
            this.web3Service = web3Service;
            this.Title = "Zloto Lotto Admin";        
            this.WithdrawCommand = new DelegateCommand(this.Withdraw, this.CanWithdraw);
            this.DepositCommand = new DelegateCommand(this.Deposit, this.CanDeposit);
        }

        private void Refresh()
        {
            this.UpdateContractBalance();
            this.UpdateMinimumBalance();
            this.UpdateAccountBalance();
        }

        public DelegateCommand WithdrawCommand { get; }
        private async void Withdraw()
        {
            this.IsBusy = true;
            this.HasError = false;
            this.Message = null;
            try
            {
                await this.web3Service.WithdrawBalance(this.WithdrawAmount);
                this.Message = $"Withdraw of {this.WithdrawAmount} ether successfull.";
                this.WithdrawAmount = 0;
            }
            catch
            {
                this.HasError = true;
            }

            this.Refresh();
            this.IsBusy = false;
        }
        private bool CanWithdraw()
        {
            return !this.IsBusy && this.WithdrawAmount > 0 && this.WithdrawAmount <= this.AvailableForWithdraw;
        }

        public DelegateCommand DepositCommand { get; }
        private async void Deposit()
        {
            this.IsBusy = true;
            this.HasError = false;
            this.Message = null;
            try
            {
                await this.web3Service.Deposit(this.DepositAmount);
                this.Message = $"Deposit of {this.DepositAmount} ether successfull.";
                this.DepositAmount = 0;
            }
            catch
            {
                this.HasError = true;
            }

            this.Refresh();
            this.IsBusy = false;
        }
        private bool CanDeposit()
        {
            return !this.IsBusy && this.DepositAmount > 0 && this.DepositAmount <= this.AccountBalance;
        }

        private decimal withdrawAmount;
        public decimal WithdrawAmount
        {
            get => this.withdrawAmount;
            set
            {
                this.SetProperty(ref this.withdrawAmount, value);
                this.WithdrawCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal depositAmount;
        public decimal DepositAmount
        {
            get => this.depositAmount;
            set
            {
                this.SetProperty(ref this.depositAmount, value);
                this.DepositCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal contractBalance;
        public decimal ContractBalance
        {
            get => this.contractBalance;
            set
            {
                this.SetProperty(ref this.contractBalance, value);
                this.RaisePropertyChanged(nameof(this.AvailableForWithdraw));
            }
        }
        private async void UpdateContractBalance()
        {
            this.ContractBalance = await this.web3Service.GetContractBalance();
        }

        private decimal minimumBalance;
        public decimal MinimumBalance
        {
            get => this.minimumBalance;
            set
            {
                this.SetProperty(ref this.minimumBalance, value);
                this.RaisePropertyChanged(nameof(this.AvailableForWithdraw));
            }
        }
        private async void UpdateMinimumBalance()
        {
            this.MinimumBalance = await this.web3Service.GetContractMinimumBalance();
        }

        public decimal AvailableForWithdraw => this.ContractBalance - this.MinimumBalance;

        private decimal accountBalance;
        public decimal AccountBalance
        {
            get => this.accountBalance;
            set
            {
                this.SetProperty(ref this.accountBalance, value);
                this.DepositCommand.RaiseCanExecuteChanged();
            }
        }

        private async void UpdateAccountBalance()
        {
            this.AccountBalance = await this.web3Service.GetBalance();
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            this.Refresh();
        }

        protected override void RaiseCommandsCanExecuteChanged()
        {
            this.WithdrawCommand.RaiseCanExecuteChanged();
            this.DepositCommand.RaiseCanExecuteChanged();
        }
    }
}
