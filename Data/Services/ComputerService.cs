using System.Collections.Generic;
using System.Linq;
using ComputerTracker.Data.DbModel;
using Microsoft.EntityFrameworkCore;

namespace ComputerTracker.Data.Services
{
    public class ComputerService
    {
        public List<Computer> GetAllComputers()
        {
            using (var context = new AppDbContext())
            {
                return context.Computers.ToList();
            }
        }

        public void AddComputer(Computer computer)
        {
            try
            {
                using (var context = new AppDbContext())
                {

                    context.Computers.Add(computer);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public void UpdateComputer(Computer computer)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    context.Computers.Update(computer);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
        

        public void DeleteComputer(int computerID)
        {
            using (var context = new AppDbContext())
            {
                var comp = context.Computers.Find(computerID);
                if (comp != null)
                {
                    context.Computers.Remove(comp);
                    context.SaveChanges();
                }
            }
        }
    }
}
