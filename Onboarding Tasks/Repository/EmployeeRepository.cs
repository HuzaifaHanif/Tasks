using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task8.Data;
using Task8.Models;
using Task8.Models.Employees;
using Task8.Repository.IRepository;

namespace Task8.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SoftechWorldWideContext _context;

        public EmployeeRepository(SoftechWorldWideContext context) 
        {
            _context = context;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();

        }

        public async Task<int> UpdateEmployeeAsync(Employee employee)
        {
            _context.Attach(employee);

            if (!String.IsNullOrEmpty(employee.FirstName) && employee.FirstName != "string")
            {
                _context.Entry(employee).Property(e => e.FirstName).IsModified = true;
            }

            if (!String.IsNullOrEmpty(employee.LastName) && employee.LastName != "string")
            {
                _context.Entry(employee).Property(e => e.LastName).IsModified = true;
            }

            if (employee.HireDate != null)
            {
                _context.Entry(employee).Property(e => e.HireDate).IsModified = true;
            }

            if (employee.Salary != 0)
            {
                _context.Entry(employee).Property(e => e.Salary).IsModified = true;
            }

            return await _context.SaveChangesAsync();
        }
    }
}
