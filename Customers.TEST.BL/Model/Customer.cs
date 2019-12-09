using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customers.Model
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use only Letters")]
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "First Name has to be between 3 and 250 Characters only")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use only Letters")]
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Last Name has to be between 3 and 250 Characters only")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Date of Birth is required")]        
        public DateTime DateOfBirth { get; set; }
    }
}
