using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task8.Data;
using Task8.Models;
using System.Net;
using AutoMapper;
using Task8.Models.Employees;
using Task8.Repository.IRepository;

namespace Task8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly SoftechWorldWideContext _context;
        private readonly APIResponse response;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employee;

        public EmployeeController(SoftechWorldWideContext context , IMapper mapper , IEmployeeRepository employee) 
        {
            _context = context;
            _mapper = mapper;
            this.response = new APIResponse();
            _employee = employee;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetEmployees()
        {
            response.Result = await _employee.GetAllEmployeesAsync();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }

        [HttpPost("AddEmployee")]
        public async Task<ActionResult<APIResponse>> GetEmployees([FromBody] AddEmployee addEmpObj)
        {
            Employee newEmployee = _mapper.Map<Employee>(addEmpObj);

            await _employee.AddEmployeeAsync(newEmployee);

            response.Result = "Employee sucessfully added";
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }


        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> UpdateEmployee(int id , [FromBody] UpdateEmployee reqUpdateObj)
        {
            if( id == 0)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            Employee empObj = _mapper.Map<Employee>(reqUpdateObj);
            empObj.Id = id;

            var rows = _employee.UpdateEmployeeAsync(empObj);

            if (rows == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.ErrorMessages = new List<string>
                {
                    "no id exsists in DataBase"
                };
                return NotFound(response);
            }

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.ErrorMessages = new List<string>
            {
                $"Employee with id {id} updated."
            };
            return Ok(response);

        }
    }
}
