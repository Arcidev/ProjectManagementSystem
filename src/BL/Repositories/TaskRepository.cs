using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class TaskRepository : BaseRepository<ProjectTask, int>
    {
        public TaskRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        /// <summary>
        /// Get task with included subtasks
        /// </summary>
        /// <param name="id">Id of task</param>
        /// <returns>Required task</returns>
        internal async Task<ProjectTask> GetTask(int id)
        {
            return await Context.Tasks.Include(x => x.SubTasks).FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Deletes task with its internal referencies
        /// </summary>
        /// <param name="id">Id of the task</param>
        internal async Task DeleteProject(int id)
        {
            var task = await GetTask(id);
            if (task == null)
                return;

            Context.Tasks.RemoveRange(task.SubTasks);
            Context.Tasks.Remove(task);
        }
    }
}
