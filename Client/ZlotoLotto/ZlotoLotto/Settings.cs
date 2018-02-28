using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Settings;

namespace ZlotoLotto
{
    public static class Settings
    {
        public const string InfuraUrl = "https://ropsten.infura.io/puD0D4M7LdqMRpSsnqM4";
        public const string ContractAddress = "0xaa9f7d3f6eccd8535a97179a16fefaeec70f29eb";
        public const string ContractAbi = @"[{'constant':true,'inputs':[],'name':'getMinimumBalance','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'currentSupply','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'getBalance','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'price','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'getTokensCount','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'amount','type':'uint256'}],'name':'buyTokens','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'amount','type':'uint256'}],'name':'sellTokens','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'buyTokens','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'value','type':'uint256'}],'name':'withdraw','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'deposit','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'inputs':[{'name':'initialSupply','type':'uint256'},{'name':'initialPriceInFinney','type':'uint256'}],'payable':true,'stateMutability':'payable','type':'constructor'},{'constant':false,'inputs':[],'name':'scratchToken','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'payable':true,'stateMutability':'payable','type':'fallback'}]";
        public const string OwnerAddress = "0x9a3f9f9218af03dc73fad57b768ad6cfff2290c1";

        private const string AccountStoreKey = "AccountStore";
        private static string AccountStore
        {
            get => CrossSettings.Current.GetValueOrDefault(AccountStoreKey, null);
            set => CrossSettings.Current.AddOrUpdateValue(AccountStoreKey, value);
        }

        private const char AccountsSplitter = '~';
        private const char AddressKeyStoreSplitter = '^';
        private static IDictionary<string, string> accounts;
        public static IDictionary<string, string> Accounts
        {
            get
            {
                if (accounts == null)
                {
                    if (AccountStore == null)
                    {
                        accounts = new Dictionary<string, string>();
                    }
                    else
                    {
                        var accountStore = AccountStore.Split(AccountsSplitter);
                        accounts = accountStore.Select(a => a.Split(AddressKeyStoreSplitter)).ToDictionary(a => a[0], a => a[1]);
                    }
                }
                
                return accounts;
            }
        }

        public static void AddAccount(string address, string keyStore)
        {
            Accounts[address] = keyStore;
            
            var accountStore = new StringBuilder();
            foreach (var account in Accounts)
            {
                accountStore.Append($"{account.Key}{AddressKeyStoreSplitter}{account.Value}{AccountsSplitter}");
            }

            AccountStore = accountStore.ToString().TrimEnd(AccountsSplitter);
        }
    }
}
