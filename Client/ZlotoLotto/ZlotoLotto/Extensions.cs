using System;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;
using ZlotoLotto.Views;

namespace ZlotoLotto
{
    public static class Extensions
    {
        public static Task NavigateToMainAsync(this INavigationService navigationService, string currentAddress)
        {
            var mainPage = string.Equals(currentAddress, Settings.OwnerAddress, StringComparison.OrdinalIgnoreCase) ? nameof(AdminPage) : nameof(MainPage);
            return navigationService.NavigateAsync($"app:///{nameof(NavigationPage)}/{mainPage}");
        }

        public static Task NavigateToStartAsync(this INavigationService navigationService)
        {
            var startPage = Settings.Accounts.Any() ? nameof(UnlockAccountPage) : nameof(CreateAccountPage);
            return navigationService.NavigateAsync(startPage);
        }
    }
}
