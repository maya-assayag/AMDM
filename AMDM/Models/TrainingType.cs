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

        [RegularExpression("^[A-Z]+[a-zA-Z0-9, ]*$", ErrorMessage = "You must input a valid name begins with a capital letter")]
        [Required]
        public string Name { get; set; }

        public List<Training> Trainings { get; set; }
    }
}
