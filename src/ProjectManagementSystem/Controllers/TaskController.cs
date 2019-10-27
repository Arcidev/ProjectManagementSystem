using BL.DTO;
using BL.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskFacade taskFacade;

        public TaskController(TaskFacade taskFacade)
        {
            this.taskFacade = taskFacade;
        }

        [HttpGet("{id}")]
        public async Task<TaskDTO> Get(int id)
        {
            return await taskFacade.GetTask(id);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IEnumerable<TaskDTO>> GetByProject(int projectId)
        {
            return await taskFacade.GetTasksByProject(projectId);
        }

        [HttpPost]
        public async Task<TaskDTO> Add(TaskDTO task)
        {
            await taskFacade.AddTask(task);
            return task;
        }

        [HttpPut]
        public async Task Update(TaskDTO task)
        {
            await taskFacade.UpdateTask(task);
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await taskFacade.DeleteTask(id);
        }
    }
}
