using EmployeeWebAPI.Interface;
using EmployeeWebAPI.Models;
using Intuit.Ipp.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWebAPI.Repository
{
    public class EmployeeRepo : IEmployee
    {
       private readonly DatabaseContext _dbContext;

        public EmployeeRepo(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<EmployeeClass> GetEmployeeDetails()
        {
            try
            {
                return _dbContext.Employee.ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeeClass GetEmployeeDetailsById(int id)
        {
            try
            {
                EmployeeClass? employee = _dbContext.Employee.Find(id);
                if (employee != null)
                {
                    return employee;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public void AddEmployee(EmployeeClass employee)
        {
            try
            {
                _dbContext.Employee.Add(employee);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateEmployee(EmployeeClass employee)
        {
            try
            {
                _dbContext.Entry(employee).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public EmployeeClass DeleteEmployee(int id)
        {
            try
            {
                EmployeeClass? employee = _dbContext.Employee.Find(id);

                if (employee != null)
                {
                    _dbContext.Employee.Remove(employee);
                    _dbContext.SaveChanges();
                    return employee;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public bool CheckEmployee(int id)
        {
            return _dbContext.Employee.Any(e => e.EmployeeId == id);
        }

        public void GetEmployeeDetailsById(int id, CancellationToken none)
        {
            throw new NotImplementedException();
        }

        public void GetEmployeeDetailsById(EmployeeClass employeeClass)
        {
            throw new NotImplementedException();
        }

        public void GetEmployeeDetailsById()
        {
            throw new NotImplementedException();
        }
    }
}