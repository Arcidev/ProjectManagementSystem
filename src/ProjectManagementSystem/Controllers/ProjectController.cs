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

        [HttpGet("{id}")]
        public async Task<ProjectDTO> Get(int id)
        {
            return await projectFacade.GetProject(id);
        }
    }
}
