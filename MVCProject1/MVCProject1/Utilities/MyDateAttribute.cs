using System;
using System.ComponentModel.DataAnnotations;



namespace MVCProject1.Utilities
{
    /* Class is used to validate expiration date of credit card. Class is called in UserInfo.cs class */
    public class MyDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;
            if (dt >= DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Credit Card is Expired");
        }

    }
}
