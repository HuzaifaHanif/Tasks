using Microsoft.AspNetCore.Mvc;
using Task8.Models;
using Task8.Models.Employees;

namespace Task8.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        public Task<List<Employee>> GetAllEmployeesAsync();

        public Task<int> UpdateEmployeeAsync(Employee employee);

        public Task AddEmployeeAsync(Employee employee);

    }
}
