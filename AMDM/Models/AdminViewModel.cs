using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class AdminViewModel
    {
        public IList<int> TicketsPie { get; set; }
        public User User { get; set; }
    }
}
