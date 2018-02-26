using System.Threading.Tasks;
using NBitcoin;
using Nethereum.HdWallet;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.KeyStore;
using Nethereum.Web3.Accounts;
using ZlotoLotto.Models;

namespace ZlotoLotto.Services
{
    public class AccountService : IAccountService
    {
        public Account Account { get; private set; }

        public Task<NewAccountModel> CreateNew(string password)
        {
            return Task.Run(async () =>
            {
                var newWallet = new Wallet(Wordlist.English, WordCount.Twelve);
                var privateKey = newWallet.GetAccount(0).PrivateKey;
                await this.RestoreAccountByKey(privateKey, password);
                return new NewAccountModel { PrivateKey = privateKey, Mnemonic = string.Join(" ", newWallet.Words) };
            });
        }

        public Task UnlockAccount(string password)
        {
            return Task.Run(() => this.Account = Account.LoadFromKeyStore(Settings.KeyStore, password));
        }

        public Task RestoreAccountByMnemonic(string mnemonic, string newPassword)
        {
            return Task.Run(async () =>
            {
                var privateKey = new Wallet(mnemonic, null).GetAccount(0).PrivateKey;
                await this.RestoreAccountByKey(privateKey, newPassword);
            });
        }

        public Task RestoreAccountByKey(string privateKey, string newPassword)
        {
            return Task.Run(() =>
            {
                this.Account = new Account(privateKey);
                var json = new KeyStoreService().EncryptAndGenerateDefaultKeyStoreAsJson(newPassword, privateKey.HexToByteArray(), this.Account.Address);
                Settings.KeyStore = json;
            });
        }
    }
}
