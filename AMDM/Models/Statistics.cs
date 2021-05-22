using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class Statistics
    {
        [Key]
        public int Code { get; set; }

        [Display(Name ="AVG: Training in week")]

        public float TrainingAVGInWeek { get; set; }

        public int TraineeId { get; set; }
        public Trainee Trainee { get; set; }


    }
}
