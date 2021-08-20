using AMDM.Data;
using AMDM.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task Register(int trainingId,string traineeId)
        {
            var training = _context.Training.Include(t =>t.Trainees).FirstOrDefault(t => t.Id == trainingId);
            if (training != null)
            {
                var trainee = _context.Trainee.Include(t => t.Trainings).FirstOrDefault(t => t.Id == traineeId);
                if (trainee!=null)
                {
                    if (training.Trainees == null)
                    {
                        training.Trainees = new List<Trainee>();
                    }
                    if(trainee.Trainings == null)
                    {
                        trainee.Trainings = new List<Training>();
                    }
                    if(!training.Trainees.Contains(trainee))
                        trainee.Trainings.Add(training);
                    if(!trainee.Trainings.Contains(training))
                        training.Trainees.Add(trainee);

                    //try
                    //{
                        await _context.SaveChangesAsync();
                    //}
                    //catch (Exception e) {
                    //    Console.WriteLine(e.InnerException.ToString());
                    //}
                }
                
            }
           

        }
        public async Task Unregister(int trainingId, string traineeId)
        {
            var training = _context.Training.Include(training => training.Trainees).FirstOrDefault(t => t.Id == trainingId);
            if (training != null)
            {
                var trainee = _context.Trainee.Include(trainee => trainee.Trainings).FirstOrDefault(t => t.Id == traineeId);
                if (trainee != null)
                {
                    if (training.Trainees != null)
                    {
                        training.Trainees.Remove(trainee);
                        await _context.SaveChangesAsync();
                    }
                    if (trainee.Trainings != null)
                    {
                        trainee.Trainings.Remove(training);
                        await _context.SaveChangesAsync();
                    }
                    

                    await _context.SaveChangesAsync();
                }

            }


        }
    }
}
