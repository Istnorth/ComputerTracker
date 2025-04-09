using System;
using System.Globalization;
using System.Windows.Controls;

namespace ComputerTracker.Data.Commands
{
    public class PercentageValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? string.Empty).ToString();

            if (double.TryParse(input, out double number))
            {
                if (number < 0 || number > 100)
                {
                    return new ValidationResult(false, "Значение должно быть в диапазоне [0..100].");
                }
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Введите число.");
        }
    }
}
