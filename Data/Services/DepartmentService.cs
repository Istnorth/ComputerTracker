using System.Collections.Generic;
using System.Linq;
using ComputerTracker.Data.DbModel;
using Microsoft.EntityFrameworkCore;

namespace ComputerTracker.Data.Services
{
    public class DepartmentService
    {
        public List<Department> GetAllDepartments()
        {
            using (var context = new AppDbContext())
            {
                return context.Departments.ToList();
            }
        }

        public void AddDepartment(Department department)
        {
            using (var context = new AppDbContext())
            {
                context.Departments.Add(department);
                context.SaveChanges();
            }
        }

        public void UpdateDepartment(Department department)
        {
            using (var context = new AppDbContext())
            {
                context.Departments.Update(department);
                context.SaveChanges();
            }
        }

        public void DeleteDepartment(int departmentID)
        {
            using (var context = new AppDbContext())
            {
                var dept = context.Departments.Find(departmentID);
                if (dept != null)
                {
                    context.Departments.Remove(dept);
                    context.SaveChanges();
                }
            }
        }
    }
}
