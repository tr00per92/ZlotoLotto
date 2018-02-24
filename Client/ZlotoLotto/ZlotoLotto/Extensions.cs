using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;
using ZlotoLotto.Views;

namespace ZlotoLotto
{
    public static class Extensions
    {
        public static Task NavigateToMainAsync(this INavigationService navigationService)
        {
            return navigationService.NavigateAsync($"app:///{nameof(NavigationPage)}/{nameof(MainPage)}");
        }
    }
}
