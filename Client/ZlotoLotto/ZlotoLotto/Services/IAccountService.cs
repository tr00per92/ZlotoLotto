using Nethereum.Web3.Accounts;
using ZlotoLotto.Models;

namespace ZlotoLotto.Services
{
    public interface IAccountService
    {
        Account Account { get; }

        NewAccountModel CreateNew(string password);

        void UnlockAccount(string password);

        void RestoreAccountByMnemonic(string mnemonic, string newPassword);

        void RestoreAccountByKey(string privateKey, string newPassword);
    }
}
