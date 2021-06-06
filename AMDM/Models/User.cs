using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public enum UserType
    {
        Nothing,
        Trainee,
        Trainer,
        Admin

    }
    public class User
    {
        [Key]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "The password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        public UserType Type { get; set; } //= UserType.Nothing;
    }
}
