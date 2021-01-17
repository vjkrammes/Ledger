using System;
using System.ComponentModel.DataAnnotations;

namespace LedgerLib.Infrastructure
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class DescriptionAttribute : Attribute
    {
        private readonly string _description;
        public DescriptionAttribute(string description) => _description = description;
        public string Description => _description;
    }

    public sealed class NonNegativeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valid = false;
            switch (value)
            {
                case int ival:
                    valid = ival >= 0;
                    break;
                case long lval:
                    valid = lval >= 0;
                    break;
                case float fval:
                    valid = fval >= 0;
                    break;
                case double dval:
                    valid = dval >= 0;
                    break;
                case decimal mval:
                    valid = mval >= 0M;
                    break;
            }
            if (!valid)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    var prop = validationContext.DisplayName;
                    return new ValidationResult($"{prop} must be greater than or equal to zero");
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return null;
        }
    }

    public sealed class PositiveAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valid = false;
            switch (value)
            {
                case int ival:
                    valid = ival > 0;
                    break;
                case long lval:
                    valid = lval > 0;
                    break;
                case float fval:
                    valid = fval > 0.0;
                    break;
                case double dval:
                    valid = dval > 0.0;
                    break;
                case decimal mval:
                    valid = mval > 0M;
                    break;
            }
            if (!valid)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    var prop = validationContext.DisplayName;
                    return new ValidationResult($"{prop} must be greater than zero");
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return null;
        }
    }
}
