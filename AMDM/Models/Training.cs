using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class Training
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Training type")]
        public int TrainingTypeId { get; set; }

        public TrainingType TrainingType { get; set; }

        [Display(Name = "Trainer id")]
        public string TrainerId { get; set; }

        public Trainer Trainer { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        [Required]
        public DateTime Time { get; set; }

        [Required]
        [RegularExpression("^[A-Z]+[a-zA-Z0-9, ]*$", ErrorMessage = "You must begins with a capital letter, the allowed characters are digits, english letters, comma marks and spaces")]
        public string Studio { get; set; }

        [Range(1, 20)]
        [Required]
        public int MaxRegisterTrainees { get; set; }
        public List<Trainee> Trainees { get; set; }

        public int TotalTraineesLeft
        {
            get
            {
                return Trainees != null ? MaxRegisterTrainees - Trainees.Count() : MaxRegisterTrainees;
            }
        }
    }
}
