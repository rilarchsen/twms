using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Attributes
{
    public class BreaksWithinBoundsAttribute : ValidationAttribute, IClientModelValidator
    { 
        private string _errorMessage { get; }

        public BreaksWithinBoundsAttribute(string errorMessage = "Break times must be within shift times")
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Shift shift = (Shift)context.ObjectInstance;
            List<Break> breaks = shift.Breaks;

            bool invalid = shift.Breaks.Any(b => b.Start.CompareTo(shift.Start) < 0) || shift.Breaks.Any(b => b.End.CompareTo(shift.End) > 0);

            return invalid ? new ValidationResult(GetErrorMessage()) : ValidationResult.Success;
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
