using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InfrastructureDemo.ValidationRules
{
    public class NoteCategoryNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || (value is string name && string.IsNullOrEmpty(name))) return new ValidationResult(false, "can not be empty!");

            return new ValidationResult(true, null);
        }
    }
}
