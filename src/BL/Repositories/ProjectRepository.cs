using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class ProjectRepository : BaseRepository<Project, int>
    {
        public ProjectRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        /// <summary>
        /// Get existing project with all references eagerly
        /// </summary>
        /// <param name="id">Id of project</param>
        /// <returns>Existing project</returns>
        internal async Task<Project> GetProject(int id)
        {
            return await GetProjectIncludes().FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Get existing project with all references eagerly
        /// </summary>
        /// <param name="code">Id of project</param>
        /// <returns>Existing project</returns>
        internal async Task<Project> GetProject(string code)
        {
            return await GetProjectIncludes().FirstOrDefaultAsync(x => x.Code == code);
        }

        /// <summary>
        /// Take subprojects/tasks/subtasks
        /// </summary>
        /// <returns>Queryable of projects</returns>
        private IQueryable<Project> GetProjectIncludes()
        {
            return Context.Projects.Include(x => x.SubProjects).ThenInclude(x => x.Tasks).ThenInclude(x => x.SubTasks).Include(x => x.Tasks).ThenInclude(x => x.SubTasks);
        }
    }
}
