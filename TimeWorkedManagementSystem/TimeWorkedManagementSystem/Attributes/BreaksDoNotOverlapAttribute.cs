using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Attributes
{
    public class BreaksDoNotOverlapAttribute : ValidationAttribute, IClientModelValidator
    { 
        private string _errorMessage { get; }

        public BreaksDoNotOverlapAttribute(string errorMessage = "Breaks cannot overlap each other")
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Shift shift = (Shift)context.ObjectInstance;
            List<Break> breaks = shift.Breaks;

            bool valid = true;

            for (int i = 0; i < breaks.Count - 1; i++)
            {
                if (breaks[i].End.CompareTo(breaks[i+1].Start) > 0)
                {
                    valid = false;
                }
            }

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
