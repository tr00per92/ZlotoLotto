using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ZlotoLotto
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;
        
        private const string KeyStoreKey = "KeyStore";
        public static string KeyStore
        {
            get => AppSettings.GetValueOrDefault(KeyStoreKey, null);
            set => AppSettings.AddOrUpdateValue(KeyStoreKey, value);
        }
    }
}
