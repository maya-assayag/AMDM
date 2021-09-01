using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class TrainingType
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression("^[A-Z]+[a-zA-Z ]*$", ErrorMessage = "You must begins with a capital letter, the allowed characters are english letters and spaces")]
        [Required]
        public string Name { get; set; }

        public List<Training> Trainings { get; set; }
    }
}
