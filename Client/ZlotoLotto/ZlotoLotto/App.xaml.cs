using System.Linq;
using Prism;
using Prism.Ioc;
using ZlotoLotto.Views;
using Xamarin.Forms.Xaml;
using Prism.Autofac;
using Xamarin.Forms;
using ZlotoLotto.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ZlotoLotto
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            this.InitializeComponent();
            var startPage = Settings.Accounts.Any() ? nameof(UnlockAccountPage) : nameof(CreateAccountPage);
            await this.NavigationService.NavigateAsync(startPage);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<CreateAccountPage>();
            containerRegistry.RegisterForNavigation<UnlockAccountPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<ExchangeTokensPage>();

            containerRegistry.RegisterSingleton<IAccountService, AccountService>();
            containerRegistry.RegisterSingleton<IWeb3Service, Web3Service>();
        }
    }
}
