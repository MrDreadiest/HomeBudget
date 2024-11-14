using CommunityToolkit.Maui;
using HomeBudget.App.Services;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels;
using HomeBudget.App.ViewModels.ContentViewModels.FullViews;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;
using HomeBudget.App.ViewModels.Widgets;
using HomeBudget.App.Views;
using HomeBudget.App.Views.ContentViews.FullViews;
using HomeBudget.App.Views.Widgets;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace HomeBudget.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseSkiaSharp()
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("RedHatDisplay-Regular.ttf", "RedHatDisplayRegular");
                    fonts.AddFont("RedHatDisplay-Medium.ttf", "RedHatDisplayMedium");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Register services
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });

            Microsoft.Maui.Handlers.TimePickerHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });

            // SERVICES

            builder.Services.AddTransient<IAppSettingsService, AppSettingsService>();
            builder.Services.AddTransient<IConfigurationService, ConfigurationService>();
            builder.Services.AddTransient<IApiClient, ApiClient>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IBudgetService, BudgetService>();
            builder.Services.AddSingleton<IAddressService, AddressService>();
            builder.Services.AddSingleton<ITransactionCategoryService, TransactionCategoryService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();
            builder.Services.AddSingleton<IExpenseService, ExpenseService>();
            builder.Services.AddTransient<IRegisterService, RegisterService>();

            // VIEW MODELS

            builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<TransactionsPageViewModel>();
            builder.Services.AddSingleton<ReportsPageViewModel>();
            builder.Services.AddSingleton<BudgetsPageViewModel>();
            builder.Services.AddSingleton<SettingsPageViewModel>();

            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<PasswordReminderViewModel>();
            builder.Services.AddSingleton<UserAccountSetupPageViewModel>();

            builder.Services.AddTransient<AddTransactionPageViewModel>();
            builder.Services.AddTransient<ManageTransactionCategoriesPageViewModel>();

            builder.Services.AddTransient<ManageFastBalancePageViewModel>();
            builder.Services.AddTransient<ManageCurrentBudgetPageViewModel>();
            builder.Services.AddTransient<ManageShortcutsPageViewModel>();
            builder.Services.AddTransient<ManageRegularTransactionsPageViewModel>();
            builder.Services.AddTransient<AddTransactionBySplitPageViewModel>();

            builder.Services.AddTransient<CollapseManager>();
            builder.Services.AddTransient<FadeManager>();
            // PAGE VIEWS

            builder.Services.AddSingleton<DashboardPageAndroidView>();
            builder.Services.AddSingleton<TransactionsPageAndroidView>();
            builder.Services.AddSingleton<ReportsPageAndroidView>();
            builder.Services.AddSingleton<BudgetsPageAndroidView>();
            builder.Services.AddSingleton<SettingsPageAndroidView>();

            builder.Services.AddSingleton<MainPageAndroidView>();
            builder.Services.AddSingleton<PasswordReminderPageAndroidView>();
            builder.Services.AddSingleton<UserAccountSetupPageAndroidView>();

            builder.Services.AddTransient<AddTransactionAndroidPageView>();
            builder.Services.AddTransient<ManageCategoriesAndroidPageView>();

            builder.Services.AddTransient<ManageFastBalanceAndroidPageView>();
            builder.Services.AddTransient<ManageCurrentBudgetAndroidPageView>();
            builder.Services.AddTransient<ManageShortcutsAndroidPageView>();
            builder.Services.AddTransient<ManageRegularTransactionsAndroidPageView>();
            builder.Services.AddTransient<ManageTransactionsSplitAndroidPageView>();
            return builder.Build();
        }
    }
}
