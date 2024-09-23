using HomeBudget.App.Services;
using HomeBudget.App.ViewModels;
using HomeBudget.App.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

namespace HomeBudget.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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

//            // Remove underline from Entry control on Android
//            EntryHandler.Mapper.ModifyMapping(nameof(Entry), (handler, view, action) =>
//            {
//                action?.Invoke(handler, view);

//#if ANDROID
//                            // Removes the underline by setting the background color to transparent
//                            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
//#endif
//            });


            // Register services
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            // SERVICES
            builder.Services.AddSingleton<ClientService>();

            // VIEW MODELS
            builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<RegisterPageViewModel>();

            // PAGE VIEWS
            builder.Services.AddSingleton<DashboardPageAndroidView>();
            builder.Services.AddSingleton<LoginPageAndroidView>();
            builder.Services.AddSingleton<RegisterPageViewModel>();

            return builder.Build();
        }
    }
}
