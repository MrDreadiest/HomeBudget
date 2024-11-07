using HomeBudget.App.Resources.Icons;

namespace HomeBudget.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Resources.Add("DashboardIcon", Icons.Home);
            Resources.Add("TransactionsIcon", Icons.Transactions);
            Resources.Add("ReportsIcon", Icons.Graphs);
            Resources.Add("BudgetIcon", Icons.Budget);
            Resources.Add("SettingsIcon", Icons.Settings);

            InitializeComponent();


        }
    }
}
