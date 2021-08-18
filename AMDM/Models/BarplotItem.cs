using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class BarplotItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxParticipant { get; set; }
        public int ActualParticipant { get; set; }
    }
}
