using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMDM.Models;
using Microsoft.AspNetCore.SignalR;

namespace AMDM.Hubs
{
    public class TrainingHub : Hub
    {
        public async Task  UpdatePlaceLeft(Training training)
        {
            await Clients.All.SendAsync("UpdatePlaceLeft", training);
        }
    }
}
