using Plugin.Settings;

namespace ZlotoLotto
{
    public static class Settings
    {
        public const string InfuraUrl = "https://ropsten.infura.io/puD0D4M7LdqMRpSsnqM4";
        public const string ContractAddress = "0xaa9f7d3f6eccd8535a97179a16fefaeec70f29eb";
        public const string ContractAbi = @"[{'constant':true,'inputs':[],'name':'getMinimumBalance','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'currentSupply','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'getBalance','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'price','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'getTokensCount','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'amount','type':'uint256'}],'name':'buyTokens','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'amount','type':'uint256'}],'name':'sellTokens','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'buyTokens','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'value','type':'uint256'}],'name':'withdraw','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'deposit','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'inputs':[{'name':'initialSupply','type':'uint256'},{'name':'initialPriceInFinney','type':'uint256'}],'payable':true,'stateMutability':'payable','type':'constructor'},{'constant':false,'inputs':[],'name':'scratchToken','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'payable':true,'stateMutability':'payable','type':'fallback'}]";
        
        private const string KeyStoreKey = "KeyStore";
        public static string KeyStore
        {
            get => CrossSettings.Current.GetValueOrDefault(KeyStoreKey, null);
            set => CrossSettings.Current.AddOrUpdateValue(KeyStoreKey, value);
        }
    }
}
