namespace HomeBudget.App.Services.Interfaces
{
    public interface IConfigurationService
    {
        void SaveConfiguration<T>(T configuration);
        T LoadConfiguration<T>() where T : class, new();
    }
}
