using AMDM.Data;
using AMDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Services
{
    public class TrainingService
    {
        private readonly AMDMContext _context;
        public TrainingService(AMDMContext context)
        {
            _context = context;
        }

        public async void Register(Training training,string traineeId)
        {
            var trainee = _context.Trainee.FirstOrDefault(t => t.Id == traineeId);
            training.Trainees.Add(trainee);

        }
    }
}
