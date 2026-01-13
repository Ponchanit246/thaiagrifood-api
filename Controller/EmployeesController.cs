using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammerTest_ThaiAgriFood.Data;
using ProgrammerTest_ThaiAgriFood.Models;


namespace ProgrammerTest_ThaiAgriFood.Controllers
{
    [Route("api/Employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly FoodDBContext _foodDBContext;
        private readonly IWebHostEnvironment _env;


        public EmployeesController(FoodDBContext foodDBContext, IWebHostEnvironment env)
        {
            _foodDBContext = foodDBContext;
            _env = env;
        }

        [HttpGet("GetDataGridEmployeeData")]
        public IActionResult GetDataGridEmployeeData(int page = 1, int pageSize = 30, string sort = null)
        {
            var query = _foodDBContext.Employees.AsQueryable();
            
            var total = query.Count();

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                data = data,
                total = total
            });
        }

        [HttpGet]
        [Route("Get/{Employee_ID}")]
        public async Task<ActionResult<Employee>> Get(string Employee_ID)
        {
            //return Ok(await _foodDBContext.Employees.ToListAsync());

            var model = _foodDBContext.Employees.Where(a => a.Employee_ID == Employee_ID).FirstOrDefault();
            return Ok(model);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] Employee model, IFormFile photo)
        {
            try
            {
                if (Exists(model.Employee_ID))
                    return BadRequest("รหัสพนักงานนี้มีอยู่แล้ว");

                if (photo != null && photo.Length > 0)
                {
                    var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadPath = Path.Combine(webRoot, "uploads");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var ext = Path.GetExtension(photo.FileName).ToLower();
                    var allowed = new[] { ".png", ".jpg", ".jpeg" };

                    if (!allowed.Contains(ext))
                        return BadRequest("รองรับเฉพาะไฟล์ .png, .jpg และ .jpeg เท่านั้น");

                    var datePrefix = DateTime.Now.ToString("yyMMdd-HHmmss");
                    var fileName = $"{datePrefix}-{model.Employee_ID}{ext}";
                    var newPath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    model.Photo = fileName;
                }

                model.Date_Joined = DateTime.Now;

                this._foodDBContext.Employees.Add(model);
                await this._foodDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost()]
        [Route("Update/{Employee_ID}")]
        public async Task<IActionResult> Update(string Employee_ID, [FromForm] Employee model, IFormFile photo)
        {
            try
            {
                var employee = await this._foodDBContext.Employees.FirstOrDefaultAsync(a => a.Employee_ID == Employee_ID);

                if (employee == null) return NotFound();

                if (photo != null && photo.Length > 0)
                {
                    var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadPath = Path.Combine(webRoot, "uploads");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    if (!string.IsNullOrEmpty(employee.Photo))
                    {
                        var oldPath = Path.Combine(uploadPath, employee.Photo);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    // สร้างชื่อไฟล์ใหม่
                    var ext = Path.GetExtension(photo.FileName);
                    var datePrefix = DateTime.Now.ToString("yyMMdd-HHmmss");
                    var fileName = $"{datePrefix}-{Employee_ID}{ext}";
                    var newPath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    employee.Photo = fileName;
                }


                employee.Employee_First_name = model.Employee_First_name;
                employee.Employee_Last_name = model.Employee_Last_name;
                employee.Gender = model.Gender;
                employee.Date_of_Birth = model.Date_of_Birth;
                employee.Employee_Address = model.Employee_Address;
                employee.Department_ID = model.Department_ID;

                await _foodDBContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete()]
        [Route("Delete/{Employee_ID}")]
        public async Task<ActionResult<Employee>> Delete(string Employee_ID)
        {
            var model = await _foodDBContext.Employees.SingleAsync(a => a.Employee_ID == Employee_ID);

            if (!string.IsNullOrEmpty(model.Photo))
            {
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadPath = Path.Combine(webRoot, "uploads");
                var filePath = Path.Combine(uploadPath, model.Photo);

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _foodDBContext.Employees.Remove(model);
            await _foodDBContext.SaveChangesAsync();
            return Ok();
        }

        private bool Exists(string Employee_ID)
        {
            return _foodDBContext.Employees.Any(e => e.Employee_ID == Employee_ID);
        }


    }
}
