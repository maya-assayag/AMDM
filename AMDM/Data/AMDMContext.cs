using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AMDM.Models;
using WebApplication1.Models;

namespace AMDM.Data
{
    public class AMDMContext : DbContext
    {
        public AMDMContext (DbContextOptions<AMDMContext> options)
            : base(options)
        {
        }

        public DbSet<AMDM.Models.Ticket> Ticket { get; set; }

        public DbSet<AMDM.Models.TicketType> TicketType { get; set; }

        public DbSet<AMDM.Models.Trainee> Trainee { get; set; }

        public DbSet<AMDM.Models.Trainer> Trainer { get; set; }

        public DbSet<AMDM.Models.Training> Training { get; set; }

        public DbSet<AMDM.Models.TrainingType> TrainingType { get; set; }

        public DbSet<AMDM.Models.User> User { get; set; }

        public DbSet<WebApplication1.Models.Place> Places { get; set; }
    }
}
