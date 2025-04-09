using ComputerTracker.Data.DbModel;
using Microsoft.EntityFrameworkCore;

namespace ComputerTracker.Services
{
    public class EmployeeService
    {
        public List<Employee> GetAllEmployees()
        {
            using (var context = new AppDbContext())
            {
                return context.Employees.Include(e => e.Department).ToList();
            }
        }

        public void AddEmployee(Employee employee)
        {
            using (var context = new AppDbContext())
            {
                context.Employees.Add(employee);
                context.SaveChanges();
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            using (var context = new AppDbContext())
            {
                context.Employees.Update(employee);
                context.SaveChanges();
            }
        }

        public void DeleteEmployee(int employeeID)
        {
            using (var context = new AppDbContext())
            {
                var emp = context.Employees.Find(employeeID);
                if (emp != null)
                {
                    context.Employees.Remove(emp);
                    context.SaveChanges();
                }
            }
        }
    }
}
