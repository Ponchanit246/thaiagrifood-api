using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammerTest_ThaiAgriFood.Data;
using ProgrammerTest_ThaiAgriFood.Models;


namespace ProgrammerTest_ThaiAgriFood.Controllers
{
    [Route("api/Departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly FoodDBContext _foodDBContext;

        public DepartmentsController(FoodDBContext foodDBContext)
        {
            _foodDBContext = foodDBContext;
        }

        [HttpGet("GetDataGridDepartmentData")]
        public IActionResult GetDataGridDepartmentData(int page = 1, int pageSize = 30, string sort = null)
        {
            var query = _foodDBContext.Departments.AsQueryable();

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

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _foodDBContext.Departments.ToListAsync());
        }

        [HttpGet]
        [Route("GetDepartment/{Department_ID}")]
        public async Task<ActionResult<Department>> GetDepartment(string Department_ID)
        {
            var model = _foodDBContext.Departments.Where(a => a.Department_ID == Department_ID).FirstOrDefault();
            return Ok(model);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] Department model)
        {
            try
            {
                if (Exists(model.Department_ID))
                    return BadRequest("รหัสแผนกนี้มีอยู่แล้ว");

                this._foodDBContext.Departments.Add(model);
                await this._foodDBContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("UseCheck/{Department_ID}")]
        public ActionResult UseCheck(string Department_ID)
        {
            var useCheck = _foodDBContext.Employees.Any(a => a.Department_ID == Department_ID);
            return Ok(useCheck);
        }

        [HttpPost()]
        [Route("Update/{Department_ID}")]
        public async Task<IActionResult> Update(string Department_ID, [FromForm] Department Model)
        { 
            var dep = await this._foodDBContext.Departments.FirstOrDefaultAsync(a => a.Department_ID == Department_ID);

            if (dep == null) return NotFound();

            dep.Department_Name = Model.Department_Name;
            dep.Department_Address = Model.Department_Address;

            await _foodDBContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete()]
        [Route("Delete/{Department_ID}")]
        public async Task<ActionResult<Department>> Delete(string Department_ID)
        {
            var model = await _foodDBContext.Departments.SingleAsync(a => a.Department_ID == Department_ID);
            _foodDBContext.Departments.Remove(model);
            await _foodDBContext.SaveChangesAsync();
            return Ok();
        }

        private bool Exists(string Department_ID)
        {
            return _foodDBContext.Departments.Any(e => e.Department_ID == Department_ID);
        }

    }
}
