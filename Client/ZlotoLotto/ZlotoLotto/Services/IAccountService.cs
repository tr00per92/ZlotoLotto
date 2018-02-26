using System.Threading.Tasks;
using Nethereum.Web3.Accounts;
using ZlotoLotto.Models;

namespace ZlotoLotto.Services
{
    public interface IAccountService
    {
        Account Account { get; }

        Task<NewAccountModel> CreateNew(string password);

        Task UnlockAccount(string password);

        Task RestoreAccountByMnemonic(string mnemonic, string newPassword);

        Task RestoreAccountByKey(string privateKey, string newPassword);
    }
}
