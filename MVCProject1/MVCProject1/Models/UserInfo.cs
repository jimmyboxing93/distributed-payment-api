using System.ComponentModel.DataAnnotations;
using System;
using MVCProject1.Utilities;
namespace MVCProject1.Models
{
    public class UserInfo
    {
        /* Class is used for holding input paramters and user data. 
         * Also performs error handling by using .net core's attribute. 
         * These attributes have built in methods that could validate data. 
         * Most validation is done using regular expression.
         */
        [Key]
        public Guid UserID { get; set; }

        public string password { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your name")]
        public string firstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your name")]
        public string lastName { get; set; }

        [Display(Name = "Credit Card Number")]
        [Required(ErrorMessage = "Please enter a valid credit card number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only number allowed")]
        [Range(100000000000, 9999999999999999999, ErrorMessage = "must be between 12 and 19 digits")]
        public string creditCardNumber { get; set; }
        [Required(ErrorMessage = "Please enter valid security code")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Please enter only 3 digits")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only number allowed")]
        public string ccv { get; set; }

        [Display(Name = "Expiration Date")]
        [Required(ErrorMessage = "Please enter expiration date")]
        [DataType(DataType.Date)]
        // Calls MyDateAttribute class to validate Expiration Date
        [MyDate] 
        //[RegularExpression(@"([0-9][0-9])\/([0-9][0-9])", ErrorMessage = "Date must be in MM/YY format")]
        public DateTime expirationDate { get; set; }
        
        public double amount { get; set; }

        public string Name { get; set; }






    }
}
