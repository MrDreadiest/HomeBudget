using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.App.ViewModels
{
    public partial class DashboardPageViewModel : BaseViewModel
    {
        public async override Task OnAppearingAsync()
        {
            await Task.CompletedTask;
        }

        public async override Task OnDisappearingAsync()
        {
            await Task.CompletedTask;
        }
    }
}