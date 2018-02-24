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

        public NewAccountModel CreateNew(string password)
        {
            var newWallet = new Wallet(Wordlist.English, WordCount.Twelve);
            var privateKey = newWallet.GetAccount(0).PrivateKey;
            this.RestoreAccountByKey(privateKey, password);

            return new NewAccountModel { PrivateKey = privateKey, Mnemonic = string.Join(" ", newWallet.Words) };
        }

        public void UnlockAccount(string password)
        {
            this.Account = Account.LoadFromKeyStore(Settings.KeyStore, password);
        }

        public void RestoreAccountByMnemonic(string mnemonic, string newPassword)
        {
            var privateKey = new Wallet(mnemonic, null).GetAccount(0).PrivateKey;
            this.RestoreAccountByKey(privateKey, newPassword);
        }

        public void RestoreAccountByKey(string privateKey, string newPassword)
        {
            this.Account = new Account(privateKey);
            var json = new KeyStoreService().EncryptAndGenerateDefaultKeyStoreAsJson(newPassword, privateKey.HexToByteArray(), this.Account.Address);
            Settings.KeyStore = json;
        }
    }
}
