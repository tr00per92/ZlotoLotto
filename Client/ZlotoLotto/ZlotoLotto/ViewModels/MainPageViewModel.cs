using Prism.Navigation;

namespace ZlotoLotto.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.Title = "Zloto Lotto";
        }
    }
}
