using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(dbContext.Employees.ToList());

        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeesById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]

        
        public async Task<IActionResult> AddEmployee([FromForm] AddEmployeeDto addEmployeeDto)
        {
            string imagePath = null;
            if (addEmployeeDto.Image != null)
            {
                var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(addEmployeeDto.Image.FileName)}";
                 imagePath = Path.Combine("C:\\workspace\\EmployeeAdminPortal\\EmployeeAdminPortal\\", "image", imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await addEmployeeDto.Image.CopyToAsync(stream);
                }
            }

            var employeeEntity = new Employee()
            {
                Id = new Guid(),
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary,
                ImagePath = imagePath
            };

            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();
            return Ok(employeeEntity);
        }










        //dbContext.Employees.Add(employeeEntity);
        //    dbContext.SaveChanges();
        //    return Ok(employeeEntity);
        //}
        [HttpPost]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromForm] UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }

            string imagePath = employee.ImagePath;
            if (updateEmployeeDto.Image != null)
            {
                var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(updateEmployeeDto.Image.FileName)}";
                imagePath = Path.Combine("C:\\workspace\\EmployeeAdminPortal\\EmployeeAdminPortal\\", "image", imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await updateEmployeeDto.Image.CopyToAsync(stream);
                }

                
                if (!string.IsNullOrEmpty(employee.ImagePath))
                {
                    var oldImagePath = Path.Combine("wwwroot", employee.ImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
            }

            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;
            employee.ImagePath = imagePath;

            dbContext.SaveChanges();
            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }
            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();
            return Ok(employee);




        }
       

        
        [HttpGet]
        [Route("{id:guid}/download-image")]
        public IActionResult DownloadImage(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null || string.IsNullOrEmpty(employee.ImagePath))
            {
                return NotFound();
            }

            var imagePath = Path.Combine("C:\\workspace\\EmployeeAdminPortal\\EmployeeAdminPortal", employee.ImagePath);
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var image = System.IO.File.OpenRead(imagePath);
            var mimeType = GetMimeType(imagePath);
            return File(image, mimeType, Path.GetFileName(imagePath));
        }

        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".tiff" => "image/tiff",
                ".webp" => "image/webp",
                ".avif" => "image/avif",
                _ => "application/octet-stream",
            };
        }
    }
}
    

