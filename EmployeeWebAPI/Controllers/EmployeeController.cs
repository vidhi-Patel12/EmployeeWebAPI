using EmployeeWebAPI.Interface;
using EmployeeWebAPI.Models;
using EmployeeWebAPI.ViewModel;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Task = System.Threading.Tasks.Task;

namespace EmployeeWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _IEmployee;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly DatabaseContext _dbContext;


        public EmployeeController(IEmployee IEmployee, IWebHostEnvironment webHostEnvironment, DatabaseContext dbContext)
        {
            _IEmployee = IEmployee;
            WebHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }

        // GET: api/Get>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeClass>>> Get()
        {
            return await Task.FromResult(_IEmployee.GetEmployeeDetails());
        }

        // GET api/Get/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeClass>> Get(int id)
        {
            var employees = await Task.FromResult(_IEmployee.GetEmployeeDetailsById(id));
            if (employees == null)
            {
                return NotFound();
            }
            return employees;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] EmployeeViewModel vm)
        {
            if (vm == null)
            {
                return BadRequest("Invalid submission.");
            }

            string stringfilename = UploadFile(vm);
            var employee = new EmployeeClass
            {

                EmployeeName = vm.EmployeeName,
                BirthDate = vm.BirthDate,
                Gender = vm.Gender,
                Email = vm.Email,
                Password = vm.Password,
                Image = stringfilename,
            };

            _dbContext.Employee.Add(employee);
            await _dbContext.SaveChangesAsync();

            return Ok(employee);
        }

        private string UploadFile(EmployeeViewModel vm)
        {
            string filename = null;
            if (vm.Image!= null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
                filename = Guid.NewGuid().ToString() + "-" + vm.Image.FileName;
                string filepath = Path.Combine(uploadDir, filename);
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    vm.Image.CopyTo(fileStream);
                }
            }
         
            return filename;
        }


       
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] EmployeeViewModel vm)
        {
            if (vm == null)
            {
                return BadRequest("Invalid submission.");
            }

            var employee = await _dbContext.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            

            string stringfilename = employee.Image; // Retain existing image filename
            if (vm.Image != null)
            {
                stringfilename = UploadFile(vm);
            }

            employee.EmployeeName = vm.EmployeeName;
            employee.BirthDate = vm.BirthDate;
            employee.Gender = vm.Gender;
            employee.Email = vm.Email;
            employee.Password = vm.Password;
            employee.Image = stringfilename;

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(employee);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound("Employee not found during update.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(employee);
            }

            return BadRequest(ModelState);
        }

      
        // DELETE api/employee/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeClass>> Delete(int id)
        {
            var employee = _IEmployee.DeleteEmployee(id);
            return await Task.FromResult(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _IEmployee.CheckEmployee(id);
        }
    }
}
