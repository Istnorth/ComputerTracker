using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ComputerTracker.Data.Commands
{
    public class IPAddressValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string ip = (value ?? string.Empty).ToString();

            if (string.IsNullOrWhiteSpace(ip))
            {
                return new ValidationResult(false, "IP-адрес не может быть пустым");
            }

            string pattern = @"^(25[0-5]|2[0-4]\d|[01]?\d\d?)(\.(25[0-5]|2[0-4]\d|[01]?\d\d?)){3}$";
            if (!Regex.IsMatch(ip, pattern))
            {
                return new ValidationResult(false, "Неверный формат IP-адреса");
            }

            return ValidationResult.ValidResult;
        }
    }
}
