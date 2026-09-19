using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Helpers
{

    // using Manual Validation 

    public class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);

            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool IsValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!IsValid)
            {
                string Errors = string.Empty;

                foreach (var result in validationResults)
                {
                    Errors += $"{result.ErrorMessage}\n";
                }

                throw new ValidationException(Errors);
            }
        }
    }
}
