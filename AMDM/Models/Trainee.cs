using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    
    public class Trainee
    {
        public enum Gender
        {
            Female,
            Male
        }
        [StringLength(9, MinimumLength = 9)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The input must be digits")]
        [Key] public string Id { get; set; }

        [Required]
        [RegularExpression("^[A-Z]+[a-zA-Z ]*$", ErrorMessage = "You must begins with a capital letter, the allowed characters are english letters and spaces")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[A-Z]+[a-zA-Z ]*$", ErrorMessage = "You must begins with a capital letter, the allowed characters are english letters and spaces")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Range(50, 250)]
        public double Height { get; set; }
        [Range(20, 300)]
        public double Weight { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "The password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        [Display(Name = "Phone number")]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The input must be digits")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Gender")]
        public Gender TraineeGender { get; set; }

        public List<Training> Trainings { get; set; }

        public Ticket Ticket { get; set; }
    }
}
