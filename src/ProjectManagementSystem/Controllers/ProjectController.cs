using BL.DTO;
using BL.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectFacade projectFacade;

        public ProjectController(ProjectFacade projectFacade)
        {
            this.projectFacade = projectFacade;
        }

        [HttpGet("{id:int}")]
        public async Task<ProjectDTO> Get(int id)
        {
            return await projectFacade.GetProject(id);
        }

        [HttpGet("{code}")]
        public async Task<ProjectDTO> Get(string code)
        {
            return await projectFacade.GetProject(code);
        }

        [HttpPost]
        public async Task<ProjectDTO> Add(ProjectDTO project)
        {
            await projectFacade.AddProject(project);
            return project;
        }

        [HttpPut]
        public async Task Update(ProjectDTO project)
        {
            await projectFacade.UpdateProject(project);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await projectFacade.DeleteProject(id);
        }
    }
}
