using AMDM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Services
{
    public class TicketService
    {

        private readonly AMDMContext _context;
        public TicketService(AMDMContext context)
        {
            _context = context;
        }
    }
}
