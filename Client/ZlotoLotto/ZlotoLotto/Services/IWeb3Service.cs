using System.Threading.Tasks;
using Nethereum.RPC.Accounts;
using ZlotoLotto.Models;

namespace ZlotoLotto.Services
{
    public interface IWeb3Service
    {
        string Address { get; }

        void Initialize(IAccount account);

        Task<decimal> GetBalance();

        Task<decimal> GetCurrentPrice();

        Task<int> GetTokensCount();

        Task BuyTokens(int count, decimal totalCost);

        Task SellTokens(int count);

        Task<ScratchResult> ScratchToken();

        Task<decimal> GetContractBalance();

        Task<decimal> GetContractMinimumBalance();

        Task WithdrawBalance(decimal amount);

        Task Deposit(decimal amount);
    }
}
