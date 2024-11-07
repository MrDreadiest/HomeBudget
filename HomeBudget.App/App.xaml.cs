namespace HomeBudget.App
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

            InitializeComponent();
            Services = services;
            MainPage = new AppShell();
        }
    }
}
