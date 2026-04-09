using System.ComponentModel.DataAnnotations;

namespace SharedData.Models
{
    public class UserInfo
    {
        /* Class is used for holding input paramters and user data. 
         * Also performs error handling by using .net core's attribute. 
         * These attributes have built in methods that could validate data. 
         * Most validation is done using regular expression.
         */

        // Autogenerates User ID
        [Key]
        public Guid UserID { get; set; }

        //Passsword should never be stored as plain text
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Display(Name = "Credit Card Number")]
        [Required(ErrorMessage = "Please enter a valid credit card number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only number allowed")]
        [Range(100000000000, 9999999999999999999, ErrorMessage = "must be between 12 and 19 digits")]
        public string creditCardNumber { get; set; }

		public string LastFourDigits { get; set; }


		[Required(ErrorMessage = "Please enter valid security code")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Please enter only 3 digits")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only number allowed")]
        public string ccv { get; set; }

       
        public DateTime expirationDate { get; set; }
        
        public decimal amount { get; set; }

        public string Name { get; set; }






    }
}
