using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls
{
    public partial class CalculatorPopupContentViewModel : BaseContentViewModel
    {
        public event EventHandler<decimal>? CalculationCompleted;

        [RelayCommand]
        public void AppendToFormula(string value)
        {
            if (value == ",")
            {
                if (CanAddDecimalSeparator())
                {
                    _calculatorModel.CurrentFormula += ".";
                }
            }
            else
            {
                _calculatorModel.CurrentFormula += value;
            }

            Calculate();
            UpdateDisplays();
        }

        [RelayCommand]
        public void ClearFormula()
        {
            _calculatorModel.CurrentFormula = string.Empty;
            UpdateDisplays();
        }

        [RelayCommand]
        public void DeleteLastCharacter()
        {
            if (_calculatorModel.CurrentFormula.Length > 0)
            {
                _calculatorModel.CurrentFormula = _calculatorModel.CurrentFormula[..^1];
                Calculate();
                UpdateDisplays();
            }
        }

        [RelayCommand]
        public void Calculate()
        {
            try
            {
                var result = _calculatorModel.EvaluateFormula();
                ResultDisplay = result.ToString();
                CalculationCompleted?.Invoke(this, result);
            }
            catch
            {
                ResultDisplay = "Błąd!";
            }
        }

        [RelayCommand]
        public async Task Accept()
        {
            try
            {
                var result = _calculatorModel.EvaluateFormula();
                CalculationCompleted?.Invoke(this, result);
                FormulaDisplay = _calculatorModel.CurrentFormula;
                ResultDisplay = "0";
                _calculatorModel.CurrentFormula = string.Empty;
                await ToggleVisibility();
            }
            catch
            {
                ResultDisplay = "Błąd!";
                await ToggleVisibility();
            }
        }

        [ObservableProperty]
        private string _formulaDisplay;

        [ObservableProperty]
        private string _resultDisplay;

        private readonly CalculatorModel _calculatorModel;

        public CalculatorPopupContentViewModel()
        {
            Title = "Kalkulator";

            _calculatorModel = new CalculatorModel();
            FormulaDisplay = _calculatorModel.CurrentFormula;
            ResultDisplay = "0";
        }

        //TODO: Coś nie działa do sprawdzenia
        //[RelayCommand]
        //public void CalculatePercentage()
        //{
        //    var lastNumber = GetLastNumber();

        //    if (decimal.TryParse(lastNumber, out decimal number))
        //    {
        //        var percentageValue = number / 100;
        //        _calculatorModel.CurrentFormula = ReplaceLastNumberWith(percentageValue.ToString());
        //        UpdateDisplays();
        //        Calculate();
        //    }
        //}

        //private string ReplaceLastNumberWith(string replacement)
        //{
        //    var formulaParts = _calculatorModel.CurrentFormula.Split(new char[] { '+', '-', '*', '/' });
        //    if (formulaParts.Length > 0)
        //    {
        //        formulaParts[^1] = replacement;
        //        return string.Join(GetLastOperator(), formulaParts);
        //    }
        //    return replacement;
        //}

        //private string GetLastOperator()
        //{
        //    foreach (char c in _calculatorModel.CurrentFormula.Reverse())
        //    {
        //        if (c == '+' || c == '-' || c == '*' || c == '/')
        //            return c.ToString();
        //    }
        //    return string.Empty;
        //}

        private void UpdateDisplays()
        {
            FormulaDisplay = _calculatorModel.CurrentFormula;
            ResultDisplay = string.IsNullOrEmpty(_calculatorModel.CurrentFormula) ? "0" : ResultDisplay;
        }

        private bool CanAddDecimalSeparator()
        {
            var lastNumber = GetLastNumber();
            return !lastNumber.Contains(".");
        }

        private string GetLastNumber()
        {
            var formulaParts = _calculatorModel.CurrentFormula.Split(new char[] { '+', '-', '*', '/' });
            return formulaParts.Length > 0 ? formulaParts[^1] : string.Empty;
        }
    }

    public class CalculatorModel
    {
        public string CurrentFormula { get; set; } = string.Empty;

        public decimal EvaluateFormula()
        {
            var dataTable = new System.Data.DataTable();
            return Convert.ToDecimal(dataTable.Compute(CurrentFormula, string.Empty));
        }
    }
}
