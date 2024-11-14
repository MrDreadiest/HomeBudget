using HomeBudget.App.Models.TransactionCategories;

namespace HomeBudget.App.Models.WidgetConfigurations
{
    public class FastBalanceWidgetConfiguration
    {
        public static int TopCounterMin = 1;

        public string SelectType { get; set; } = TransactionCategoriesSelectType.Own.ToString();
        public List<string> SelectedCategoriesIds { get; set; } = new();
        public int TopCounter { get; set; } = TopCounterMin;
        public bool IsShowPercentageSwitch { get; set; } = true;
        public bool IsSumOtherAsLastSwitch { get; set; } = true;
    }
}
