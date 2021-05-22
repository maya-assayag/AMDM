using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public enum Gender
    {
        Female,
        Male
    }
    public class Trainee
    {
       
        [StringLength(9, MinimumLength = 9)]
        [Key] public string Id { get; set; }

        [Required]
        [RegularExpression("^[A-Z]+[a-zA-Z ]*$"]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[A-Z]+[a-zA-Z ]*$"]
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
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }

        [Required]
       
        public Gender TraineeGender { get; set; }

        public List<Training> Trainings { get; set; }
        public List<Ticket> Tickets { get; set; }

    }
}
