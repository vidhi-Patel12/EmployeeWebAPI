using EmployeeWebAPI.Models;

namespace EmployeeWebAPI.Interface
{
    public interface IEmployee
    {
        public List<EmployeeClass> GetEmployeeDetails();

        public EmployeeClass GetEmployeeDetailsById(int id);
        public void AddEmployee(EmployeeClass employee);
        public void UpdateEmployee(EmployeeClass employee);
        public EmployeeClass DeleteEmployee(int id);

        public bool CheckEmployee(int id);
        void GetEmployeeDetailsById(int id, CancellationToken none);
        void GetEmployeeDetailsById(EmployeeClass employeeClass);
        void GetEmployeeDetailsById();
    }
}

