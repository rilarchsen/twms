using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TimeWorkedManagementSystem.Interfaces;

namespace TimeWorkedManagementSystem.Attributes
{
    public class TimeSpanValidAttribute : ValidationAttribute, IClientModelValidator
    {
        private string _errorMessage { get; }

        public TimeSpanValidAttribute(string errorMessage = "Start time must be before end time")
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            ITimeSpanned b = (ITimeSpanned)value;

            bool valid = b.Start.CompareTo(b.End) <= 0;

            return valid ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
        }

        public string GetErrorMessage() => _errorMessage;

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-requiredIf", GetErrorMessage());
        }

        private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}
