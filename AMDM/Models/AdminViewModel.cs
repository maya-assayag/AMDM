using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class AdminViewModel
    {
        //public IList<int> TicketsTypesPie { get; set; }
        //public IList<string> TicketsTypesNames { get; set; }
        public IList<KeyValuePair<string, int>> TicketsTypesPurchasedLollipop { get; set; }

        public User User { get; set; }

        [DataType(DataType.Currency)]
        public double SumOfRevenueThisMonth { get; set; }

        public int SumOfTicketPurchasedThisMonth { get; set; }

        public int ActiveTrainees { get; set; }

        public int AllTrainees { get; set; }
    }
}
