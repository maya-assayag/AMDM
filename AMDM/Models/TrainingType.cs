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
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Training> Trainings { get; set; }
    }
}
