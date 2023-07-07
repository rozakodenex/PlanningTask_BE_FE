using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlanningTasksEF.Data;
using PlanningTasksEF.Models;
using System.Net;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace PlanningTasksEF.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<Tasks> _logger;
        private readonly DataContext _dbContext;

        public TasksController(ILogger<Tasks> logger, DataContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = _dbContext.Tasks;
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            try
            {
                Tasks task = JsonConvert.DeserializeObject<Tasks>(values);

                _dbContext.Tasks.Add(task);
                await _dbContext.SaveChangesAsync();

                return Ok(task);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var task = await _dbContext.Tasks.FirstAsync(o => o.Id == key);
            JsonConvert.PopulateObject(values, task);

            if (!TryValidateModel(task))
                return BadRequest("Data not valid!");

            await _dbContext.SaveChangesAsync();

            return Ok(task);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var studentToDelete = await _dbContext.Tasks.FindAsync(id);
            if (studentToDelete == null)
            {
                return NotFound();
            }
            _dbContext.Tasks.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
