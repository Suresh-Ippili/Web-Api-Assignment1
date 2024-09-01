using Microsoft.AspNetCore.Mvc;
using TaskApi.DataAccess;
using TaskApi.Models;
using System.Collections.Generic;
using Task = TaskApi.Models.Task;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskRepository _taskRepository;

        public TaskController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/Task
        [HttpGet]
        public IEnumerable<Task> GetTasks()
        {
            return _taskRepository.GetTasks();
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public ActionResult<Task> GetTask(int id)
        {
            var task = _taskRepository.GetTaskById(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST: api/Task
        [HttpPost]
        public ActionResult<Task> PostTask(Task task)
        {
            _taskRepository.AddTask(task);
            return CreatedAtAction(nameof(GetTask), new { id = task.TaskID }, task);
        }

        // PUT: api/Task/5
        [HttpPut("{id}")]
        public IActionResult PutTask(int id, Task task)
        {
            if (id != task.TaskID)
            {
                return BadRequest();
            }

            _taskRepository.UpdateTask(task);
            return NoContent();
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _taskRepository.GetTaskById(id);

            if (task == null)
            {
                return NotFound();
            }

            _taskRepository.DeleteTask(id);
            return NoContent();
        }
    }
}