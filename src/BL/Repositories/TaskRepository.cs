using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class TaskRepository : BaseRepository<ProjectTask, int>
    {
        public TaskRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        internal async Task<ProjectTask> GetTask(int id)
        {
            return await Context.Tasks.Include(x => x.SubTasks).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
