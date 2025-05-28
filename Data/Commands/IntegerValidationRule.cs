using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ComputerTracker.Data.Commands
{
    public class IntegerValidationRule : ValidationRule
    {
        public int Min { get; set; } = 1;
        public int Max { get; set; } = 65535;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string s = (value ?? "").ToString();

            if (string.IsNullOrWhiteSpace(s))
                return new ValidationResult(false, "Значение не может быть пустым");

            if (!int.TryParse(s, out int parsed))
                return new ValidationResult(false, "Введите целое число");

            if (parsed < Min || parsed > Max)
                return new ValidationResult(false, $"Число должно быть от {Min} до {Max}");

            return ValidationResult.ValidResult;
        }
    }
}
