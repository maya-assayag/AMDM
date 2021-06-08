using AMDM.Data;
using AMDM.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Services
{
    public class TraineeService
    {

        private readonly AMDMContext _context;
        public TraineeService(AMDMContext context)
        {
            _context = context;
        }
        //public void ThereIsATicket(string id, HttpContext context)
        //{
        //    var ticket = _context.Ticket.FirstOrDefault(t=> t.Trainee.Id == id);
        //    if(ticket!=null)
        //    {
        //        context.Session.SetString("Name", name);
        //        context.Session.SetString("Id", id);
        //        return "your ticket is valid until:" + ticket.ExpiredDate;
        //    }
        //    else
        //    {
        //        return "You do not have a ticket to purchase a ticket";
        //    }
        //}
    }
}
