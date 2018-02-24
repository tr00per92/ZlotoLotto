using System;
using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using ZlotoLotto.Models;

namespace ZlotoLotto.Services
{
    public class Web3Service : IWeb3Service
    {
        private string address;
        private Web3 web3;
        private Contract contract;
        private bool initialized;

        public void Initialize(IAccount account)
        {
            this.address = account.Address;
            this.web3 = new Web3(account, Settings.InfuraUrl);
            this.contract = this.web3.Eth.GetContract(Settings.ContractAbi, Settings.ContractAddress);
            this.initialized = true;
        }

        public async Task<decimal> GetBalance()
        {
            this.EnsureInitialized();
            var balance = await this.web3.Eth.GetBalance.SendRequestAsync(this.address);
            return Web3.Convert.FromWei(balance.Value);
        }

        public async Task<decimal> GetCurrentPrice()
        {
            this.EnsureInitialized();
            var price = await this.contract.GetFunction("price").CallAsync<ulong>();
            return Web3.Convert.FromWei(price);
        }

        public async Task<int> GetTokensCount()
        {
            this.EnsureInitialized();
            var count = await this.contract.GetFunction("getTokensCount").CallAsync<int>(this.address, null, null);
            return count;
        }

        public async Task BuyTokens(int count, decimal totalCost)
        {
            this.EnsureInitialized();
            var buyFunction = this.contract.GetFunction("buyTokens");
            var value = new HexBigInteger(Web3.Convert.ToWei(totalCost));
            var gas = await buyFunction.EstimateGasAsync(this.address, null, value, count);
            await buyFunction.SendTransactionAsync(this.GetTransactionInput(gas, value), count);
        }

        public async Task SellTokens(int count)
        {
            this.EnsureInitialized();
            var sellFunction = this.contract.GetFunction("sellTokens");
            var gas = await sellFunction.EstimateGasAsync(this.address, null, null, count);
            await sellFunction.SendTransactionAsync(this.GetTransactionInput(gas), count);
        }

        public async Task<ScratchResult> ScratchToken()
        {
            this.EnsureInitialized();
            var scratchFunction = this.contract.GetFunction("scratchToken");
            var count = await this.GetTokensCount();
            var gas = await scratchFunction.EstimateGasAsync(this.address, null, null);
            await scratchFunction.SendTransactionAndWaitForReceiptAsync(this.GetTransactionInput(new HexBigInteger(gas.Value * 2)));
            return (ScratchResult)(await this.GetTokensCount() - count + 1);
        }

        private TransactionInput GetTransactionInput(HexBigInteger gas, HexBigInteger value = null)
        {
            return new TransactionInput { Gas = gas, From = this.address, GasPrice = new HexBigInteger(100000000000), Value = value };
        }

        private void EnsureInitialized()
        {
            if (!this.initialized)
            {
                throw new InvalidOperationException("Web3Service is not initialized. Please call Initialize() method first.");
            }
        }
    }
}
