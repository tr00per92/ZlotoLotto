using System.Threading.Tasks;
using Nethereum.RPC.Accounts;
using ZlotoLotto.Models;

namespace ZlotoLotto.Services
{
    public interface IWeb3Service
    {
        void Initialize(IAccount account);

        Task<decimal> GetBalance();

        Task<decimal> GetCurrentPrice();

        Task<int> GetTokensCount();

        Task BuyTokens(int count, decimal totalCost);

        Task SellTokens(int count);

        Task<ScratchResult> ScratchToken();
    }
}
