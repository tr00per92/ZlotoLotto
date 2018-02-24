﻿using Prism;
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
            var startPage = Settings.KeyStore == null ? nameof(CreateAccountPage) : nameof(UnlockAccountPage);
            await this.NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{startPage}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<CreateAccountPage>();
            containerRegistry.RegisterForNavigation<UnlockAccountPage>();
            containerRegistry.RegisterForNavigation<MainPage>();

            containerRegistry.RegisterSingleton<IAccountService, AccountService>();
        }
    }
}
