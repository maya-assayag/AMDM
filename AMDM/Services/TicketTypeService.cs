using AMDM.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Services
{
    public class TicketTypeService
    {
        private readonly AMDMContext _context;
        public TicketTypeService(AMDMContext context)
        {
            _context = context;
        }

        public async Task Purchase(int ticketTypeId, string traineeId)
        {
            //_context.Ticket.wh
            var ticketType = _context.TicketType.FirstOrDefault(t => t.Id == ticketTypeId);
            if (ticketType != null)
            {
                //_context.Trainee
                //.Include(t => t.Ticket)
                //.Include(t => t.Trainings)
                //.FirstOrDefault(t =>
                //              t.Id == traineeId);
                var trainee = _context.Trainee.Include(t=> t.Ticket).FirstOrDefault(t => t.Id == traineeId);
                if (trainee != null)
                {
                    //if (trainee.Ticket == null)
                    //{
                        trainee.Ticket = new Models.Ticket();
                        trainee.Ticket.PurchaseDate = DateTime.UtcNow;
                        trainee.Ticket.TicketTypeId = ticketTypeId;
                        trainee.Ticket.TicketType = ticketType;
                        trainee.Ticket.Trainee = trainee;
                        if(ticketType.PunchingHolesNumber!=null)
                        {
                            trainee.Ticket.RemainingPunchingHoles = (int)ticketType.PunchingHolesNumber;
                        }

                        
                        if (ticketType.Name == "Month" || ticketType.Name == "Free Monthly" || ticketType.Name == "Trial")
                        {
                            trainee.Ticket.ExpiredDate = DateTime.UtcNow.AddMonths(1);
                        }
                        else if(ticketType.Name == "Year")
                        {
                            trainee.Ticket.ExpiredDate = DateTime.UtcNow.AddYears(1);
                        }
                        else if (ticketType.Name == "Week")
                        {
                            trainee.Ticket.ExpiredDate = DateTime.UtcNow.AddDays(7);
                        }
                        if(ticketType.Tickets==null)
                        {
                            ticketType.Tickets = new List<Models.Ticket>();
                        }

                        ticketType.Tickets.Add(trainee.Ticket);
                        await _context.SaveChangesAsync();

                    //}

                    //training.Trainees.Add(trainee);
                    //await _context.SaveChangesAsync();
                }

            }


        }
    }
}
